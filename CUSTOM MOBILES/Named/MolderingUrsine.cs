/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a moldering ursine corpse")]
    public class MolderingUrsine : BaseMount
    {
        [Constructable]
        public MolderingUrsine() : this("Moldering Ursine")
        {
        }

        [Constructable]
        public MolderingUrsine(string name) : base(name, 1638, 16092, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // BodyValue: 1638 | MountID: 16092 — preserved from original
            BaseSoundID = 0xA3; // Bear sound

            // --- Taming ---
            Tamable = true;
            ControlSlots = 3;      
            MinTameSkill = 108.0;
                        
            // --- Attributes (fixed values — zero spread per bestiary) ---
            SetStr(500);
            SetDex(165);
            SetInt(200);

            SetHits(500);
            SetStam(150);
            SetMana(200);

            // --- Damage Profile ---
            SetDamage(16, 22);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold,     35);
            SetDamageType(ResistanceType.Energy,   35);

            // --- Resistances (fixed values — zero spread per bestiary) ---
            SetResistance(ResistanceType.Physical, 65);
            SetResistance(ResistanceType.Fire,     40);
            SetResistance(ResistanceType.Cold,     55);
            SetResistance(ResistanceType.Poison,   40);
            SetResistance(ResistanceType.Energy,   55);

            // --- Skills ---
            SetSkill(SkillName.Wrestling,   100.1, 115.0);
            SetSkill(SkillName.Tactics,     100.1, 115.0);
            SetSkill(SkillName.MagicResist,  80.0, 100.0);
            SetSkill(SkillName.Anatomy,      50.0,  70.0);

            // --- Innate Special Ability ---
            SetSpecialAbility(SpecialAbility.LifeLeech);
        }
              
        
        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public MolderingUrsine(Serial serial) : base(serial)
        {
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
