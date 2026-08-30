using System;

namespace Server.Items
{
    public class JadeLanternTall : Item
    {
        public override int LabelNumber => 1161989; //Jade Lantern
		public override bool IsArtifact => true;

        [Constructable]
        public JadeLanternTall()
            : base(0x24BF)
        {
            Weight = 1.0;
            Hue = 2964;
        }

        public JadeLanternTall(Serial serial)
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

    public class JadeLanternShort : Item
    {
        public override int LabelNumber => 1161989; //Jade Lantern
		public override bool IsArtifact => true;

        [Constructable]
        public JadeLanternShort()
            : base(0x2419)
        {
            Weight = 1.0;
            Hue = 2964;
        }

        public JadeLanternShort(Serial serial)
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