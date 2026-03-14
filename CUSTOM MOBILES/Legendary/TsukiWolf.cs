/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections;
using Server.Items;


namespace Server.Mobiles
{
    [CorpseName("a tsuki wolf corpse")]
    public class TsukiWolf : BaseCreature, ILegendaryPet
    {
        private bool m_IsLegendary; 
        public bool IsLegendary => m_IsLegendary;

        [Constructable]
        public TsukiWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tsuki wolf";
            Body = 250;

            switch (Utility.Random(3))
            {
                case 0:
                    Hue = Utility.RandomNeutralHue();
                    break; //No, this really isn't accurate ;->
            }

            SetStr(401, 450);
            SetDex(151, 200);
            SetInt(66, 76);

            SetHits(376, 450);
            SetMana(40);

            SetDamage(14, 18);

            SetDamageType(ResistanceType.Physical, 90);
            SetDamageType(ResistanceType.Cold, 5);
            SetDamageType(ResistanceType.Energy, 5);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 65.1, 72.0);
            SetSkill(SkillName.MagicResist, 65.1, 70.0);
            SetSkill(SkillName.Tactics, 95.1, 110.0);
            SetSkill(SkillName.Wrestling, 97.6, 107.5);
            SetSkill(SkillName.Necromancy, 20.0);
            SetSkill(SkillName.SpiritSpeak, 20.0);
            SetSkill(SkillName.DetectHidden, 100.0);
            SetSkill(SkillName.Parry, 90.0, 100.0);

            Fame = 8500;
            Karma = -8500;


            if (Core.ML && Utility.RandomDouble() < .33)
                PackItem(Engines.Plants.Seed.RandomPeculiarSeed(1));

            PackBodyPartOrBones();

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 96.0;

            SetSpecialAbility(SpecialAbility.Rage);
        }

        public TsukiWolf(Serial serial)
            : base(serial)
        {
        }

        // 1. Fixed the return type from 'bool' to 'void'
        public override void OnBeforeSpawn(Point3D location, Map map)
        {
            base.OnBeforeSpawn(location, map);

            // 10% chance Legendary version spawns from killing regular Tsuki Wolves
            if (Utility.RandomDouble() < 0.05)
            {
                ConvertToLegendary();
            }
        }

        public void ConvertToLegendary()
        {
            m_IsLegendary = true; // Set our flag to true
            this.Name = "a tsuki wolf";


            int[] legendaryHues = new int[] { 1929, 1918, 1910, 1462, 1158, 2716, 2747 };
            this.Hue = legendaryHues[Utility.Random(legendaryHues.Length)];

            this.ControlSlots = 2;
            this.MinTameSkill = 120.0;

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Cold, 10);
            this.SetDamageType(ResistanceType.Energy, 40);

            this.SetSpecialAbility(SpecialAbility.LifeLeech);
            this.SetSpecialAbility(SpecialAbility.Rage);
            //this.SetWeaponAbility(WeaponAbility.ArmorIgnore);
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                if (!m_IsLegendary)
                    return null;

                return new TrainingDefinition(
                    typeof(TsukiWolf),
                    Class.MagicalClawedTailedNecromanticAndTokuno,
                    MagicalAbility.TsukiWolf,
                    PetTrainingHelper.SpecialAbilityTsukiWolf,
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea2,
                    2, 5);
            }
        }

        public override bool CanAngerOnTame { get { return true; } }

        public override int TreasureMapLevel { get { return 3; } }
        public override int Meat { get { return 4; } }
        public override int Hides { get { return 25; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override int GetAngerSound()
        {
            return 0x52D;
        }

        public override int GetIdleSound()
        {
            return 0x52C;
        }

        public override int GetAttackSound()
        {
            return 0x52B;
        }

        public override int GetHurtSound()
        {
            return 0x52E;
        }

        public override int GetDeathSound()
        {
            return 0x52A;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_IsLegendary)
            {
                // #FFD700 is the hex code for Gold
                list.Add("<BASEFONT COLOR=#FFD700>Legendary</BASEFONT>");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version bump 0 -> 1
            writer.Write(m_IsLegendary);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
                m_IsLegendary = reader.ReadBool();
        }

        // --- ADD THE FIX HERE ---
        public override WeaponAbility GetWeaponAbility()
        {
            if (m_IsLegendary)
                return WeaponAbility.ArmorIgnore;

            return base.GetWeaponAbility();
        }
        // -----------------------

    } 
} 
