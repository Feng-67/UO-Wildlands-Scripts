using System;

namespace Server.Items
{
    public class RolledInvasionMap : Item
    {
        public override bool IsArtifact => true;

        [Constructable]
        public RolledInvasionMap()
            : base(0x2831)
        {
            Hue = 2500; //Not verified!
			Name = "Rolled Invasion Map";
        }

        public RolledInvasionMap(Serial serial)
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