/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ossein ram corpse")]
    public class OsseinRam : BaseCreature, ILegendaryPet
    {
        private bool m_IsLegendary;
        public bool IsLegendary => m_IsLegendary;

        [Constructable]
        public OsseinRam() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "an ossein ram";
            Body = 0x591;
            BaseSoundID = 0x99;

            SetStr(300, 400);
            SetDex(80, 100);
            SetInt(100, 120);

            SetHits(450, 550);

            SetDamage(18, 23);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 70.0, 80.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.DetectHidden, 40.0, 50.0);
            SetSkill(SkillName.Anatomy, 75.0, 85.0);
            SetSkill(SkillName.Necromancy, 20.0);
            SetSkill(SkillName.SpiritSpeak, 20.0);

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 72.0;

            SetMagicalAbility(MagicalAbility.BattleDefense);
            SetWeaponAbility(WeaponAbility.MortalStrike);
            SetSpecialAbility(SpecialAbility.LifeLeech);
        }

        public override int Meat { get { return 3; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override bool StatLossAfterTame { get { return true; } }

        public OsseinRam(Serial serial) : base(serial)
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
            this.Name = "a ossein ram";

            int[] legendaryHues = new int[] { 2076, 2706, 2955, 2075 };
            this.Hue = legendaryHues[Utility.Random(legendaryHues.Length)];

            this.ControlSlots = 1;
            this.MinTameSkill = 120.0;

            this.SetMagicalAbility(MagicalAbility.Necromage);
            this.SetSkill(SkillName.Necromancy, 50.6, 75.0);

            // Remove weapon abilities granted by base constructor and BattleDefense
            this.RemoveWeaponAbility(WeaponAbility.MortalStrike);
            this.RemoveMagicalAbility(MagicalAbility.BattleDefense);
            this.RemoveWeaponAbility(WeaponAbility.ParalyzingBlow);
            this.RemoveWeaponAbility(WeaponAbility.Disarm);

            this.SetWeaponAbility(WeaponAbility.DoubleStrike);
            this.SetSpecialAbility(SpecialAbility.LifeLeech);
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                if (!m_IsLegendary)
                    return null;

                return new TrainingDefinition(
                    typeof(OsseinRam),
                    Class.None,
                    MagicalAbility.Necromage | MagicalAbility.Necromancy | MagicalAbility.Piercing | MagicalAbility.Slashing | MagicalAbility.Bashing | MagicalAbility.WrestlingMastery,
                    new SpecialAbility[] { SpecialAbility.LifeLeech, SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds },
                    PetTrainingHelper.WepAbility12,
                    PetTrainingHelper.AreaEffectArea2,
                    1, 5);
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
