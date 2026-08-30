using System;

namespace Server.Items
{
    public class ShojiScreenFeudal : Item
    {
        public override int LabelNumber => 1161991; //Shoji Screen

        [Constructable]
        public ShojiScreenFeudal()
            : base(0x1945)
        {
            Hue = 2764;
            Weight = 6.0;
        }

        public ShojiScreenFeudal(Serial serial)
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
