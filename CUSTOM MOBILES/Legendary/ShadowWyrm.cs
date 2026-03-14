/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;

namespace Server.Mobiles
{
    [CorpseName("a shadow wyrm corpse")]
    public class ShadowWyrm : BaseCreature, ILegendaryPet
    {
        private bool m_IsLegendary;
        public bool IsLegendary => m_IsLegendary;

        [Constructable]
        public ShadowWyrm()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow wyrm";
            Body = 106;
            BaseSoundID = 362;

            SetStr(898, 1030);
            SetDex(68, 200);
            SetInt(488, 620);

            SetHits(558, 599);

            SetDamage(29, 35);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.Meditation, 52.5, 75.0);
            SetSkill(SkillName.MagicResist, 100.3, 130.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 97.6, 100.0);
            SetSkill(SkillName.DetectHidden, 90.0, 100.0);
            SetSkill(SkillName.Necromancy, 80.0, 90.0);
            SetSkill(SkillName.SpiritSpeak, 100.0, 105.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            Tamable = true;
            ControlSlots = 5;
            MinTameSkill = 105.0;

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public ShadowWyrm(Serial serial)
            : base(serial)
        {
        }

        public override void OnBeforeSpawn(Point3D location, Map map)
        {
            base.OnBeforeSpawn(location, map);

            // 10% chance Legendary version spawns
            if (Utility.RandomDouble() < 0.10)
            {
                ConvertToLegendary();
            }
        }

        public void ConvertToLegendary()
        {
            m_IsLegendary = true;
            this.Name = "a shadow wyrm";

            this.Hue = 1910;
            this.ControlSlots = 2;
            this.MinTameSkill = 120.0;

            // Attributes and Stats
            this.SetStr(500, 600);
            this.SetDex(150, 200);
            this.SetInt(600, 700);
            this.SetHits(450, 500);

            // Damage Types (Cold 20%, Poison 80%)
            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Fire, 0);
            this.SetDamageType(ResistanceType.Cold, 20);
            this.SetDamageType(ResistanceType.Poison, 80);
            this.SetDamageType(ResistanceType.Energy, 0);

            // Resistances
            this.SetResistance(ResistanceType.Poison, 60, 70);

            // Added Skills
            this.SetSkill(SkillName.Necromancy, 130.0, 150.0);
            this.SetSkill(SkillName.SpiritSpeak, 130.0, 150.0);

            // --- Special Abilities & Immunities ---

            // Remove DragonBreath, then add Life Leech
            this.RemoveSpecialAbility(SpecialAbility.DragonBreath);
            this.SetSpecialAbility(SpecialAbility.LifeLeech);
            this.RemoveMagicalAbility(MagicalAbility.Magery);
            this.SetMagicalAbility(MagicalAbility.Necromancy);
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                if (!m_IsLegendary)
                    return null;

                return new TrainingDefinition(
                    typeof(ShadowWyrm),
                    Class.MagicalAndNecromantic,
                    MagicalAbility.Necromage | MagicalAbility.Necromancy | MagicalAbility.Poisoning | MagicalAbility.Discordance | MagicalAbility.Bashing | MagicalAbility.Piercing | MagicalAbility.Slashing | MagicalAbility.WrestlingMastery,
                    PetTrainingHelper.SpecialAbilityNecroMagical,
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea2,
                    2, 5);
            }
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool AutoDispel { get { return !Controlled; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override int Meat { get { return 19; } }
        public override int Hides { get { return 20; } }
        public override int Scales { get { return 10; } }
        public override ScaleType ScaleType { get { return ScaleType.Black; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override bool CanFly { get { return true; } }



        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);
        }

        public override int GetIdleSound()
        {
            return 0x2D5;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
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
    }
}
