using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
	[Flipable( 0x9EE5, 0x9EE6 )]
	public class BloodSpawnRitualTable : Item
	{

        [Constructable]
		public BloodSpawnRitualTable() : base(0x9EE5)
		{
            Name = "Blood Spawn Ritual Table";
            Weight = 1.0;
            Hue = 2758;
        }

        public override void OnDoubleClick(Mobile m)
		{
			string text1 = "A table made of Blood Spawn used by the Cult of Wildfire in their wicked blood magic.";
			string text2 = "<center><basefont color=#FF0000>\"Crios Grav Flam\"</center>";
			m.SendGump(new SimpleTextGump(m,this,text1,text2));
		}

       
        public BloodSpawnRitualTable(Serial serial) : base(serial)
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
