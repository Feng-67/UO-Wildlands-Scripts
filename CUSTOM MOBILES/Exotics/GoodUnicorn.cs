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
    [CorpseName("a good unicorn corpse")]
    public class GoodUnicorn : BaseMount
    {
        [Constructable]
        public GoodUnicorn()
            : this("Good Unicorn")
        {
        }

        [Constructable]
        public GoodUnicorn(string name)
            : base(name, 1407, 16075, AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4BC; // Unicorn sounds

            SetStr(650, 700);
            SetDex(110, 120);
            SetInt(250, 475);

            SetHits(478, 495);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.EvalInt, 30.1, 40.0);
            SetSkill(SkillName.Magery, 30.1, 40.0);
            SetSkill(SkillName.MagicResist, 99.1, 110.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 92.5);

            Fame = 9000;
            Karma = 9000;

            Tamable = true;
            ControlSlots = 3;
            ControlSlotsMax = 5;
            MinTameSkill = 108;
        }

        public GoodUnicorn(Serial serial)
            : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(GoodUnicorn),
                    Class.Magical,
                    MagicalAbility.Chivalry | MagicalAbility.Discordance | MagicalAbility.MageryMastery | MagicalAbility.Mysticism | MagicalAbility.Spellweaving | MagicalAbility.Poisoning | MagicalAbility.Bashing | MagicalAbility.Piercing | MagicalAbility.Slashing | MagicalAbility.WrestlingMastery,
                    PetTrainingHelper.SpecialAbilityUnicorn,
                    PetTrainingHelper.WepAbility11,
                    PetTrainingHelper.AreaEffectArea2,
                    3, 5);
            }
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override HideType HideType { get { return HideType.Horned; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("<BASEFONT COLOR=#FFD700>Exotic</BASEFONT>");
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
