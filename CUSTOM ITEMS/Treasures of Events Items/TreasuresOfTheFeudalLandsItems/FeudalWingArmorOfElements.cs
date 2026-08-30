using System;

namespace Server.Items
{
    public class FeudalWingArmorOfElements : GargishLeatherWingArmor
    {
        public override int LabelNumber => 1161975; //Feudal Wing Armor of Elements
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        public override int PhysicalResistance { get { return AbsorptionAttributes.EaterKinetic > 0 ? 15 : 0; } }
        public override int FireResistance { get { return AbsorptionAttributes.EaterFire > 0 ? 15 : 0; } }
        public override int ColdResistance { get { return AbsorptionAttributes.EaterCold > 0 ? 15 : 0; } }
        public override int PoisonResistance { get { return AbsorptionAttributes.EaterPoison > 0 ? 15 : 0; } }
        public override int EnergyResistance { get { return AbsorptionAttributes.EaterEnergy > 0 ? 15 : 0; } }

        [Constructable]
        public FeudalWingArmorOfElements()
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
                    AbsorptionAttributes.EaterKinetic = 15;
                    break;
                case 1: //Fire
                    AbsorptionAttributes.EaterFire = 15;
                    break;
                case 2: //Cold
                    AbsorptionAttributes.EaterCold = 15;
                    break;
                case 3: //Poison
                    AbsorptionAttributes.EaterPoison = 15;
                    break;
                case 4: //Energy
                    AbsorptionAttributes.EaterEnergy = 15;
                    break;
                default:
                    break;
            }
        }

        public FeudalWingArmorOfElements(Serial serial)
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