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
    [CorpseName("a bone cat corpse")]
    public class BoneCat : BaseMount
    {
        [Constructable]
        public BoneCat()
            : this("Bone Cat")
        {
        }

        [Constructable]
        public BoneCat(string name)
            : base(name, 1441, 16080, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x69;

            SetStr(200, 280);
            SetDex(150, 200);
            SetInt(50, 100);

            SetHits(280, 360);

            SetDamage(10, 16);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 60);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 65.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 75.0);
            SetSkill(SkillName.Necromancy, 30.0, 45.0);
            SetSkill(SkillName.SpiritSpeak, 30.0, 45.0);

            Fame = 5000;
            Karma = -5000;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 108;

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public BoneCat(Serial serial)
            : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(typeof(BoneCat), Class.None,
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
                    SpecialAbility.GraspingClaw,
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
                    //WeaponAbility.ColdWind,
                    WeaponAbility.ConcussionBlow,
                    //WeaponAbility.CrushingBlow,
                    //WeaponAbility.Disarm,
                    WeaponAbility.Dismount,
                    WeaponAbility.Feint,
                    WeaponAbility.ForceOfNature,
                    //WeaponAbility.FrenziedWhirlwind,
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

        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
              
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
