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

            if (foundCorpse == null)
            {
                from.SendMessage("No corpse belonging to you was found on this facet.");
                return;
            }

            // Destroy all gold sitting on the corpse before it changes hands,
            // including gold tucked inside any bags/containers on the corpse
            foreach (Item item in new List<Item>(foundCorpse.Items))
            {
                DeleteGoldRecursively(item);
            }

            if (foundCorpse.Items.Count == 0)
            {
                from.SendMessage("Your corpse was found, but it appears to have already been looted.");
                return;
            }

            // Bring the actual corpse to the player rather than copying its
            // contents into the backpack. Corpse.Open() is the same self-loot
            // routine the client triggers when a player double clicks their
            // own corpse - it re-equips worn items to the paperdoll and
            // restores backpack items to their original positions, instead of
            // dumping everything loose into the top of the backpack.
            foundCorpse.MoveToWorld(from.Location, from.Map);
            foundCorpse.Open(from, true);

            from.SendMessage("The staff glows brightly, pulling your corpse through the ether!");
            from.SendMessage("The gold you carried was destroyed!");
            from.PlaySound(0x1F2); // Play a 'summoning' sound effect
            from.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot); // Add a visual sparkle

            m_Charges--;
            if (m_Charges <= 0)
            {
                from.SendMessage("The staff crumbles.");
                this.Delete();
            }
        }

        public static void TryRemoveTimer(Mobile m) { }

        // Recursively deletes any Gold found on an item - itself, or nested
        // inside any container (e.g. a bag left on the corpse). Non-gold
        // containers are left intact; only the gold inside them is removed.
        private static void DeleteGoldRecursively(Item item)
        {
            if (item == null || item.Deleted)
                return;

            if (item is Gold)
            {
                item.Delete();
                return;
            }

            if (item is Container)
            {
                foreach (Item child in new List<Item>(((Container)item).Items))
                {
                    DeleteGoldRecursively(child);
                }
            }
        }

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
