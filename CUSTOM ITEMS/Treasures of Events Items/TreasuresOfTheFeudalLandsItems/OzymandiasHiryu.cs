using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a hiryu corpse")]
    public class OzymandiasHiryu : Hiryu
    {
        [Constructable]
        public OzymandiasHiryu()
            : base()
        {
            Name = "Ozymandias' Hiryu";
            Hue = 2500;

            SetStr(201, 300);
            SetDex(66, 85);
            SetInt(61, 100);

            SetHits(121, 180);

            SetDamage(3, 4);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);
        }

        public OzymandiasHiryu(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteOnRelease => true;
        public override double GetControlChance(Mobile m, bool useBaseSkill) => 1.0;

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1115719, "60000"); // armor points: ~1_val~
            list.Add(1049646); // (summoned)
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if(version < 1)
            {
                SetStr(201, 300);
                SetDex(66, 85);
                SetInt(61, 100);

                SetHits(121, 180);

                SetDamage(3, 4);

                SetResistance(ResistanceType.Physical, 35, 40);
                SetResistance(ResistanceType.Fire, 20, 30);
                SetResistance(ResistanceType.Cold, 20, 40);
                SetResistance(ResistanceType.Poison, 20, 30);
                SetResistance(ResistanceType.Energy, 30, 40);

                SetSkill(SkillName.Anatomy, 45.1, 55.0);
                SetSkill(SkillName.MagicResist, 45.1, 55.0);
                SetSkill(SkillName.Tactics, 45.1, 55.0);
                SetSkill(SkillName.Wrestling, 45.1, 55.0);
            }
        }

        public override void OnRiderDamaged(Mobile from, ref int amount, bool willKill)
        {
            base.OnRiderDamaged(from, ref amount, willKill);

            if (Rider == null)
                return;

            if ((from == null || !from.Player) && Rider.Player && Rider.Mount == this)
            {
                int percent = 12;
                int absorbed = AOS.Scale(amount, percent);
                amount -= absorbed;
            }
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                Type type = typeof(OzymandiasHiryu);
                Class classificaion = Class.Untrainable;
                MagicalAbility magicalAbility = MagicalAbility.None;
                SpecialAbility[] specialAbility = PetTrainingHelper.SpecialAbilityNone;
                WeaponAbility[] weaponAbility = PetTrainingHelper.WepAbilityNone;
                AreaEffect[] areaEffect = PetTrainingHelper.AreaEffectNone;
                int controlmin = 1;
                int controlmax = 1;
                return new TrainingDefinition(type,classificaion,magicalAbility,specialAbility,weaponAbility,areaEffect,controlmin,controlmax);
            }            
        }
    }
}

namespace Server.Items
{
    public class OzymandiasHiryuStatuette : BaseImprisonedMobile
    {
        [Constructable]
        public OzymandiasHiryuStatuette()
            : base(0x276A)
        {
            this.Weight = 1.0;
            this.Hue = 2500;
        }

        public OzymandiasHiryuStatuette(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1161983; //Ozymandias' Hiryu
        public override BaseCreature Summon
        {
            get
            {
                return new OzymandiasHiryu();
            }
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