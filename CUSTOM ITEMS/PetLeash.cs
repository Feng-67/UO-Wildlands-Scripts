/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Xanthos)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;
using Server.ContextMenus;

namespace ShrinkSystem
{
    #region Interfaces
    public interface IShrinkTool : IEntity
    {
        bool DeleteWhenEmpty { get; set; }
        int ShrinkCharges { get; set; }
    }

    public interface IShrinkItem : IEntity
    {
        BaseCreature ShrunkenPet { get; }
    }
    #endregion

    #region Configuration
    public static class ShrinkSettings
    {
        // Settings formerly in ShrinkConfig.xml
        public static bool PetAsStatuette = true;
        public static bool AllowLocking = false;
        public static bool ShowPetDetails = true; // Set to true to show Name/Type
        public static double ShrunkenWeight = 10.0;
        public static bool BlessedLeash = true;
        public static int TamingRequired = 0;
        public static int ShrinkCharges = 100; // -1 for infinite

        public enum BlessStatus { All, BondedOnly, None }
        public static BlessStatus LootStatus = BlessStatus.All;
    }
    #endregion

    #region Shrink Table (Improved)
    public class ShrinkTable
    {
        public const int DefaultItemID = 0x1870; // Yellow virtue stone fallback
        private static Dictionary<int, int> m_Table;

        public static int Lookup(Mobile m)
        {
            if (m_Table == null) Load();

            if (m_Table.TryGetValue(m.Body.BodyID, out int itemID))
                return itemID;

            return DefaultItemID;
        }

        private static void Load()
        {
            m_Table = new Dictionary<int, int>();
            string path = Path.Combine(Core.BaseDirectory, "Data/shrink.cfg");

            if (!File.Exists(path)) return;

            using (StreamReader ip = new StreamReader(path))
            {
                string line;
                while ((line = ip.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0 || line.StartsWith("#")) continue;

                    string[] split = line.Split('\t');
                    if (split.Length >= 2)
                    {
                        int body = Utility.ToInt32(split[0]);
                        int item = Utility.ToInt32(split[1]);
                        m_Table[body] = item;
                    }
                }
            }
        }
    }
    #endregion

    #region The Pet Leash Tool
    public class PetLeash : Item, IShrinkTool
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public int ShrinkCharges { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeleteWhenEmpty { get; set; }

        [Constructable]
        public PetLeash() : this(ShrinkSettings.ShrinkCharges) { }

        [Constructable]
        public PetLeash(int charges) : base(0x1374)
        {
            Name = "Pet Leash";
            Hue = 1153;
            Weight = 1.0;
            ShrinkCharges = charges;
            DeleteWhenEmpty = true; // This ensures it disappears at 0 charges
            LootType = ShrinkSettings.BlessedLeash ? LootType.Blessed : LootType.Regular;
        }

        public PetLeash(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            else
                ShrinkTarget.Begin(from, this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (ShrinkCharges >= 0)
                list.Add(1060658, "Charges\t{0}", ShrinkCharges);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(ShrinkCharges);
            writer.Write(DeleteWhenEmpty);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            ShrinkCharges = reader.ReadInt();
            DeleteWhenEmpty = reader.ReadBool();
        }
    }
    #endregion

    #region The Shrunken Pet Item
    public class ShrinkItem : Item, IShrinkItem
    {
        private BaseCreature m_Pet;
        private Mobile m_Owner;
        private bool m_Locked;

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseCreature ShrunkenPet => m_Pet;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Locked { get => m_Locked; set => m_Locked = value; }

        public ShrinkItem(BaseCreature pet) : base()
        {
            m_Pet = pet;
            m_Owner = pet.ControlMaster;
            Weight = ShrinkSettings.ShrunkenWeight;
            Name = pet.Name + (pet.Title != null ? " " + pet.Title : "");

            if (ShrinkSettings.PetAsStatuette)
            {
                ItemID = ShrinkTable.Lookup(m_Pet);
                Hue = m_Pet.Hue;
            }
            else
            {
                ItemID = 0x0E2D; // Leash icon
            }

            // Apply Blessing based on config
            if (ShrinkSettings.LootStatus == ShrinkSettings.BlessStatus.All ||
               (ShrinkSettings.LootStatus == ShrinkSettings.BlessStatus.BondedOnly && pet.IsBonded))
                LootType = LootType.Blessed;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Pet != null)
            {
                // Simplified Tooltip: Only Name and Type
                list.Add(1060658, "Pet Name\t{0}", m_Pet.Name);
                list.Add(1060659, "Pet Type\t{0}", m_Pet.GetType().Name);
                list.Add(1060660, "Note\tUse Animal Lore for full stats");
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
                return;
            }

            if (m_Locked && from != m_Owner && from.AccessLevel == AccessLevel.Player)
            {
                from.SendMessage("This pet is locked to its owner.");
                return;
            }

            if (from.Followers + m_Pet.ControlSlots > from.FollowersMax)
            {
                from.SendMessage("You have too many followers to reclaim this pet.");
                return;
            }

            m_Pet.SetControlMaster(from);
            m_Pet.IsStabled = false;
            m_Pet.MoveToWorld(from.Location, from.Map);
            m_Pet.ControlOrder = OrderType.Follow;

            this.Delete();
        }

        public ShrinkItem(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(m_Pet);
            writer.Write(m_Owner);
            writer.Write(m_Locked);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_Pet = reader.ReadMobile<BaseCreature>();
            m_Owner = reader.ReadMobile();
            m_Locked = reader.ReadBool();

            if (m_Pet != null) m_Pet.IsStabled = true;
        }
    }
    #endregion

