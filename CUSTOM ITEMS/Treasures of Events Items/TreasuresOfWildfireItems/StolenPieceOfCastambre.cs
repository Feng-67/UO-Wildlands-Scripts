using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class StolenPieceOfCastambre : Item
    {
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; }
        }

        [Constructable]
		public StolenPieceOfCastambre() : base( 0x1363 )
		{
            Name = "Stolen Piece of Castambre";
			Weight = 1.0;
			Hue = 2671; //TODO: Verify hue!
            UsesRemaining = 10;
		}

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);            
            list.Add("Find My Heart...");
        }

        public override void OnDoubleClick(Mobile m)
		{
            if (UsesRemaining <= 0)
            {
                m.SendLocalizedMessage(501228); //I can do no more for you at this time.
                return;
            }
            HeartOfCastambre heart = new HeartOfCastambre();
			if (heart != null)
            {
                if (m.Backpack == null || !m.Backpack.TryDropItem(m, heart, true))
                    heart.MoveToWorld(m.Location, m.Map);

                UsesRemaining--;
            }
		}

        public StolenPieceOfCastambre( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
            writer.Write(m_UsesRemaining);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
            m_UsesRemaining = reader.ReadInt();
		}

    }

    public class HeartOfCastambre : Item
    {
        [Constructable]
        public HeartOfCastambre()
            : base(0x24B)
        {
        }

        public HeartOfCastambre(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1027405; // heart
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