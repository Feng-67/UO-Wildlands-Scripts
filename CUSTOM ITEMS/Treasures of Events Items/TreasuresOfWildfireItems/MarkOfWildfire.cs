using System;

namespace Server.Items
{
    public class MarkOfWildfire : SavageMask
	{
		public override bool IsArtifact => true;
        [Constructable]
        public MarkOfWildfire()
            : base()
        {
            Name = "Mark Of Wildfire";
            Hue = 2758;		
            Attributes.BonusMana = 10;
            Attributes.Luck = 250;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 25;			
        }

        public MarkOfWildfire(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;
		public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

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

    public class GargishMarkOfWildfire : GargishGlasses
	{
		public override bool IsArtifact => true;
        [Constructable]
        public GargishMarkOfWildfire()
            : base()
        {
            Name = "Mark Of Wildfire";
            Hue = 2758;		
            Attributes.BonusMana = 10;
            Attributes.Luck = 250;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 25;			
        }

        public GargishMarkOfWildfire(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;
		public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        
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