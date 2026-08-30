using System;
using Server.SkillHandlers;

namespace Server.Items
{
    public class GargishKiltOfTheHolyWarrior : GargishPlateKilt
    {
        public override int LabelNumber => 1161993; //Gargish Kilt of the Holy Warrior
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override int BasePhysicalResistance => 15;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 15;
        public override int BasePoisonResistance => 15;
        public override int BaseEnergyResistance => 15;

        private SkillMod m_NecroMod;

        [Constructable]
        public GargishKiltOfTheHolyWarrior()
            : base()
        {
            Hue = 2764;
            SkillBonuses.Skill_1_Name = SkillName.Chivalry;
            SkillBonuses.Skill_1_Value = 15;
            //SkillBonuses.Skill_2_Name = SkillName.Necromancy;
            //SkillBonuses.Skill_2_Value = -30;
            Attributes.BonusStr = 5;
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 5;
            Attributes.BonusStam = 10;
            Attributes.BonusMana = 15;
            Attributes.LowerManaCost = 8;
        }

        public GargishKiltOfTheHolyWarrior(Serial serial)
            : base(serial)
        {
        }

        public override bool OnEquip(Mobile from)
        {
            if (m_NecroMod != null) //Safety check
                m_NecroMod.Remove();

            m_NecroMod = new DefaultSkillMod(SkillName.Necromancy, true, -30);
			from.AddSkillMod(m_NecroMod);

            return base.OnEquip(from);
        }

        public override void OnRemoved( object parent )
		{
            if(m_NecroMod != null)
            {
                m_NecroMod.Remove();
                m_NecroMod = null;
            }
            base.OnRemoved(parent);
            return;
        }

        public override void AddWeightProperty(ObjectPropertyList list)
        {
            base.AddWeightProperty(list);
            list.Add("Reactive Holy Light 10%");
            list.Add("Necromancy -30");
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            if(version < 1)
            {
                SkillBonuses.Skill_2_Name = SkillName.Alchemy;
                SkillBonuses.Skill_2_Value = 0;
            }

            if (Parent is Mobile)
            {
                m_NecroMod = new DefaultSkillMod(SkillName.Necromancy, true, -30);
			    ((Mobile)Parent).AddSkillMod(m_NecroMod);
            }
        }
    }
}