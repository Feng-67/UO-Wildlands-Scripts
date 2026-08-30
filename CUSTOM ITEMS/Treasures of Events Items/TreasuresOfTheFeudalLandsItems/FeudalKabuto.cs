using System;

namespace Server.Items
{
    [FlipableAttribute(0x236C, 0x236D)]
    public class FeudalKabuto : BaseArmor
    {
        public override int LabelNumber => 1161984; //Feudal Kabuto
        public override int BasePhysicalResistance => 5;
        public override int BaseFireResistance => 3;
        public override int BaseColdResistance => 3;
        public override int BasePoisonResistance => 2;
        public override int BaseEnergyResistance => 2;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Plate;

        [Constructable]
        public FeudalKabuto()
            : base(0x236C)
        {
        }

        public FeudalKabuto(Serial serial)
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