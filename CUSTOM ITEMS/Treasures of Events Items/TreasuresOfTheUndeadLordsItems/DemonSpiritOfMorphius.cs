using System;

namespace Server.Items
{
    public class DemonSpiritOfMorphius : Item
    {
        public override bool IsArtifact => true;

        [Constructable]
        public DemonSpiritOfMorphius()
            : base(0x469C)
        {
            Hue = 2730; //Verified!
			Name = "Demon Spirit Of Morphius";
        }

        public DemonSpiritOfMorphius(Serial serial)
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