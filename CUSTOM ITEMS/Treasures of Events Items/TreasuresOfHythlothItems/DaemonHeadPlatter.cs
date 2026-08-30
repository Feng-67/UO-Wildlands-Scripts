using System;
using Server;

namespace Server.Items
{
    [Flipable(0xA7A7, 0xA7A8)]
    public class DaemonHeadPlatter : Item, IUsesRemaining
    {
        public override int LabelNumber { get { return 1126943; } } // daemon head platter
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        public bool ShowUsesRemaining { get { return true; } set { } }

        private DateTime _NextRecharge;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextRecharge
        {
            get { return _NextRecharge; }
            set { _NextRecharge = value; }
        }

        [Constructable]
        public DaemonHeadPlatter()
            : base(0xA7A7)
        {
            Stackable = false;
            this.Weight = 1.0;
            UsesRemaining = 5;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_UsesRemaining > 0)
            {
                Item item = new DemonCheekSandwich();

                if (item != null)
                {
                    if (from.Backpack == null || !from.Backpack.TryDropItem(from, item, false))
                        item.MoveToWorld(from.Location, from.Map);

                    UsesRemaining--;
                }
            }
            else
            {
                from.SendLocalizedMessage(501789); //You must wait before trying again.
            }
        }

        public override void AddUsesRemainingProperties(ObjectPropertyList list)
        {
            if(ShowUsesRemaining)
            {
                string label = "Sandwiches";
                if(m_UsesRemaining==1)
                    label = "Sandwich";
                    
                list.Add($"{m_UsesRemaining} {label}"); // X Sandwich(es)
            }
        }

        private void CheckRecharge()
        {
            if (DateTime.UtcNow.Month == 11 && UsesRemaining < 10 && _NextRecharge < DateTime.UtcNow)
            {
                UsesRemaining++;
                _NextRecharge = DateTime.UtcNow + TimeSpan.FromDays(1);
            }
        }

        public DaemonHeadPlatter(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(_NextRecharge);
            writer.Write(m_UsesRemaining);

            CheckRecharge();
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            _NextRecharge = reader.ReadDateTime();
            m_UsesRemaining = reader.ReadInt();
        }
    }

    public class DemonCheekSandwich : Food
    {
        public override int LabelNumber { get { return 1159797; } } // demon cheek sandwich

        [Constructable]
        public DemonCheekSandwich()
            : base(0xA0DA)
        {
            this.Weight = 1.0;
            this.FillFactor = 1;
        }

        public DemonCheekSandwich(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
