/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fire horse corpse")]
    public class ElementalFireHorse : BaseMount
    {
        [Constructable]
        public ElementalFireHorse() : this("an elemental fire horse")
        {
        }

        [Constructable]
        public ElementalFireHorse(string name) : base(name, 1653, 16096, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // 1653 is the BodyValue, 16096 is the MountID
            BaseSoundID = 0xA8; // Standard horse sound

            SetStr(302, 420);
            SetDex(131, 185);
            SetInt(101, 150);

            SetHits(151, 200);
            SetStam(131, 150);
            SetMana(101, 150);

            // --- Damage Profile ---
            SetDamage(14, 20);
            SetDamageType(ResistanceType.Fire, 100);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 85);
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

        public ElementalFireHorse(Serial serial) : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(typeof(ElementalFireHorse), Class.None,
                (
                    // Magical Schools
                    MagicalAbility.Chivalry |
                    //MagicalAbility.Discordance |
                    MagicalAbility.MageryMastery |
                    MagicalAbility.Mysticism |
                    MagicalAbility.Necromage |
                    MagicalAbility.Necromancy |
                    MagicalAbility.Poisoning |
                    MagicalAbility.Spellweaving |
                    //Tokuno
                    MagicalAbility.Bushido |
                    MagicalAbility.Ninjitsu |
                    //Melee
                    MagicalAbility.Bashing |
                    MagicalAbility.BattleDefense |
                    MagicalAbility.Piercing |
                    MagicalAbility.Slashing |
                    MagicalAbility.WrestlingMastery
                ),
                new SpecialAbility[]
                {
                    SpecialAbility.AngryFire,
                    SpecialAbility.ConductiveBlast,
                    SpecialAbility.DragonBreath,
                    SpecialAbility.GraspingClaw,
                    SpecialAbility.Inferno,
                    SpecialAbility.LifeLeech,
                    SpecialAbility.LightningForce,
                    SpecialAbility.ManaDrain,
                    SpecialAbility.RagingBreath,
                    SpecialAbility.Repel,
                    SpecialAbility.RuneCorruption,
                    SpecialAbility.SearingWounds,
                    SpecialAbility.StealLife,
                    SpecialAbility.StickySkin,
                    SpecialAbility.TailSwipe,
                    SpecialAbility.VenomousBite,
                    SpecialAbility.ViciousBite,
                },
                new WeaponAbility[]
                {
                    WeaponAbility.ArmorIgnore,
                    WeaponAbility.ArmorPierce,
                    WeaponAbility.Bladeweave,
                    WeaponAbility.BleedAttack,
                    WeaponAbility.Block,
                    WeaponAbility.ColdWind,
                    WeaponAbility.ConcussionBlow,
                    WeaponAbility.CrushingBlow,
                    WeaponAbility.Disarm,
                    WeaponAbility.Dismount,
                    WeaponAbility.Feint,
                    WeaponAbility.ForceOfNature,
                    WeaponAbility.FrenziedWhirlwind,
                    WeaponAbility.MortalStrike,
                    WeaponAbility.NerveStrike,
                    WeaponAbility.ParalyzingBlow,
                    WeaponAbility.PsychicAttack,
                    WeaponAbility.TalonStrike,
                },
                new AreaEffect[]
                {
                    AreaEffect.AuraOfEnergy,
                    AreaEffect.ExplosiveGoo,
                    AreaEffect.EssenceOfEarth,
                    AreaEffect.AuraOfNausea,
                    AreaEffect.PoisonBreath,
                    AreaEffect.EssenceOfDisease,
                },
                3, 5);
            }
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
