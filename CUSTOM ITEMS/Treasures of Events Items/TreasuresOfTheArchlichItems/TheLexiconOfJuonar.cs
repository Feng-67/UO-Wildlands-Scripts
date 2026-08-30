using System;

namespace Server.Items
{
	public class TheLexiconOfJuonar : BookOfChivalry
	{
		[Constructable]
		public TheLexiconOfJuonar()
			: base()
		{
            Name = "The Lexicon Of Juo'nar";
			Hue = 2702;

            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 50;
            Attributes.CastSpeed = 1;
            Attributes.LowerRegCost = 10;
		}

		public TheLexiconOfJuonar(Serial serial)
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
