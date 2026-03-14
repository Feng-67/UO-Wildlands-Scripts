/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;

namespace Server.Mobiles
{
    [CorpseName("a phoenix corpse")]
    public class Phoenix : BaseCreature, IAuraCreature, ILegendaryPet
    {
        private bool m_IsLegendary; 
        public bool IsLegendary => m_IsLegendary;

        [Constructable]
        public Phoenix()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a phoenix";
            Body = 0x340;
            BaseSoundID = 0x8F;

            SetStr(504, 700);
            SetDex(202, 300);
            SetInt(504, 700);

            SetHits(340, 383);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.2, 100.0);
            SetSkill(SkillName.Magery, 90.2, 100.0);
            SetSkill(SkillName.Meditation, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 86.0, 135.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.DetectHidden, 70.0, 80.0);

            Fame = 15000;
            Karma = 0;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 4;
            MinTameSkill = 102.0;

            SetAreaEffect(AreaEffect.AuraDamage);
        }

        public Phoenix(Serial serial)
            : base(serial)
        {
        }

        public override void OnBeforeSpawn(Point3D location, Map map)
        {
            base.OnBeforeSpawn(location, map);

            // 10% chance a Legendary version spawns, equally split across 3 variants
            if (Utility.RandomDouble() < 0.15)
            {
                switch (Utility.Random(3))
                {
                    case 0: ConvertToLegendaryMagery(); break;
                    case 1: ConvertToLegendaryMysticism(); break;
                    case 2: ConvertToLegendarySpellweaving(); break;
                }
            }
        }

        public void ConvertToLegendaryMagery()
        {
            m_IsLegendary = true;
            this.Name = "a magery phoenix";
            this.Hue = 2753;

            this.AI = AIType.AI_Mage;
            this.ControlSlots = 3;
            this.MinTameSkill = 120.0;

            this.SetSkill(SkillName.EvalInt, 200.0, 250.0);
            this.SetSkill(SkillName.Magery, 100.0, 100.0);

            this.SetMagicalAbility(MagicalAbility.MageryMastery);
            this.SetAreaEffect(AreaEffect.AuraDamage);
            this.RemoveMagicalAbility(MagicalAbility.Magery);
        }

        public void ConvertToLegendaryMysticism()
        {
            m_IsLegendary = true;
            this.Name = "a mysticism phoenix";
            this.Hue = 1995;

            this.AI = AIType.AI_Mystic;
            this.ControlSlots = 3;
            this.MinTameSkill = 120.0;

            this.SetSkill(SkillName.Mysticism, 100.0, 100.0);
            this.SetSkill(SkillName.Focus, 150.0, 225.0);
            this.SetSkill(SkillName.Magery, 0, 0);
            this.SetSkill(SkillName.EvalInt, 0, 0);

            this.SetMagicalAbility(MagicalAbility.Mysticism);
            this.SetAreaEffect(AreaEffect.AuraDamage);
            this.RemoveMagicalAbility(MagicalAbility.Magery);
        }

        public void ConvertToLegendarySpellweaving()
        {
            m_IsLegendary = true;
            this.Name = "a spellweaving phoenix";
            this.Hue = 2953;

            this.AI = AIType.AI_Spellweaving;
            this.ControlSlots = 3;
            this.MinTameSkill = 120.0;

            this.SetSkill(SkillName.Spellweaving, 225.0, 225.0);
            this.SetSkill(SkillName.Magery, 0, 0);
            this.SetSkill(SkillName.EvalInt, 0, 0);

            this.SetMagicalAbility(MagicalAbility.Spellweaving);
            this.SetAreaEffect(AreaEffect.AuraDamage);
            this.RemoveMagicalAbility(MagicalAbility.Magery);
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override int Feathers { get { return 36; } }
        public override bool CanFly { get { return true; } }

        public void AuraEffect(Mobile m)
        {
            m.SendLocalizedMessage(1008112); // The intense heat is damaging you!
        }

        public override void OnAfterTame(Mobile tamer)
        {
            base.OnAfterTame(tamer);

            var profile = PetTrainingHelper.GetAbilityProfile(this);

            if (profile != null)
            {
                profile.RemoveAbility(AreaEffect.AuraDamage);
            }
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                if (!m_IsLegendary)
                    return null; // falls through to PetTrainingHelper.Definitions

                if (AI == AIType.AI_Mystic)
                {
                    return new TrainingDefinition(
                        typeof(Phoenix),
                        Class.MagicalAndClawed,
                        MagicalAbility.Poisoning | MagicalAbility.Discordance | MagicalAbility.Chivalry,
                        PetTrainingHelper.SpecialAbilityPhoenix,
                        PetTrainingHelper.WepAbility1,
                        PetTrainingHelper.AreaEffectArea2,
                        3, 5);
                }

                if (AI == AIType.AI_Spellweaving)
                {
                    return new TrainingDefinition(
                        typeof(Phoenix),
                        Class.MagicalAndClawed,
                        MagicalAbility.Poisoning | MagicalAbility.Discordance | MagicalAbility.Chivalry,
                        PetTrainingHelper.SpecialAbilityPhoenix,
                        PetTrainingHelper.WepAbility1,
                        PetTrainingHelper.AreaEffectArea2,
                        3, 5);
                }

                // AI_Mage (Legendary Magery)
                return new TrainingDefinition(
                    typeof(Phoenix),
                    Class.MagicalAndClawed,
                    MagicalAbility.Poisoning | MagicalAbility.Discordance | MagicalAbility.Chivalry,
                    PetTrainingHelper.SpecialAbilityPhoenix,
                    PetTrainingHelper.WepAbility1,
                    PetTrainingHelper.AreaEffectArea2,
                    3, 5);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
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
