/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Mobiles
{
    public interface ILegendaryPet
    {
        bool IsLegendary { get; }
    }

    [CorpseName("a fire steed corpse")]
    public class FireSteed : BaseMount, ILegendaryPet
    {
        private bool m_IsLegendary;
        public bool IsLegendary => m_IsLegendary;

        [Constructable]
        public FireSteed()
            : this("a fire steed")
        {
        }

        [Constructable]
        public FireSteed(string name)
            : base(name, 0xBE, 0x3E9E, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xA8;
            Hue = 1161;

            SetStr(376, 400);
            SetDex(91, 120);
            SetInt(291, 300);

            SetHits(226, 240);

            SetDamage(11, 30);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 20000;
            Karma = -20000;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 106.0;

            PackItem(new SulfurousAsh(Utility.RandomMinMax(151, 300)));
            PackItem(new Ruby(Utility.RandomMinMax(16, 30)));

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public FireSteed(Serial serial)
            : base(serial)
        {
        }

        public override void OnBeforeSpawn(Point3D location, Map map)
        {
            base.OnBeforeSpawn(location, map);

            if (Utility.RandomDouble() < 0.10)
            {
                ConvertToLegendary();
            }
        }

        public void ConvertToLegendary()
        {
            m_IsLegendary = true;
            this.Name = "a fire steed";
            this.Hue = 1174;
            this.ControlSlots = 1;
            this.MinTameSkill = 120.0;
            this.SetStr(410, 440);
            this.SetHits(300, 375);
            this.SetSkill(SkillName.Magery, 80.0, 100.0);
            this.RemoveSpecialAbility(SpecialAbility.DragonBreath);
            this.SetSpecialAbility(SpecialAbility.Inferno);
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                if (m_IsLegendary)
                {
                    return new TrainingDefinition(
                        typeof(FireSteed),
                        Class.Magical,
                        MagicalAbility.Dragon2,
                        PetTrainingHelper.SpecialAbilityMagical2,
                        PetTrainingHelper.WepAbility2,
                        PetTrainingHelper.AreaEffectArea2,
                        1, 5);
                }
                return null;
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

        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Daemon | PackInstinct.Equine;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2);
            writer.Write(m_IsLegendary);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version < 1)
            {
                for (int i = 0; i < Skills.Length; ++i)
                {
                    Skills[i].Cap = Math.Max(100.0, Skills[i].Cap * 0.9);
                    if (Skills[i].Base > Skills[i].Cap) Skills[i].Base = Skills[i].Cap;
                }
            }

            if (version >= 2)
                m_IsLegendary = reader.ReadBool();
        }
    }
}
