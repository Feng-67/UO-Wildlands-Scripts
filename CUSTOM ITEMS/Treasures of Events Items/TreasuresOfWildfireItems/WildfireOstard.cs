using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a wildfire ostard corpse")]
    public class WildfireOstard : FrenziedOstard
    {
        public static readonly int MaxPower = 10;

        [CommandProperty(AccessLevel.GameMaster)]
        public int PowerLevel { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PowerDecay
        {
            get { return _PowerDecay; }
            set
            {
                _PowerDecay = value;

                if (_PowerDecay >= 10)
                {
                    _PowerDecay = 0;
                    PowerLevel = Math.Max(1, PowerLevel - 1);
                }
            }
        }

        private DateTime _NextSpecial;
        private int _PowerDecay;
        
        [Constructable]
        public WildfireOstard()
            : base()
        {
            Name = "Wildfire Ostard";
            Hue = 2758;

            SetStr(540);
            SetDex(100);
            SetInt(150);

            SetHits(400);
            SetStam(100);
            SetMana(150);

            SetDamage(16, 22);

            SetResistance(ResistanceType.Physical, 65);
            SetResistance(ResistanceType.Fire, 45);
            SetResistance(ResistanceType.Cold, 40);
            SetResistance(ResistanceType.Poison, 55);
            SetResistance(ResistanceType.Energy, 35);

            SetSkill(SkillName.Anatomy, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 96.0;

            PowerLevel = 10;
            _NextSpecial = DateTime.UtcNow;
        }

        public WildfireOstard(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteOnRelease => true;
        public override Poison HitPoison => Poison.Lethal;
        public override FoodType FavoriteFood => FoodType.BlackrockStew;

        public override bool CheckFeed(Mobile from, Item dropped)
        {
            if (dropped is BowlOfBlackrockStew)
            {
                if (PowerLevel >= MaxPower)
                {
                    from.SendLocalizedMessage(1115755); // The creature looks at you strangely and shakes its head no.
                }
                else
                {
                    PowerLevel++;

                    if (PowerLevel >= MaxPower)
                    {
                        from.SendLocalizedMessage(1115753); // Your bane dragon is returned to maximum power by this stew.
                    }
                    else
                    {
                        from.SendLocalizedMessage(1115754); // Your bane dragon seems a bit peckish today and is not at full power.
                    }

                    return base.CheckFeed(from, dropped);
                }
            }

            return false;
        }

        public override void OnDamagedBySpell( Mobile caster )
		{
			if (_NextSpecial < DateTime.UtcNow)
            {
                DoSpecial(caster);

                _NextSpecial = DateTime.UtcNow + TimeSpan.FromSeconds((double)Utility.RandomMinMax(15, 30) * (double)(11.0 - PowerLevel));
            }

			base.OnDamagedBySpell( caster );
		}
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			if (_NextSpecial < DateTime.UtcNow)
            {
                DoSpecial(attacker);

                _NextSpecial = DateTime.UtcNow + TimeSpan.FromSeconds((double)Utility.RandomMinMax(15, 30) * (double)(11.0 - PowerLevel));
            }

			base.OnGotMeleeAttack( attacker );
		}

        public void DoSpecial(Mobile from)
        {
            if (Controlled)
            {
                PowerDecay++;
            }

            if (from == null)
                return;
            
            from.FixedEffect(0x9DAC, 3, 32, 59, 0);
            
            PlaySound(0x15E);

            Timer.DelayCall(TimeSpan.FromSeconds(1), m =>
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(8 * PowerLevel, 10 * PowerLevel), 0, 50, 0, 50, 0);
                    m.ApplyPoison(this, GetHitPoison());
                }, from);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add($"Power Level: {PowerLevel}");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(typeof(WildfireOstard), Class.None, 
                (                        
                    // Magical Schools
                    MagicalAbility.Chivalry |
                    MagicalAbility.Discordance |
                    MagicalAbility.MageryMastery |
                    MagicalAbility.Mysticism |
                    //--MagicalAbility.Necromage |
                    //--MagicalAbility.Necromancy |
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
                    //--SpecialAbility.LifeLeech,
                    SpecialAbility.LightningForce,
                    SpecialAbility.ManaDrain,
                    SpecialAbility.RagingBreath,
                    SpecialAbility.Repel,
                    //--SpecialAbility.RuneCorruption,
                    SpecialAbility.SearingWounds,
                    //--SpecialAbility.StealLife,
                    //--SpecialAbility.StickySkin,
                    //--SpecialAbility.TailSwipe,
                    SpecialAbility.VenomousBite,
                    SpecialAbility.ViciousBite,
                }, 
                new WeaponAbility[] 
                {
                    WeaponAbility.ArmorIgnore,
                    WeaponAbility.ArmorPierce,
                    WeaponAbility.Bladeweave,
                    WeaponAbility.BleedAttack,
                    //--WeaponAbility.Block,
                    WeaponAbility.ColdWind,
                    WeaponAbility.ConcussionBlow,
                    WeaponAbility.CrushingBlow,
                    //--WeaponAbility.Disarm,
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
    }
}

namespace Server.Items
{
    public class WildfireOstardStatuette : BaseImprisonedMobile
    {
        [Constructable]
        public WildfireOstardStatuette()
            : base(0x2136)
        {
            this.Weight = 1.0;
            this.Hue = 2758;
        }

        public WildfireOstardStatuette(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1159675; //Wildfire Ostard
        public override BaseCreature Summon
        {
            get
            {
                return new WildfireOstard();
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