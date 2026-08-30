using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
	public class DecorativePlinth : Item
	{
		public override int LabelNumber => 1159908; //Decorative Plinth

        [Constructable]
		public DecorativePlinth() : base(0x1F2A)
		{
            Weight = 1.0;
        }
       
        public DecorativePlinth(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
	}
}
