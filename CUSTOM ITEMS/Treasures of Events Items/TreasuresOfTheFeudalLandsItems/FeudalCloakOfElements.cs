using System;

namespace Server.Items
{
    public class FeudalCloakOfElements : FurCape
    {
        public override int LabelNumber => 1161974; //Feudal Cloak of Elements
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool IsArtifact => true;

        [Constructable]
        public FeudalCloakOfElements()
            : base()
        {
            Hue = 2764;
            Weight = 4.0;
            Attributes.RegenHits = 2;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 2;
            Attributes.Luck = 150;

             //Random eater/resistance
            switch(Utility.Random(5))
            {
                case 0: //Physical
                    SAAbsorptionAttributes.EaterKinetic = 15;
                    Resistances.Physical = 15;
                    break;
                case 1: //Fire
                    SAAbsorptionAttributes.EaterFire = 15;
                    Resistances.Fire = 15;
                    break;
                case 2: //Cold
                    SAAbsorptionAttributes.EaterCold = 15;
                    Resistances.Cold = 15;
                    break;
                case 3: //Poison
                    SAAbsorptionAttributes.EaterPoison = 15;
                    Resistances.Poison = 15;
                    break;
                case 4: //Energy
                    SAAbsorptionAttributes.EaterEnergy = 15;
                    Resistances.Energy = 15;
                    break;
                default:
                    break;
            }
        }

        public FeudalCloakOfElements(Serial serial)
            : base(serial)
        {
        }
 
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