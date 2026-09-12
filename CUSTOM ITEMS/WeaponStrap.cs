using System;
using Server;

namespace Server.Items
{
    public class WeaponStrap : Container
    {
        private int m_WeightReduction;

        public override bool IsArtifact { get { return true; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WeightReduction
        {
            get { return m_WeightReduction; }
            set { m_WeightReduction = value; InvalidateProperties(); }
        }

        public override int DefaultMaxItems { get { return 10; } }
        public override int DefaultGumpID { get { return 61; } }

        [Constructable]
        public WeaponStrap() : base(0x2B02)
        {
            Hue = 0x901;
            Layer = Layer.Cloak;
            Weight = 1.0;
            m_WeightReduction = 50;

            // Explicit name so it shows on the item
            Name = "Weapon Strap";
        }

        public WeaponStrap(Serial serial) : base(serial)
        {
        }

        public override int GetTotal(TotalType type)
        {
            int total = base.GetTotal(type);

            if (type == TotalType.Weight && m_WeightReduction > 0)
            {
                total = (total * (100 - m_WeightReduction)) / 100;
            }

            return total;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_WeightReduction > 0)
            {
                list.Add(1072210, m_WeightReduction.ToString());
            }
        }

        // Restrict what can be placed inside to melee weapons only
        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (!(item is BaseWeapon))
            {
                if (message)
                    m.SendMessage("You can only place melee weapons in a weapon strap.");

                return false;
            }

            if (item is BaseRanged)
            {
                if (message)
                    m.SendMessage("Ranged weapons do not belong in a weapon strap.");

                return false;
            }

            return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.Alive)
                return;

            if (Parent == from || !IsChildOf(from.Backpack))
            {
                base.OnDoubleClick(from);
            }
            else
            {
                from.EquipItem(this);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0);
            writer.Write((int)m_WeightReduction);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            m_WeightReduction = reader.ReadInt();
        }
    }
}
