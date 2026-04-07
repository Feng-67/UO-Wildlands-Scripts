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
    [CorpseName("a fire horse corpse")]
    public class FlameOfKindlehart : BaseMount
    {
        [Constructable]
        public FlameOfKindlehart() : base("Flame of Kindlehart", 1653, 16096, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // BodyValue: 1653 | MountID: 16096 — preserved from original

            BaseSoundID = 0xA8;

            // --- Taming ---
            Tamable = true;
            ControlSlots = 2;       // Spawns at 2 slots; trainable to 5
            MinTameSkill = 88.0;

            // --- Pack Instinct & Food ---
            //PackInstinct = PackInstinct.Equine;

            // --- Attributes (fixed values per bestiary — zero spread) ---
            SetStr(460);
            SetDex(180);
            SetInt(300);

            SetHits(410);
            SetStam(150);
            SetMana(300);

            // --- Damage Profile ---
            SetDamage(18, 26);
            SetDamageType(ResistanceType.Physical,  0);
            SetDamageType(ResistanceType.Fire,     100);

            // --- Resistances (fixed values — zero spread) ---
            SetResistance(ResistanceType.Physical, 60);
            SetResistance(ResistanceType.Fire,     90);
            SetResistance(ResistanceType.Cold,     30);
            SetResistance(ResistanceType.Poison,   40);
            SetResistance(ResistanceType.Energy,   50);

            // --- Skills ---
            SetSkill(SkillName.Wrestling,   90.1, 100.0);
            SetSkill(SkillName.Tactics,     90.1, 100.0);
            SetSkill(SkillName.MagicResist, 70.0,  90.0);
            SetSkill(SkillName.Anatomy,     50.0,  60.0);

            // --- Innate Special Ability ---
            SetSpecialAbility(SpecialAbility.Inferno);
        }

        // ------------------------------------------------------------------
        // Pet Training Profile
        //
        // MagicalAbility.Triton = Chivalry | Discordance | MageryMastery |
        //                         Mysticism | Poisoning | Spellweaving |
        //                         Bushido | Ninjitsu | BattleDefense |
        //                         Bashing | Piercing | Slashing | WrestlingMastery
        //
        // Special: inline { Inferno } — only one innate/trainable ability
        //          (same pattern as DreadWarhorse's single DragonBreath entry)
        //
        // WepAbility2    = 16-move set including ColdWind and Dismount
        // AreaEffectArea2 = all 6 area effects
        // ------------------------------------------------------------------
        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(FlameOfKindlehart),
                    Class.Magical | Class.Tokuno,
                    MagicalAbility.Triton,
                    new SpecialAbility[] { SpecialAbility.Inferno },
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea2,
                    2, 5);
            }
        }

        public FlameOfKindlehart(Serial serial) : base(serial)
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
