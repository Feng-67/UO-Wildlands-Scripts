/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a kuraku dread corpse")]
    public class KurakuDread : BaseCreature
    {
        [Constructable]
        public KurakuDread()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a kuraku dread";
            Body = 1400;
            Hue = Utility.RandomList(1143, 2012, 1437, 2419);
            BaseSoundID = 362;
          
            // --- Taming ---
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 108.0;

            // --- Attributes ---
            SetStr(700, 825);
            SetDex(190, 220);
            SetInt(50, 100);

            SetHits(500, 700);

            // --- Damage Profile ---
            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 100);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 22000;
            Karma = -22000;

            // --- Abilities ---
            SetWeaponAbility(WeaponAbility.ArmorIgnore);
            SetSpecialAbility(SpecialAbility.TailSwipe);
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(KurakuDread),
                    Class.ClawedAndTailed,
                    MagicalAbility.StandardClawedOrTailed,
                    PetTrainingHelper.SpecialAbilityClawedAndTailed,
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea1,
                    3, 5);
            }
        }

        public override bool AutoDispel { get { return !Controlled; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override bool CanFlee { get { return false; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public KurakuDread(Serial serial) : base(serial) { }

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
