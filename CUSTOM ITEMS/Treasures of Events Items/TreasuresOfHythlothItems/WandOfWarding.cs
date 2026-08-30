using Server;

namespace Server.Items
{
    public class WandOfWarding : BaseShield
	{	
        public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public WandOfWarding()
			: base(0xF6B)
		{
			Name = "Wand of Warding";
			Weight = 8.0;
		}

		public WandOfWarding(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadEncodedInt();
		}
	}
}