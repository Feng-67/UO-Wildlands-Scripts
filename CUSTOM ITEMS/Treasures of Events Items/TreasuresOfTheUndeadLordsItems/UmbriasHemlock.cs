using System;

namespace Server.Items
{
    public class UmbriasHemlock : Item
    {
        public override bool IsArtifact => true;

        [Constructable]
        public UmbriasHemlock()
            : base(0x0DEE)
        {
            Hue = 2075; //Confirmed on Atlantic
			Name = "Umbria's Hemlock";
        }

        public UmbriasHemlock(Serial serial)
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