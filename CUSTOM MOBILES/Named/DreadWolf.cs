/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;
namespace Server.Mobiles
{
    [CorpseName("a dread wolf corpse")]
    public class DreadWolf : BaseMount
    {
        [Constructable]
        public DreadWolf()
            : this("dread wolf")
        {
        }

        [Constructable]
        public DreadWolf(string name)
            : base(name, 1410, 16076, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xA8;

            SetStr(500, 555);
            SetDex(85, 125);
            SetInt(50, 60);

            SetHits(625, 700);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 25, 45);

            SetSkill(SkillName.MagicResist, 125.0, 150.0);
            SetSkill(SkillName.Tactics, 80.0, 95.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Poisoning, 90.0, 100.0);
            SetSkill(SkillName.DetectHidden, 45.0, 55.0);

            Fame = 15000;
            Karma = -15000;

            SetMagicalAbility(MagicalAbility.Poisoning);
            
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 108;
        }

        public DreadWolf(Serial serial)
            : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(typeof(DreadWolf), Class.None,
                (
                    // Magical Schools
                    //MagicalAbility.Chivalry |
                    //MagicalAbility.Discordance |
                    //MagicalAbility.MageryMastery |
                    //MagicalAbility.Mysticism |
                    MagicalAbility.Necromage |
                    MagicalAbility.Necromancy |
                    MagicalAbility.Poisoning |
                    //MagicalAbility.Spellweaving |
                    //Tokuno
                    //MagicalAbility.Bushido |
                    //MagicalAbility.Ninjitsu |
                    //Melee
                    //MagicalAbility.Bashing |
                    //MagicalAbility.BattleDefense |
                    //MagicalAbility.Piercing |
                    //MagicalAbility.Slashing |
                    MagicalAbility.WrestlingMastery
                ),
                new SpecialAbility[]
                {
                    //SpecialAbility.AngryFire,
                    //SpecialAbility.ConductiveBlast,
                    //SpecialAbility.DragonBreath,
                    //SpecialAbility.GraspingClaw,
                    //SpecialAbility.Inferno,
                    SpecialAbility.LifeLeech,
                    //SpecialAbility.LightningForce,
                    SpecialAbility.ManaDrain,
                    //SpecialAbility.RagingBreath,
                    SpecialAbility.Repel,
                    //SpecialAbility.RuneCorruption,
                    SpecialAbility.SearingWounds,
                    //SpecialAbility.StealLife,
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
                    //AreaEffect.AuraOfEnergy,
                    //AreaEffect.ExplosiveGoo,
                    //AreaEffect.EssenceOfEarth,
                    AreaEffect.AuraOfNausea,
                    AreaEffect.PoisonBreath,
                    AreaEffect.EssenceOfDisease,
                },
                3, 5);
            }
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
              
        public override int GetIdleSound()
        {
            return 0x577;
        }

        public override int GetAttackSound()
        {
            return 0x576;
        }

        public override int GetAngerSound()
        {
            return 0x578;
        }

        public override int GetHurtSound()
        {
            return 0x576;
        }

        public override int GetDeathSound()
        {
            return 0x579;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version bump 0 -> 1
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
