using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
	public class IntricatelyCraftedBirdHouse : Item
	{
        [Constructable]
		public IntricatelyCraftedBirdHouse() : base(0xA607)
		{
			Name = "An Intricately Crafted Bird House";
            Weight = 1.0;
        }
       
        public IntricatelyCraftedBirdHouse(Serial serial) : base(serial)
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
            list.Add("Salvaged From The Remains Of A Dessicated Treefellow");
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
