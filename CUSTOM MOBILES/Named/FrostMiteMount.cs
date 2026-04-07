/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using Server.Items;
using System;

namespace Server.Mobiles
{
    [CorpseName("a frost mite corpse")]
    public class FrostMiteMount : BaseMount
    {
        [Constructable]
        public FrostMiteMount()
            : this("Frost Mite")
        {
        }

        [Constructable]
        public FrostMiteMount(string name) : base(name, 0x590, 0x3EDA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Frost Mite";
            Body = 0x590;

            SetStr(1017);
            SetDex(164);
            SetInt(283);

            SetHits(800, 1000);

            SetDamage(21, 28);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 65, 75);

            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);

            Fame = 15000;
            Karma = -15000;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 108.0;
        }

        public bool AuraActive { get { return true; } }
        public int AuraRange { get { return 3; } }
        public TimeSpan AuraInterval { get { return TimeSpan.FromSeconds(2.0); } }

        public void OnAuraEffect(Mobile m)
        {
            int damage = 10;
            m.Damage(damage, this);
            m.FixedParticles(0x376A, 1, 32, 0x1531, EffectLayer.Waist);
            m.PlaySound(0x5C6);
            m.SendLocalizedMessage(1008111, false, Name); // The intense cold is damaging you!
        }

        public FrostMiteMount(Serial serial) : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(FrostMiteMount),
                    Class.ClawedTailedNecromanticAndTokuno,
                    MagicalAbility.Poisoning | MagicalAbility.Piercing,
                    PetTrainingHelper.SpecialAbilityClawedAndNecromantic,
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea2,
                    3, 5);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version > 0)
            {
                reader.ReadBool(); // Consume the old isExotic bool if loading an old save
            }
        }
    }
}
