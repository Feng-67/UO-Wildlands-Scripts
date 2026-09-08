/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author JesteR)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */

using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a corpse")]
    public class RussianTerrier : BaseMount
    {
        public override double HealChance { get { return 1.0; } }

        [Constructable]
        public RussianTerrier()
            : this("Russian Terrier")
        {
        }


        [Constructable]
        public RussianTerrier(string name) : base(name, 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            int random = Utility.Random(4);


            Body = 0x60F;
            ItemID = 0x3ED8;
            Hue = 0;

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

        public RussianTerrier(Serial serial)
            : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(typeof(RussianTerrier), Class.None,
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
                    //MagicalAbility.Bashing |
                    //MagicalAbility.BattleDefense |
                    //MagicalAbility.Piercing |
                    //MagicalAbility.Slashing |
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
                    //SpecialAbility.RuneCorruption,
                    SpecialAbility.SearingWounds,
                    SpecialAbility.StealLife,
                    //SpecialAbility.StickySkin,
                    //SpecialAbility.TailSwipe,
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

