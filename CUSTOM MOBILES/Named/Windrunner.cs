/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;

namespace Server.Mobiles
{
    [CorpseName("a windrunner corpse")]
    public class Windrunner : BaseMount
    {
        [Constructable]
        public Windrunner()
            : this("Windrunner")
        {
        }

        [Constructable]
        public Windrunner(string name)
            : base(name, 1410, 16076, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xA8;

            SetStr(500, 555);
            SetDex(85, 125);
            SetInt(50, 60);

            SetHits(625, 700);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 25, 45);

            SetSkill(SkillName.MagicResist, 125.0, 150.0);
            SetSkill(SkillName.Tactics, 80.0, 95.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Poisoning, 90.0, 100.0);
            SetSkill(SkillName.DetectHidden, 45.0, 55.0);

            Fame = 15000;
            Karma = -15000;

            SetMagicalAbility(MagicalAbility.Poisoning);
            
            Tamable = true;
            ControlSlots = 3;
            ControlSlotsMax = 5;
            MinTameSkill = 108;
        }

        public Windrunner(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
                
        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(Windrunner),
                    Class.None,
                    MagicalAbility.Ninjitsu |
                    MagicalAbility.Bashing | MagicalAbility.Piercing |
                    MagicalAbility.Slashing | MagicalAbility.WrestlingMastery,
                    new SpecialAbility[] {
                        SpecialAbility.VenomousBite, SpecialAbility.ViciousBite,
                        SpecialAbility.ManaDrain, SpecialAbility.Repel,
                        SpecialAbility.SearingWounds, SpecialAbility.LifeLeech
                    },
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea3,
                    3, 5);
            }
        }

        public override int GetIdleSound()
        {
            return 0x577;
        }

        public override int GetAttackSound()
        {
            return 0x576;
        }

        public override int GetAngerSound()
        {
            return 0x578;
        }

        public override int GetHurtSound()
        {
            return 0x576;
        }

        public override int GetDeathSound()
        {
            return 0x579;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version bump 0 -> 1
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