    #region Shrink Logic & Commands
    public static class ShrinkTarget
    {
        public static void Begin(Mobile from, IShrinkTool tool)
        {
            if (from.Skills[SkillName.AnimalTaming].Value < ShrinkSettings.TamingRequired && from.AccessLevel < AccessLevel.GameMaster)
            {
                from.SendMessage("You lack the taming skill to use this.");
                return;
            }

            from.SendMessage("Target the pet you wish to shrink.");
            from.BeginTarget(-1, false, TargetFlags.None, (m, targeted) =>
            {
                if (targeted is BaseCreature pet)
                {
                    if (!pet.Controlled || pet.ControlMaster != m)
                        m.SendMessage("That is not your pet.");
                    else if (pet.IsDeadPet)
                        m.SendMessage("You cannot shrink the dead.");

                    // MODIFIED SECTION: Only block if it's a pack animal WITH items.
                    // Regular pets (Dragons, etc.) will skip this and shrink even with loot.
                    else if ((pet is PackHorse || pet is PackLlama || pet is Beetle) && pet.Backpack != null && pet.Backpack.Items.Count > 0)
                    {
                        m.SendMessage("You must empty this pack animal's storage before it can be shrunken.");
                    }

                    else
                    {
                        if (tool != null && tool.ShrinkCharges > 0) tool.ShrinkCharges--;

                        ShrinkItem item = new ShrinkItem(pet);
                        m.AddToBackpack(item);
                        pet.ControlTarget = null;
                        pet.ControlOrder = OrderType.Stay;
                        pet.Internalize();
                        pet.SetControlMaster(null);
                        pet.SummonMaster = null;
                        pet.IsStabled = true;
                        m.PlaySound(492);
                        m.SendMessage("The pet has been shrunken into your pack.");

                        if (tool != null && tool.ShrinkCharges == 0 && tool.DeleteWhenEmpty)
                            tool.Delete();
                    }
                }
                else m.SendMessage("That is not a pet.");
            });
        }
    }

    public class ShrinkCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("Shrink", AccessLevel.GameMaster, e => ShrinkTarget.Begin(e.Mobile, null));
        }
    }
    #endregion
}
