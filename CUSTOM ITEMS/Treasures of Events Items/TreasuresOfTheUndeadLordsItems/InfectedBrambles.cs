using System;

namespace Server.Items
{
    public class InfectedBrambles : Item
    {
        public override bool IsArtifact => true;

        [Constructable]
        public InfectedBrambles()
            : base(0x0D3F)
        {
            Hue = 2744; //Not correct, need hue!
			Name = "Infected Brambles";
        }

        public InfectedBrambles(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}