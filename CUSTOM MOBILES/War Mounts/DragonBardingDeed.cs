/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
namespace Server.Mobiles
{
    public interface IBardedMount
    {
        bool BardingExceptional { get; set; }
        Mobile BardingCrafter { get; set; }
        int BardingHP { get; set; }
        bool HasBarding { get; set; }
        CraftResource BardingResource { get; set; }
        int BardingMaxHP { get; }
    }
}

namespace Server.Items
{
    [TypeAlias("Server.Items.DragonBarding")]
    public class DragonBardingDeed : Item, ICraftable
    {
        private bool m_Exceptional;
        private Mobile m_Crafter;
        private CraftResource m_Resource;

        public override int LabelNumber
        {
            get { return this.m_Exceptional ? 1053181 : 1053012; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get { return this.m_Crafter; }
            set { this.m_Crafter = value; this.InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Exceptional
        {
            get { return this.m_Exceptional; }
            set { this.m_Exceptional = value; this.InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return this.m_Resource; }
            set
            {
                this.m_Resource = value;
                this.Hue = CraftResources.GetHue(value);
                this.InvalidateProperties();
            }
        }

        [Constructable]
        public DragonBardingDeed() : base(0x14F0)
        {
            this.Weight = 1.0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (this.m_Exceptional && this.m_Crafter != null)
                list.Add(1050043, m_Crafter.TitleName);
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (m_Crafter != null)
            {
                LabelTo(from, 1050043, m_Crafter.TitleName);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.IsChildOf(from.Backpack))
            {
                from.BeginTarget(6, false, TargetFlags.None, new TargetCallback(OnTarget));
                from.SendMessage("Select the tamed mount you wish to place the barding on.");
            }
            else
            {
                from.SendLocalizedMessage(1042001);
            }
        }

        public virtual void OnTarget(Mobile from, object obj)
        {
            if (this.Deleted)
                return;

            BaseMount mount = obj as BaseMount;
            IBardedMount barded = obj as IBardedMount;

            if (mount == null || barded == null)
            {
                from.SendMessage("That is not a creature that can wear dragon barding.");
                return;
            }

            if (barded.HasBarding)
            {
                from.SendMessage("That mount is already wearing barding.");
                return;
            }

            if (!mount.Controlled || mount.ControlMaster != from)
            {
                from.SendMessage("You can only put barding on a tamed mount that you own.");
                return;
            }

            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640);
                return;
            }

            // Apply barding properties
            barded.BardingExceptional = this.Exceptional;
            barded.BardingCrafter = this.Crafter;
            barded.BardingResource = this.Resource;
            barded.HasBarding = true;
            barded.BardingHP = barded.BardingMaxHP;

            // Special handling for SwampDragon - set hue directly
            if (mount is SwampDragon)
            {
                mount.Hue = this.Hue;
            }

            this.Delete();

            // Use mount-specific message
            if (mount is SwampDragon)
            {
                from.SendLocalizedMessage(1053027);
            }
            else
            {
                from.SendMessage("You place the barding on your mount. Use a bladed item on your mount to remove the armor.");
            }
        }

        public DragonBardingDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((bool)this.m_Exceptional);
            writer.Write((Mobile)this.m_Crafter);
            writer.Write((int)this.m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                case 0:
                    {
                        this.m_Exceptional = reader.ReadBool();
                        this.m_Crafter = reader.ReadMobile();

                        if (version < 1)
                            reader.ReadInt();

                        this.m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
            }
        }

        #region ICraftable Members

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            this.Exceptional = (quality >= 2);

            if (makersMark)
                this.Crafter = from;

            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            this.Resource = CraftResources.GetFromType(resourceType);
            return quality;
        }

        #endregion
    }
}
