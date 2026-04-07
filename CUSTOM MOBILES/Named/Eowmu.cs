/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;

namespace Server.Mobiles
{
    [CorpseName("an eowmu corpse")]
    public class Eowmu : BaseMount
    {
        [Constructable]
        public Eowmu()
            : this("Eowmu")
        {
        }

        [Constructable]
        public Eowmu(string name)
            : base(name, 1440, 16079, AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x270;
            
            SetStr(504, 700);
            SetDex(202, 300);
            SetInt(504, 700);

            SetHits(340, 383);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 10, 12);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.2, 100.0);
            SetSkill(SkillName.Magery, 90.2, 100.0);
            SetSkill(SkillName.Meditation, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 86.0, 135.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.DetectHidden, 70.0, 80.0);

            Fame = 15000;
            Karma = 15000;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 108;

            SetMagicalAbility(MagicalAbility.Magery);
            SetSpecialAbility(SpecialAbility.TailSwipe);
            SetAreaEffect(AreaEffect.AuraOfEnergy);
        }

        public Eowmu(Serial serial)
            : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }
                
        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(Eowmu),
                    Class.MagicalAndTailed,
                    MagicalAbility.MageryMastery | MagicalAbility.Mysticism | MagicalAbility.Spellweaving |
                    MagicalAbility.Chivalry | MagicalAbility.Discordance | MagicalAbility.Poisoning |
                    MagicalAbility.Bashing | MagicalAbility.Piercing | MagicalAbility.Slashing |
                    MagicalAbility.WrestlingMastery,
                    PetTrainingHelper.SpecialAbilityMagical2,
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea2,
                    3, 5);
            }
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
