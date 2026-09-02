/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a clydesdale corpse")]
    public class ClydesdaleHorse : BaseMount
    {
        // Last Stand: prevents lethal damage to the rider once every 30 minutes.
        private DateTime m_LastStandUsed = DateTime.MinValue;
        private static readonly TimeSpan LastStandCooldown = TimeSpan.FromMinutes(30);

        [Constructable]
        public ClydesdaleHorse() : this("a clydesdale horse")
        {
        }

        [Constructable]
        public ClydesdaleHorse(string name) : base(name, 1651, 16094, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // BodyValue: 1651 | MountID: 16094 — preserved from original
            BaseSoundID = 0xA8;

            // Roll a random decimal between 0.00 and 1.00 (0% to 100%)
            double roll = Utility.RandomDouble();

            if (roll < 0.01) // 1% chance for Uber Rare
            {
                int[] uberRare = new int[] { 2775, 2781 };
                this.Hue = uberRare[Utility.Random(uberRare.Length)];
            }
            else if (roll < 0.05) // 4% chance for Very Rare
            {
                int[] veryRare = new int[] { 2075, 2753 };
                this.Hue = veryRare[Utility.Random(veryRare.Length)];
            }
            else if (roll < 0.15) // 10% chance for Rare
            {
                int[] rare = new int[] { 2659, 2605 };
                this.Hue = rare[Utility.Random(rare.Length)];
            }
            else if (roll < 0.40) // 25% chance for Uncommon
            {
                int[] uncommon = new int[] { 2500, 2678, 2120 };
                this.Hue = uncommon[Utility.Random(uncommon.Length)];
            }
            else if (roll < 0.70) // 30% chance for Common
            {
                int[] common = new int[] { 2761, 2763 };
                this.Hue = common[Utility.Random(common.Length)];
            }
            else // Remaining 30% chance for Very Common
            {
                this.Hue = 0;
            }

            // --- Attributes (source: uo-cah.com Clydesdale bestiary, tamed ranges) ---
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
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     35, 45);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   30, 40);

            // --- Skills ---
            SetSkill(SkillName.Wrestling,   50.0, 70.0);
            SetSkill(SkillName.Tactics,     50.0, 70.0);
            SetSkill(SkillName.MagicResist,  77.5, 100.0);
            SetSkill(SkillName.Anatomy,      50.0,  70.0);

            // --- Taming ---
            Tamable = true;
            ControlSlots = 1;       // Spawns at 1 slot; trainable to 5
            MinTameSkill = 94.0;
        }

        // ------------------------------------------------------------------
        // Last Stand
        // When the rider would receive lethal damage, the blow is reduced so
        // they survive with 1 HP. Fires at most once every 30 minutes.
        // ------------------------------------------------------------------
        public bool TryLastStand(Mobile rider, ref int damage)
        {
            if (rider == null || !rider.Alive)
                return false;

            // Only triggers if the hit would be lethal
            if (rider.Hits - damage > 0)
                return false;

            if (DateTime.UtcNow - m_LastStandUsed < LastStandCooldown)
                return false;

            // Clamp damage so the rider survives at 1 HP
            damage = rider.Hits - 1;
            m_LastStandUsed = DateTime.UtcNow;

            rider.SendLocalizedMessage(1158360); // "Your mount's Last Stand saves your life!"
            rider.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
            rider.PlaySound(0x1F7);

            return true;
        }

        // Returns the time remaining before Last Stand can trigger again
        public TimeSpan LastStandRemaining
        {
            get
            {
                TimeSpan elapsed = DateTime.UtcNow - m_LastStandUsed;
                return elapsed >= LastStandCooldown ? TimeSpan.Zero : LastStandCooldown - elapsed;
            }
        }
               

        // ------------------------------------------------------------------
        // Serialization  (version 1 adds m_LastStandUsed)
        // ------------------------------------------------------------------
        public ClydesdaleHorse(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write(m_LastStandUsed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
                m_LastStandUsed = reader.ReadDateTime();
        }
    }
}
