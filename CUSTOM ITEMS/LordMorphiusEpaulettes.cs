using Server;
using System;

namespace Server.Items
{
    public class LordMorphiusEpaulettes : Cloak
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public LordMorphiusEpaulettes()
        {
            Name = "Lord Morphius' Epaulettes";
            ItemID = 0x9985;
            Weight = 1.0;
            Layer = Layer.OuterTorso;

            StrRequirement = 10;

            Attributes.BonusStam = 8;
            Attributes.RegenStam = 2;
            Attributes.WeaponSpeed = 10;
            Attributes.LowerManaCost = 3;
        }

        public LordMorphiusEpaulettes(Serial serial)
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
