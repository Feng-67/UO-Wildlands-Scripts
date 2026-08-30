using System;

namespace Server.Items
{
    [Flipable(0xA600, 0xA601)]
    public class SnowyHedgeShort : Item
    {
        public override int LabelNumber => 1126520; 
		public override bool IsArtifact => true;

        [Constructable]
        public SnowyHedgeShort()
            : base(0xA600)
        {
            Weight = 1.0;
        }

        public SnowyHedgeShort(Serial serial)
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

    [Flipable(0xA602, 0xA603)]
    public class SnowyHedgeTall : Item
    {
        public override int LabelNumber => 1126520; 
		public override bool IsArtifact => true;

        [Constructable]
        public SnowyHedgeTall()
            : base(0xA602)
        {
            Weight = 1.0;
        }

        public SnowyHedgeTall(Serial serial)
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