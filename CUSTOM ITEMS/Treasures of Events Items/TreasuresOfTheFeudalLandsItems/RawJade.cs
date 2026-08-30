using System;

namespace Server.Items
{
    public class RawJade : Item
    {
        public override int LabelNumber => 1161990; //Raw Jade
		public override bool IsArtifact => true;

        [Constructable]
        public RawJade()
            : base(0xA1D8)
        {
            Weight = 3.0;
            Hue = 2964;
        }

        public RawJade(Serial serial)
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