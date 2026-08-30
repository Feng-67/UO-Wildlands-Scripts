using System;

namespace Server.Items
{
    [FlipableAttribute(0x27A4, 0x27EF)]
    public class StormLordsSteel : Wakizashi
    {
        public override int LabelNumber => 1161976; //Storm Lord's Steel
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool IsArtifact => false;

        [Constructable]
        public StormLordsSteel()
            : base()
        {
            // Use ItemQuality enum which exists in standard ServUO
            Quality = ItemQuality.Exceptional;
            Hue = 2764;
            WeaponAttributes.HitLightning = 80;
            ExtendedWeaponAttributes.HitSparks = 30;
            NegativeAttributes.Prized = 1;
        }

        public StormLordsSteel(Serial serial)
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
