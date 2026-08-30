using System;

namespace Server.Items
{
    public class UnfinishedTableLegs : Item
    {
		public override bool IsArtifact => true;

        [Constructable]
        public UnfinishedTableLegs()
            : base(0x1E75)
        {
            Name = "Unfinished Table Legs";
            Weight = 1.0;
        }

        public UnfinishedTableLegs(Serial serial)
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