/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author JesteR)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */

using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corpse")]
    public class GreatDane : BaseMount
    {
        public override double HealChance { get { return 1.0; } }

        [Constructable]
        public GreatDane()
            : this("Great Dane")
        {
        }


        [Constructable]
        public GreatDane(string name) : base(name, 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            int random = Utility.Random(4);


            Body = 0x60C;
            ItemID = 0x3ED6;
            Hue = 1117;

            SetStr(650);
            SetDex(190);
            SetInt(125);

            SetHits(600);
            SetMana(475);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 25, 45);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Wrestling, 90.1, 105.8);
            SetSkill(SkillName.Tactics, 89.3, 98.3);
            SetSkill(SkillName.Parry, 55.5, 70.4);
            SetSkill(SkillName.MagicResist, 59.3, 69.0);
            SetSkill(SkillName.Anatomy, 55.5, 70.4);
            SetSkill(SkillName.Healing, 72.2, 98.9);

            Fame = 24000;
            Karma = -24000;

            Tamable = true;
            ControlSlots = 3;
            ControlSlotsMax = 5;
            MinTameSkill = 100.0;

        }

        public GreatDane(Serial serial)
            : base(serial)
        {
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
                
        public override int GetAngerSound() { return 0x85; }
        public override int GetIdleSound() { return 0x85; }
        public override int GetAttackSound() { return 0x85; }
        public override int GetHurtSound() { return 0x84; }
        public override int GetDeathSound() { return 0x84; }

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

