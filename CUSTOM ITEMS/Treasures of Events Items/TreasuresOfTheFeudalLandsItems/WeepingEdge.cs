using System;

namespace Server.Items
{
    [FlipableAttribute(0x27A4, 0x27EF)]
    public class WeepingEdge : Bokuto
    {
        public override int LabelNumber => 1162002; //Weeping Edge
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool IsArtifact => false;

        [Constructable]
        public WeepingEdge()
            : base()
        {
            // Use ItemQuality enum which exists in standard ServUO
            Quality = ItemQuality.Exceptional;
            Hue = 2964;
            WeaponAttributes.SplinteringWeapon = 30;
        }

        public WeepingEdge(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
