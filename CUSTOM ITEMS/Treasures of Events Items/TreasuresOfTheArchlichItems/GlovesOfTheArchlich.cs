using System;

namespace Server.Items
{
    public class GlovesOfTheArchlich : BoneGloves
    {
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public GlovesOfTheArchlich()
            : base()
        {
            Name = "Gloves of the Archlich";
            Hue = 2702;
            AbsorptionAttributes.EaterFire = 15;
            Attributes.BonusStr = 5;
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 5;
            Attributes.BonusMana = 8;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 20;
            ArmorAttributes.MageArmor = 1;
        }

        public GlovesOfTheArchlich(Serial serial)
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
			
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			
            int version = reader.ReadInt();
            if (version < 1)
                ArmorAttributes.MageArmor = 1;
        }
    }
}
