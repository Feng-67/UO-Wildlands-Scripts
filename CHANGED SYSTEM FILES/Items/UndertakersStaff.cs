/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class UndertakersStaff : Item
    {
        private int m_Charges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set { m_Charges = value; InvalidateProperties(); }
        }

        [Constructable]
        public UndertakersStaff() : base(0xE89)
        {
            Weight = 7.0;
            Hue = 0x482;
            Name = "Undertaker's Staff";
            LootType = LootType.Blessed;
            m_Charges = 100;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            // This replaces the old "40 tile" description with your custom text
            list.Add("Special Ability: Global Corpse Recovery"); 
            list.Add("Range: Unlimited (Must be on the same map)"); 

            // This displays the charges
            list.Add(1060658, "Charges\t{0}", m_Charges);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); 
                return;
            }

            if (m_Charges <= 0)
            {
                from.SendMessage("The staff is out of charges.");
                return;
            }

            Corpse foundCorpse = null;

            // Search map for the most recent corpse owned by the player
            foreach (Item item in World.Items.Values)
            {
                if (item is Corpse)
                {
                    Corpse c = (Corpse)item;
                    if (c.Owner == from && c.Map == from.Map && !c.Deleted)
                    {
                        foundCorpse = c;
                        break; 
                    }
                }
            }

            if (foundCorpse != null)
            {
                // NEW: Logic to grab equipped items (armor/clothing) as well as bag items
                List<Item> itemsToMove = new List<Item>(foundCorpse.Items);
                
                // Also check for items "worn" by the corpse that aren't in the main list
                foreach (Item equipped in foundCorpse.EquipItems)
                {
                    if (equipped != null && !equipped.Deleted)
                        itemsToMove.Add(equipped);
                }

                if (itemsToMove.Count > 0)
                {
                    foreach (Item item in itemsToMove)
                    {
                        from.AddToBackpack(item);
                    }

                    from.SendMessage("The staff glows brightly, pulling all your gear through the ether!");
                    from.PlaySound(0x1F2); // Play a 'summoning' sound effect
                    from.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot); // Add a visual sparkle

                    m_Charges--;
                    if (m_Charges <= 0)
                    {
                        from.SendMessage("The staff crumbles.");
                        this.Delete();
                    }
                }
                else
                {
                    from.SendMessage("Your corpse was found, but it appears to have already been looted.");
                }
            }
            else
            {
                from.SendMessage("No corpse belonging to you was found on this facet.");
            }
        }

        public static void TryRemoveTimer(Mobile m) { }

        public UndertakersStaff(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); 
            writer.Write((int)m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Charges = reader.ReadInt();
        }
    }
}