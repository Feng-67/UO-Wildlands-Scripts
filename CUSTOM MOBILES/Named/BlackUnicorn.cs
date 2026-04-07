/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an black unicorn corpse")]
    public class BlackUnicorn : BaseMount
    {
        [Constructable]
        public BlackUnicorn()
            : this("Black Unicorn")
        {
        }

        [Constructable]
        public BlackUnicorn(string name)
            : base(name, 0x7A, 0x3EB4, AIType.AI_Necro, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4BC; // Unicorn sounds
            Hue = 1; // Pure black

            SetStr(1300, 1400);
            SetDex(220, 240);
            SetInt(500, 950);

            SetHits(956, 990);

            SetDamage(21, 28);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Poison, 75);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.SpiritSpeak, 30.1, 40.0);
            SetSkill(SkillName.Necromancy, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 99.1, 110.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 92.5);

            Fame = 15000;
            Karma = -50000;

            Tamable = true;
            ControlSlots = 3;
            ControlSlotsMax = 5;
            MinTameSkill = 108;

            this.SetMagicalAbility(MagicalAbility.Necromancy);
        }

        public BlackUnicorn(Serial serial)
            : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(BlackUnicorn),
                    Class.MagicalAndNecromantic,
                    MagicalAbility.Necromage | MagicalAbility.Poisoning | MagicalAbility.Discordance | MagicalAbility.MageryMastery | MagicalAbility.Bashing | MagicalAbility.Piercing | MagicalAbility.Slashing | MagicalAbility.WrestlingMastery,
                    PetTrainingHelper.SpecialAbilityNecroMagical,
                    PetTrainingHelper.WepAbility11,
                    PetTrainingHelper.AreaEffectArea2,
                    3, 5);
            }
        }

        // Immune to all poisons except Lethal
        public override Poison PoisonImmune { get { return Poison.Deadly; } }

        public override bool CanAngerOnTame { get { return true; } }
        public override bool StatLossAfterTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
                                
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
