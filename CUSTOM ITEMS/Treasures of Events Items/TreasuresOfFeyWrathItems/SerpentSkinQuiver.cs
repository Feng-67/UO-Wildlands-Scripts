using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefTailoring), typeof(SerpentSkinWingArmor))]
    public class SerpentSkinQuiver : BaseQuiver
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public SerpentSkinQuiver() : base(0x2B02)
        {
			Name = "Serpent Skin Quiver";
            Hue = 2755;
            
            SkillBonuses.SetValues( 0, SkillName.Anatomy, 5.0 );
            DamageIncrease = 10;
            Attributes.Luck = 125;
            Attributes.WeaponSpeed = 5;
            Attributes.WeaponDamage = 10;
            LowerAmmoCost = 30;
            WeightReduction = 30;
        }

        public SerpentSkinQuiver(Serial serial)
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

    public class SerpentSkinWingArmor : GargishClothWingArmor
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public SerpentSkinWingArmor()
        {
            Name = "Serpent Skin Wing Armor";
            Hue = 2755;
            
            SkillBonuses.SetValues( 0, SkillName.Anatomy, 5.0 );
            Attributes.Luck = 125;
            Attributes.WeaponSpeed = 5;
            Attributes.WeaponDamage = 10;
        }

        public SerpentSkinWingArmor(Serial serial)
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