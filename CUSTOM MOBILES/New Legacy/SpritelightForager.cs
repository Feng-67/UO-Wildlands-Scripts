/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a spritelight forager corpse")]
    public class SpritelightForager : BaseMount
    {
        [Constructable]
        public SpritelightForager() : this("a spritelight forager")
        {
        }

        [Constructable]
        public SpritelightForager(string name) : base(name, 1652, 16095, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // 1652 is the BodyValue, 16095 is the MountID
            BaseSoundID = 0xA8;
                        
            SetStr(460, 460);
            SetDex(180, 180);
            SetInt(300, 300);

            SetHits(410, 410);
            SetStam(150, 150);
            SetMana(300, 300);

            // --- Damage Profile ---
            SetDamage(18, 26);
            SetDamageType(ResistanceType.Poison, 100);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 60);
            SetResistance(ResistanceType.Fire, 40, 40);
            SetResistance(ResistanceType.Cold, 30, 30);
            SetResistance(ResistanceType.Poison, 90, 90);
            SetResistance(ResistanceType.Energy, 50, 50);

            // --- Skills ---
            SetSkill(SkillName.Wrestling, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 77.5, 100.0);
            SetSkill(SkillName.Anatomy, 50.0, 70.0);

            // --- Taming ---
            Tamable = true;
            ControlSlots = 2;       
            MinTameSkill = 88.0;
        }
                
        public SpritelightForager(Serial serial) : base(serial)
        {
        }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
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
