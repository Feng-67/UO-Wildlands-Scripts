using System;

namespace Server.Items
{
	public class TabardOfTheFallenPaladin : BaseOuterTorso
	{
		public override string DefaultName => "Tabard of the Fallen Paladin";
		public override bool CanBeWornByGargoyles => true;
		public override double DefaultWeight => 3.0;

		[Constructable]
		public TabardOfTheFallenPaladin()
			: base(0xA412)
		{
			Hue = 2702;
		}

		public TabardOfTheFallenPaladin(Serial serial)
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
