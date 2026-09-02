/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a manticore corpse")]
    public class JuvenileManticore : BaseMount
    {
        [Constructable]
        public JuvenileManticore() : this("Juvenile Manticore")
        {
        }

        [Constructable]
        public JuvenileManticore(string name) : base(name, 1553, 16091, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // BodyValue: 1553 | MountID: 16091 — preserved from original
            BaseSoundID = 0x27D;

            // --- Random Hue Assignment ---
            double roll = Utility.RandomDouble();

            if (roll < 0.005) // 0.5% chance: Uber Rare
            {
                this.Hue = 2767; // Manticore Blue-Green
            }
            else if (roll < 0.02) // 1.5% chance: Extremely Rare
            {
                this.Hue = 2770; // Manticore Lavender
            }
            else if (roll < 0.05) // 3% chance: Very Rare
            {
                this.Hue = 2766; // Manticore Pink
            }
            else if (roll < 0.15) // 10% chance: Rare
            {
                int[] rare = new int[] { 2768, 2592 }; // Decay, Metallic Blue
                this.Hue = rare[Utility.Random(rare.Length)];
            }
            else if (roll < 0.30) // 15% chance: More Uncommon
            {
                int[] moreUncommon = new int[] { 2762, 2765 }; // Manticore Orange, Manticore Cocoa
                this.Hue = moreUncommon[Utility.Random(moreUncommon.Length)];
            }
            else if (roll < 0.50) // 20% chance: Uncommon
            {
                int[] uncommon = new int[] { 2764, 1330 }; // Shogun Grey, Light Blue
                this.Hue = uncommon[Utility.Random(uncommon.Length)];
            }
            else if (roll < 0.75) // 25% chance: Common
            {
                int[] common = new int[] { 2763, 2761 }; // Manticore Tan, Manticore Brown
                this.Hue = common[Utility.Random(common.Length)];
            }
            else // Remaining 25%: Very Common
            {
                this.Hue = Utility.RandomBool() ? 0 : 2769; // White or Off-White
            }

            // --- Taming ---
            Tamable = true;
            ControlSlots = 3;      
            MinTameSkill = 108.0;

            // --- Attributes ---
            SetStr(551, 600);
            SetDex(151, 180);
            SetInt(301, 350);

            SetHits(451, 525);
            SetStam(151, 180);
            SetMana(301, 350);

            // --- Damage Profile (100% Physical) ---
            SetDamage(14, 26);
            SetDamageType(ResistanceType.Physical, 100);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire,     55, 65);
            SetResistance(ResistanceType.Cold,     55, 65);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   60, 70);

            // --- Skills ---
            SetSkill(SkillName.Wrestling,   100.1, 115.0);
            SetSkill(SkillName.Tactics,     100.1, 115.0);
            SetSkill(SkillName.MagicResist,  80.0, 100.0);
            SetSkill(SkillName.Anatomy,      50.0,  70.0);
                        
        }
                
        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public JuvenileManticore(Serial serial) : base(serial)
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
