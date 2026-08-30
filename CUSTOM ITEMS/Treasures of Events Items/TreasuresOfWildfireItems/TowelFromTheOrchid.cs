using System;
using Server.Gumps;

namespace Server.Items
{
    public class TowelFromTheOrchid : Item
    {
        [Constructable]
        public TowelFromTheOrchid()
            : base(0x1914)
        {
            Name = "Towel From The Orchid";
            Hue = 1195;
            Weight = 1.0;
        }

        public TowelFromTheOrchid(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile m)
		{
			string text1 = "This towel comes from the Orchid Bath House on Buccaneer's Den. It has a scent of flowers and the sea.";
			string text2 = "Monogram - \"Compliments of Ghorza and Dura\"";
			m.SendGump(new SimpleTextGump(m,this,text1,text2));
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
