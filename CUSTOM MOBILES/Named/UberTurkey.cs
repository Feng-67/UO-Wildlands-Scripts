/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a turkey corpse")]
    public class UberTurkey : BaseCreature
    {
        private DateTime m_NextGobble;

        // ------------------------------------------------------------------
        // Constructors
        // Default spawn is tamable; pass false for non-tamable event variants.
        // ------------------------------------------------------------------
        [Constructable]
        public UberTurkey() : this(true)
        {
        }

        [Constructable]
        public UberTurkey(bool tamable) : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name        = "an Uber Turkey";
            Body        = 1026;
            BaseSoundID = 0x66A;

            // 50% chance for a special hue, 50% chance for default (0)
            if (Utility.RandomDouble() < 0.5)
            {
                // One of your 6 favorite special hues
                Hue = Utility.RandomList(2101, 1150, 1109, 1901, 1143, 2012);
            }
            else
            {
                // Normal/Natural turkey color
                Hue = 0;
            }

            // --- Taming ---
            Tamable      = tamable;
            ControlSlots = 3;       // Spawns at 3 slots; trainable to 5
            MinTameSkill = 108.0;

            // --- Attributes ---
            // Designed as a genuinely powerful 3-slot creature. Fast and
            // aggressive — built like a predatory bird, not a farmyard animal.
            SetStr(350, 450);
            SetDex(120, 160);
            SetInt(75,  125);

            SetHits(300, 400);
            SetStam(120, 160);
            SetMana(75,  125);

            // --- Damage Profile (pure Physical — talons and beak) ---
            SetDamage(14, 20);
            SetDamageType(ResistanceType.Physical, 100);

            // --- Resistances ---
            // Tough feathered hide; above-average Poison resist (barn birds
            // historically immune to all kinds of filth).
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     25, 35);
            SetResistance(ResistanceType.Poison,   35, 45);
            SetResistance(ResistanceType.Energy,   30, 40);

            // --- Skills ---
            SetSkill(SkillName.Wrestling,   90.1, 105.0);
            SetSkill(SkillName.Tactics,     85.0, 100.0);
            SetSkill(SkillName.MagicResist, 65.0,  80.0);
            SetSkill(SkillName.Anatomy,     40.0,  60.0);

            // --- Fame / Karma ---
            Fame  = 6500;
            Karma = 0;

            // --- Gobble cooldown init ---
            m_NextGobble = DateTime.UtcNow;
        }

        // ------------------------------------------------------------------
        // Pet Training Profile
        //
        // Class.Clawed → the Uber Turkey uses powerful talons (GraspingClaw).
        //   No tail classification.
        //
        // MagicalAbility.Cusidhe (exact named composite):
        //   Chivalry | Discordance | Mysticism | Poisoning | Spellweaving |
        //   WrestlingMastery
        //   This is the standard CuSidhe-tier package — the correct choice
        //   for a 3–5 slot Clawed creature. Poisoning fits the "foul fowl"
        //   theme; Discordance is thematically perfect for a bird.
        //
        // SpecialAbilityClawed:
        //   ManaDrain | Repel | SearingWounds | GraspingClaw
        //   GraspingClaw = "talon strike" for a bird. Appropriate innate
        //   special pool for a Clawed creature at this tier.
        //
        // WepAbility1 = 15-move set (standard; excludes ColdWind which is
        //   reserved for higher-tier or cold-themed creatures).
        //
        // AreaEffectArea1 = EssenceOfEarth | ExplosiveGoo | AuraOfEnergy
        //   Same package as CuSidhe and similar 3–5 slot Clawed pets.
        // ------------------------------------------------------------------
        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(UberTurkey),
                    Class.Clawed,
                    MagicalAbility.Cusidhe,
                    PetTrainingHelper.SpecialAbilityClawed,
                    PetTrainingHelper.WepAbility1,
                    PetTrainingHelper.AreaEffectArea1,
                    3, 5);
            }
        }

        // ------------------------------------------------------------------
        // Creature Properties
        // ------------------------------------------------------------------
        public override bool CanAngerOnTame { get { return true; } }
        public override int      Meat         { get { return 4; } }   // Uber Turkey = bigger meal
        public override MeatType MeatType     { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override int      Feathers     { get { return 50; } }  // Uber = more feathers

        // ------------------------------------------------------------------
        // Sounds — preserve original turkey audio
        // ------------------------------------------------------------------
        public override int GetIdleSound()  { return 0x66A; }
        public override int GetAngerSound() { return 0x66A; }
        public override int GetHurtSound()  { return 0x66B; }
        public override int GetDeathSound() { return 0x66B; }

        // ------------------------------------------------------------------
        // Gobble mechanic
        // Wild tamable Uber Turkeys periodically gobble to announce themselves.
        // Once tamed they go silent — a tamed Uber Turkey means business.
        // ------------------------------------------------------------------
        public override void OnThink()
        {
            base.OnThink();

            if (Tamable && !Controlled && m_NextGobble < DateTime.UtcNow)
            {
                Say(1153511); // *gobble* *gobble*
                PlaySound(GetIdleSound());

                m_NextGobble = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 240));
            }
        }

        // ------------------------------------------------------------------
        // Serialization
        // ------------------------------------------------------------------
        public UberTurkey(Serial serial) : base(serial)
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

            m_NextGobble = DateTime.UtcNow; // reset on load; don't gobble instantly on first tick
        }
    }
}
