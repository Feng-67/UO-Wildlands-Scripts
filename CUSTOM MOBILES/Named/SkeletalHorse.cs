/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team] (Original Author Rutibex)
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a skeletal horse corpse")]
    public class SkeletalHorse : BaseMount
    {
        [Constructable]
        public SkeletalHorse()
            : this("Skeletal Horse")
        {
        }

        [Constructable]
        public SkeletalHorse(string name)
            : base(name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // --- Taming ---
            Tamable = true;
            ControlSlots = 3;       // Upgraded: Spawns at 3 slots; trainable to 5
            MinTameSkill = 108.0;    // Upgraded: reflect 3-slot tier

            // --- Attributes ---
            SetStr(400, 500);       // Upgraded from 100
            SetDex(100, 125);       // Upgraded from 80
            SetInt(150, 250);       // Upgraded from 100 (Necromantic focus)

            SetHits(450, 600);      // Upgraded from 100
            SetStam(100, 125);
            SetMana(150, 250);

            // --- Damage Profile ---
            SetDamage(18, 24);      // Upgraded from 5-7

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.Wrestling, 90.1, 105.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 95.0);
            SetSkill(SkillName.Anatomy, 60.0, 80.0);

            // Innate: Necromancy (Enhanced)
            SetSkill(SkillName.Necromancy, 80.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);

            Fame = 12000;
            Karma = -12000;
        }

        // ------------------------------------------------------------------
        // Pet Training Profile
        // ------------------------------------------------------------------
        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(SkeletalHorse),
                    Class.MagicalAndNecromantic,
                    MagicalAbility.GrizzledMare,
                    PetTrainingHelper.SpecialAbilityGrizzledMare,
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea4,
                    3, 5); // Upgraded: Range set to 3 -> 5
            }
        }

        public SkeletalHorse(Serial serial)
            : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version < 2)
                reader.ReadInt();
        }
    }
}
