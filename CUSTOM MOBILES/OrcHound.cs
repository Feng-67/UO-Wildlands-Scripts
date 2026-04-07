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
    [CorpseName("an orc hound corpse")]
    public class OrcHound : BaseCreature
    {
        [Constructable]
        public OrcHound() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an orc hound";
            Body = 1576;
            BaseSoundID = 0x3EE; // Standard Hound/Wolf sound

            // Stats from HTML: Str 126-155, Dex 81-105, Int 11-25
            SetStr(126, 155);
            SetDex(81, 105);
            SetInt(11, 25);

            // Hits: 151-200, Stam: 81-105, Mana: 11-25
            SetHits(151, 200);
            SetStam(81, 105);
            SetMana(11, 25);

            // Damage: 13-22 (100% Physical)
            SetDamage(13, 22);
            SetDamageType(ResistanceType.Physical, 100);

            // Resistances from HTML
            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills from HTML
            SetSkill(SkillName.Wrestling, 80.1, 90.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Anatomy, 80.1, 90.0);

            Fame = 1500;
            Karma = -1500;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 0.0;
        }

        // Orc Hounds are part of the Orc Tribe for Slayers
        public override TribeType Tribe { get { return TribeType.Orc; } }
        //public override SlayerGroup SlayerGroup { get { return SlayerGroup.Repond; } }

        // Feed it meat
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public OrcHound(Serial serial) : base(serial)
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
