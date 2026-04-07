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
    [CorpseName("an earth horse corpse")]
    public class ElementalEarthHorse : BaseMount
    {
        [Constructable]
        public ElementalEarthHorse() : this("an elemental earth horse")
        {
        }

        [Constructable]
        public ElementalEarthHorse(string name) : base(name, 1652, 16095, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // 1652 is the BodyValue, 16095 is the MountID
            BaseSoundID = 0xA8;

            SetStr(302, 420);
            SetDex(131, 185);
            SetInt(101, 150);

            SetHits(151, 200);
            SetStam(131, 150);
            SetMana(101, 150);

            // --- Damage Profile ---
            SetDamage(14, 20);
            SetDamageType(ResistanceType.Physical, 100);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 85);
            SetResistance(ResistanceType.Fire, 35, 45);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // --- Skills ---
            SetSkill(SkillName.Wrestling, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 77.5, 100.0);
            SetSkill(SkillName.Anatomy, 50.0, 70.0);

            // --- Taming ---
            Tamable = true;
            ControlSlots = 1;       // Spawns at 1 slot; trainable to 5
            MinTameSkill = 94.0;
        }

        public ElementalEarthHorse(Serial serial) : base(serial)
        {
        }

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
