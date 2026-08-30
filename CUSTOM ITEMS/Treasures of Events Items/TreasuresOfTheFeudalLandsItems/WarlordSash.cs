namespace Server.Items
{
    [FlipableAttribute( 0x1541, 0x1542 )] 
    public class WarlordSash : BodySash
    {
        public override int LabelNumber => 1161980; //Warlord Sash
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public WarlordSash() : base()
        {
            Hue = 2500;

            //Random Primary Stat
            switch(Utility.Random(3))
            {
                case 0:
                    Attributes.BonusStr = 4;
                    break;
                case 1:
                    Attributes.BonusDex = 4;
                    break;
                case 2:
                    Attributes.BonusInt = 4;
                    break;
                default: break;
            }

            //Raandom Eater
            switch(Utility.Random(5))
            {
                case 0: //Physical
                    SAAbsorptionAttributes.EaterKinetic = 10;
                    break;
                case 1: //Fire
                    SAAbsorptionAttributes.EaterFire = 10;
                    break;
                case 2: //Cold
                    SAAbsorptionAttributes.EaterCold = 10;
                    break;
                case 3: //Poison
                    SAAbsorptionAttributes.EaterPoison = 10;
                    break;
                case 4: //Energy
                    SAAbsorptionAttributes.EaterEnergy = 10;
                    break;
                default:
                    break;
            }		
        }

        public WarlordSash(Serial serial) : base(serial)
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

    public class GargishWarlordSash : GargishSash
    {
        public override int LabelNumber => 1161980; //Warlord Sash
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        [Constructable]
        public GargishWarlordSash() : base()
        {
            Hue = 2500;
            
            //Random Primary Stat
            switch(Utility.Random(3))
            {
                case 0:
                    Attributes.BonusStr = 4;
                    break;
                case 1:
                    Attributes.BonusDex = 4;
                    break;
                case 2:
                    Attributes.BonusInt = 4;
                    break;
                default: break;
            }

            //Raandom Eater
            switch(Utility.Random(5))
            {
                case 0: //Physical
                    SAAbsorptionAttributes.EaterKinetic = 10;
                    break;
                case 1: //Fire
                    SAAbsorptionAttributes.EaterFire = 10;
                    break;
                case 2: //Cold
                    SAAbsorptionAttributes.EaterCold = 10;
                    break;
                case 3: //Poison
                    SAAbsorptionAttributes.EaterPoison = 10;
                    break;
                case 4: //Energy
                    SAAbsorptionAttributes.EaterEnergy = 10;
                    break;
                default:
                    break;
            }				
        }

        public GargishWarlordSash(Serial serial) : base(serial)
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