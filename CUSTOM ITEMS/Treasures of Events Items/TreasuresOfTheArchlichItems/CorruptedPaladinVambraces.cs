using System;

namespace Server.Items
{
    public class CorruptedPaladinVambraces : PlateArms
    {
		public override bool IsArtifact => true;

        [Constructable]
        public CorruptedPaladinVambraces()
            : base()
        {
            Name = "Corrupted Paladin Vambraces";
            Hue = 2702;

            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.BonusStam = 10;
            Attributes.BonusMana = 10;
            Attributes.RegenHits = 4;
            Attributes.RegenStam = 4;
            Attributes.RegenMana = 4;
            Attributes.LowerManaCost = 8;
        }

        public CorruptedPaladinVambraces(Serial serial)
            : base(serial)
        {
        }
        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GargishCorruptedPaladinVambraces : GargishPlateArms
    {
		public override bool IsArtifact => true;

        [Constructable]
        public GargishCorruptedPaladinVambraces()
            : base()
        {
            Name = "Corrupted Paladin Vambraces";
            Hue = 2702;

            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.BonusStam = 10;
            Attributes.BonusMana = 10;
            Attributes.RegenHits = 4;
            Attributes.RegenStam = 4;
            Attributes.RegenMana = 4;
            Attributes.LowerManaCost = 8;
        }

        public GargishCorruptedPaladinVambraces(Serial serial)
            : base(serial)
        {
        }
        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}