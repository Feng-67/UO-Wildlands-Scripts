using System;

namespace Server.Items
{
    public class SentinelsNecklace : GargishNecklace
    {
        public override int LabelNumber => 1162017; //Sentinel's Necklace
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;
        [Constructable]
        public SentinelsNecklace()
            : base()
        {
            Hue = 2764;
            Attributes.BonusStr = 4;
            Attributes.BonusDex = 4;
            Attributes.BonusHits = 8;
            Attributes.BonusStam = 12;
            Attributes.BonusMana = 8;
            Attributes.AttackChance = 5;
            Attributes.DefendChance = 5;
            Attributes.LowerManaCost = 8;
        }

        public SentinelsNecklace(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}