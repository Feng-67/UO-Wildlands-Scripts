using System;

namespace Server.Items
{
    public class ShugenjasRaiment : PlateDo
    {
        public override int LabelNumber => 1161973; //Shugenja's Raiment
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;

        [Constructable]
        public ShugenjasRaiment()
            : base()
        {
            Hue = 2764;
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 5;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 20;
            ArmorAttributes.MageArmor = 1;
        }

        public ShugenjasRaiment(Serial serial)
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

    public class ShugenjasRaimentStudded : StuddedDo
    {
        public override int LabelNumber => 1161973; //Shugenja's Raiment
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;

        [Constructable]
        public ShugenjasRaimentStudded()
            : base()
        {
            Hue = 2764;
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 5;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 20;
            ArmorAttributes.MageArmor = 1;
        }

        public ShugenjasRaimentStudded(Serial serial)
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

    public class ShugenjasRaimentGargish : GargishStoneChest
    {
        public override int LabelNumber => 1161973; //Shugenja's Raiment
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;

        [Constructable]
        public ShugenjasRaimentGargish()
            : base()
        {
            Hue = 2764;
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 5;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 20;
            ArmorAttributes.MageArmor = 1;
        }

        public ShugenjasRaimentGargish(Serial serial)
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