using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefTailoring), typeof(GargishMushroomCultivatorsApron))]
    public class MushroomCultivatorsApron : HalfApron
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public MushroomCultivatorsApron()
            : base()
        {
            Name = "Mushroom Cultivator's Apron";
            Hue = 2759;		
            Attributes.BonusHits = 5;
            Attributes.RegenHits = 2;
            Attributes.EnhancePotions = 15;
            SkillBonuses.Skill_1_Name = SkillName.Alchemy;
            SkillBonuses.Skill_1_Value = 10;
        }

        public MushroomCultivatorsApron(Serial serial)
            : base(serial)
        {
        }

        //public override int LabelNumber => ??;

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

    public class GargishMushroomCultivatorsApron : GargoyleHalfApron
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public GargishMushroomCultivatorsApron()
            : base()
        {
            Name = "Mushroom Cultivator's Apron";
            Hue = 2759;		
            Attributes.BonusHits = 5;
            Attributes.RegenHits = 2;
            Attributes.EnhancePotions = 15;
            SkillBonuses.Skill_1_Name = SkillName.Alchemy;
            SkillBonuses.Skill_1_Value = 10;
        }

        public GargishMushroomCultivatorsApron(Serial serial)
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