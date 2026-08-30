using Server;

namespace Server.Items
{    
    public class LanternOfProtection : BaseEquipableLight
    {
        // public override int InitMinHits{ get{ return 255; } }
		// public override int InitMaxHits{ get{ return 255; } }
        [Constructable]
        public LanternOfProtection()
            : base(0xA76A)
        {
            Name = "Lantern of Protection";
			Weight = 1.0;
        }

        public LanternOfProtection(Serial serial)
            : base(serial)
        {
        }

        public override int LitItemID
        {
            get
            {
                return 0xA769;
            }
        }
        public override int UnlitItemID
        {
            get
            {
                return 0xA76A;
            }
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