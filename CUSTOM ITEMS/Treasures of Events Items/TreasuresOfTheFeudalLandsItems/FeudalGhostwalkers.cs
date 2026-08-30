using System;

namespace Server.Items
{
    public class FeudalGhostWalkers : NinjaTabi
    {
        public override int LabelNumber => 1161978; //Feudal Ghostwalkers
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool IsArtifact => true;

        [Constructable]
        public FeudalGhostWalkers()
            : base()
        {
            Hue = 2764;
            Weight = 2.0;
            Attributes.NightSight = 1;
            
            SkillBonuses.Skill_1_Name = SkillName.Stealth;
            SkillBonuses.Skill_1_Value = 5;
			SkillBonuses.Skill_2_Name = SkillName.Hiding;
            SkillBonuses.Skill_2_Value = 5;
        }

        public FeudalGhostWalkers(Serial serial)
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

    [FlipableAttribute(0xA296, 0xA294)]
    public class FeudalGhostWalkersGargish : BaseShoes
    {
        public override int LabelNumber => 1161978; //Feudal Ghostwalkers
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool IsArtifact => true;

        [Constructable]
        public FeudalGhostWalkersGargish()
            : base(0xA296) //0xA294
        {
            Hue = 2764;
            Weight = 2.0;
            Attributes.NightSight = 1;
            
            SkillBonuses.Skill_1_Name = SkillName.Stealth;
            SkillBonuses.Skill_1_Value = 5;
			SkillBonuses.Skill_2_Name = SkillName.Hiding;
            SkillBonuses.Skill_2_Value = 5;
        }

        public FeudalGhostWalkersGargish(Serial serial)
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