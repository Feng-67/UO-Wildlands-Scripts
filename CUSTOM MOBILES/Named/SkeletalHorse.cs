/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team] (Original Author Rutibex)
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Mobiles;
using Server.Items;
namespace Server.Mobiles
{
    [CorpseName("a skeletal horse corpse")]
    public class SkeletalHorse : BaseMount
    {
        [Constructable]
        public SkeletalHorse()
            : this("Skeletal Horse")
        {
        }

        [Constructable]
        public SkeletalHorse(string name)
            : base(name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // --- Taming ---
            Tamable = true;
            ControlSlots = 3;      
            MinTameSkill = 108.0;    

            // --- Attributes ---
            SetStr(400, 500);       
            SetDex(100, 125);       
            SetInt(150, 250);       

            SetHits(450, 600);      
            SetStam(100, 125);
            SetMana(150, 250);

            // --- Damage Profile ---
            SetDamage(18, 24);      

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.Wrestling, 90.1, 105.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 95.0);
            SetSkill(SkillName.Anatomy, 60.0, 80.0);

            // Innate: Necromancy (Enhanced)
            SetSkill(SkillName.Necromancy, 80.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);

            Fame = 12000;
            Karma = -12000;
        }
                

        public SkeletalHorse(Serial serial)
            : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(typeof(SkeletalHorse), Class.None,
                (
                    // Magical Schools
                    //MagicalAbility.Chivalry |
                    //MagicalAbility.Discordance |
                    //MagicalAbility.MageryMastery |
                    MagicalAbility.Mysticism |
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
                    SpecialAbility.AngryFire,
                    SpecialAbility.ConductiveBlast,
                    SpecialAbility.DragonBreath,
                    //SpecialAbility.GraspingClaw,
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
                    //AreaEffect.EssenceOfEarth,
                    AreaEffect.AuraOfNausea,
                    AreaEffect.PoisonBreath,
                    AreaEffect.EssenceOfDisease,
                },
                3, 5);
            }
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version < 2)
                reader.ReadInt();
        }
    }
}
