/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;

namespace Server.Mobiles
{
    [CorpseName("a polar bear corpse")]
    [TypeAlias("Server.Mobiles.Polarbear")]
    public class PolarBear : BaseMount, ILegendaryPet
    {
        private bool m_IsLegendary;
        public bool IsLegendary => m_IsLegendary;

        [Constructable]
        public PolarBear()
            : base("a polar bear", 213, 0x3EC5, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.BaseSoundID = 0xA3;

            this.SetStr(116, 140);
            this.SetDex(81, 105);
            this.SetInt(26, 50);

            this.SetHits(70, 84);
            this.SetMana(0);

            this.SetDamage(7, 12);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 35);
            this.SetResistance(ResistanceType.Cold, 60, 80);
            this.SetResistance(ResistanceType.Poison, 15, 25);
            this.SetResistance(ResistanceType.Energy, 10, 15);

            this.SetSkill(SkillName.MagicResist, 45.1, 60.0);
            this.SetSkill(SkillName.Tactics, 60.1, 90.0);
            this.SetSkill(SkillName.Wrestling, 45.1, 70.0);

            this.Fame = 1500;
            this.Karma = 0;

            this.VirtualArmor = 18;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = 35.1;
        }

        public PolarBear(Serial serial)
            : base(serial)
        {
        }

        public override void OnBeforeSpawn(Point3D location, Map map)
        {
            base.OnBeforeSpawn(location, map);

            if (map == Map.Tokuno && IsInWinterSpur(location.X, location.Y))
            {
                if (Utility.RandomDouble() < 0.10)
                {
                    ConvertToLegendary();
                }
            }
        }

        private bool IsInWinterSpur(int x, int y)
        {
            // Define Triangle Vertices: A(846, 139), B(1002, 146), C(905, 34)
            float d1 = GetSide(x, y, 846, 139, 1002, 146);
            float d2 = GetSide(x, y, 1002, 146, 905, 34);
            float d3 = GetSide(x, y, 905, 34, 846, 139);

            bool has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

        private float GetSide(int px, int py, int x1, int y1, int x2, int y2)
        {
            return (px - x2) * (y1 - y2) - (x1 - x2) * (py - y2);
        }

        public void ConvertToLegendary()
        {
            m_IsLegendary = true;
            this.Name = "a polar bear";

            int[] legendaryHues = new int[] { 1153, 2953 };
            this.Hue = legendaryHues[Utility.Random(legendaryHues.Length)];

            this.AI = AIType.AI_Melee;

            this.SetDamage(21, 28);
            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Cold, 50);
            this.SetDamageType(ResistanceType.Energy, 50);

            this.SetHits(1010, 1275);

            this.SetResistance(ResistanceType.Physical, 50, 65);
            this.SetResistance(ResistanceType.Fire, 25, 45);
            this.SetResistance(ResistanceType.Cold, 70, 85);
            this.SetResistance(ResistanceType.Poison, 30, 50);
            this.SetResistance(ResistanceType.Energy, 70, 85);

            this.SetSkill(SkillName.Wrestling, 90.1, 96.8);
            this.SetSkill(SkillName.Tactics, 90.3, 99.3);
            this.SetSkill(SkillName.MagicResist, 75.3, 90.0);
            this.SetSkill(SkillName.Anatomy, 65.5, 69.4);
            this.SetSkill(SkillName.Healing, 72.2, 98.9);

            this.MinTameSkill = 120.0;
            this.ControlSlots = 2;
            this.ControlSlotsMax = 5;
            this.SetSpecialAbility(SpecialAbility.ColossalRage);
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                if (!m_IsLegendary)
                    return null;

                return new TrainingDefinition(
                    typeof(PolarBear),
                    Class.Clawed,
                    MagicalAbility.Chivalry | MagicalAbility.Discordance | MagicalAbility.Poisoning | MagicalAbility.BattleDefense | MagicalAbility.Bashing | MagicalAbility.Piercing | MagicalAbility.Slashing | MagicalAbility.WrestlingMastery,
                    new SpecialAbility[] { SpecialAbility.GraspingClaw, SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.LifeLeech, SpecialAbility.StealLife, SpecialAbility.FlurryForce },
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea2,
                    2, 5);
            }
        }

        public override bool CanAngerOnTame { get { return m_IsLegendary; } }

        public override int Meat
        {
            get
            {
                return 2;
            }
        }
        public override int Hides
        {
            get
            {
                return 16;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Bear;
            }
        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_IsLegendary)
            {
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
