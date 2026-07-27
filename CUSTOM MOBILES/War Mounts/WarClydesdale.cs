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
    [CorpseName("a clydesdale corpse")]
    public class WarClydesdale : BaseMount, IBardedMount
    {
        private bool m_BardingExceptional;
        private Mobile m_BardingCrafter;
        private int m_BardingHP;
        private bool m_HasBarding;
        private CraftResource m_BardingResource;

        [Constructable]
        public WarClydesdale()
            : this("a war clydesdale")
        {
        }

        [Constructable]
        public WarClydesdale(string name)
            : base(name, 1651, 16094, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xA8;

            SetStr(201, 300);
            SetDex(66, 85);
            SetInt(61, 100);

            SetHits(121, 180);

            SetDamage(3, 4);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 2000;
            Karma = -2000;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 93.9;
        }

        public WarClydesdale(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BardingCrafter
        {
            get { return m_BardingCrafter; }
            set { m_BardingCrafter = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BardingExceptional
        {
            get { return m_BardingExceptional; }
            set { m_BardingExceptional = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BardingHP
        {
            get { return m_BardingHP; }
            set { m_BardingHP = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasBarding
        {
            get { return m_HasBarding; }
            set
            {
                m_HasBarding = value;

                // No dedicated barded-clydesdale body art exists, so barding
                // is expressed purely as a hue change (unlike SwampDragon,
                // which also swaps BodyValue/ItemID). This intentionally
                // replaces ClydesdaleHorse's random rarity-hue roll.
                Hue = m_HasBarding ? CraftResources.GetHue(m_BardingResource) : 0;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource BardingResource
        {
            get { return m_BardingResource; }
            set
            {
                m_BardingResource = value;

                if (m_HasBarding)
                    Hue = CraftResources.GetHue(value);

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BardingMaxHP
        {
            get
            {
                switch (m_BardingResource)
                {
                    default:
                        return BardingExceptional ? 12000 : 10000;
                    case CraftResource.DullCopper:
                    case CraftResource.Valorite:
                        return BardingExceptional ? 14500 : 12500;
                    case CraftResource.ShadowIron:
                        return BardingExceptional ? 17000 : 15000;
                }
            }
        }

        private int CalculateBardingResistance(ResistanceType type)
        {
            if (m_BardingResource == CraftResource.None || !m_HasBarding)
                return 0;

            CraftResourceInfo resInfo = CraftResources.GetInfo(m_BardingResource);

            if (resInfo == null)
                return 0;

            CraftAttributeInfo attrs = resInfo.AttributeInfo;

            if (attrs == null)
                return 0;

            int expBonus = BardingExceptional ? 1 : 0;
            int resBonus = 0;

            switch (type)
            {
                default:
                case ResistanceType.Physical: resBonus = Math.Max(5, attrs.ArmorPhysicalResist); break;
                case ResistanceType.Fire: resBonus = Math.Max(3, attrs.ArmorFireResist); break;
                case ResistanceType.Cold: resBonus = Math.Max(2, attrs.ArmorColdResist); break;
                case ResistanceType.Poison: resBonus = Math.Max(3, attrs.ArmorPoisonResist); break;
                case ResistanceType.Energy: resBonus = Math.Max(2, attrs.ArmorEnergyResist); break;
            }

            return (resBonus + expBonus) * 5;
        }

        public override int GetResistance(ResistanceType type)
        {
            return base.GetResistance(type) + CalculateBardingResistance(type);
        }

        public override double GetControlChance(Mobile m, bool useBaseSkill)
        {
            if (PetTrainingHelper.Enabled)
            {
                var profile = PetTrainingHelper.GetAbilityProfile(this);

                if (profile != null && profile.HasCustomized())
                {
                    return base.GetControlChance(m, useBaseSkill);
                }
            }

            return 1.0;
        }

        public override bool ReacquireOnMovement { get { return true; } }

        public override FoodType FavoriteFood
        {
            get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; }
        }

        public override int Meat { get { return 9; } }

        public override int Hides { get { return 10; } }

        public override bool CanAngerOnTame { get { return true; } }

        public override bool OverrideBondingReqs()
        {
            return true;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_HasBarding && m_BardingExceptional && m_BardingCrafter != null)
                list.Add(1060853, m_BardingCrafter.Name); // armor exceptionally crafted by ~1_val~

            if (m_HasBarding)
                list.Add(1115719, m_BardingHP.ToString()); // armor points: ~1_val~
        }

        public override void OnRiderDamaged(Mobile from, ref int amount, bool willKill)
        {
            base.OnRiderDamaged(from, ref amount, willKill);

            if (Rider == null)
                return;

            if ((from == null || !from.Player) && Rider.Player && Rider.Mount == this)
            {
                if (HasBarding)
                {
                    int percent = (BardingExceptional ? 20 : 10);
                    int absorbed = AOS.Scale(amount, percent);

                    amount -= absorbed;
                    BardingHP -= absorbed;

                    if (BardingHP < 0)
                    {
                        HasBarding = false;
                        BardingHP = 0;

                        Rider.SendLocalizedMessage(1053031); // Your dragon's barding has been destroyed!
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((bool)m_BardingExceptional);
            writer.Write((Mobile)m_BardingCrafter);
            writer.Write((bool)m_HasBarding);
            writer.Write((int)m_BardingHP);
            writer.Write((int)m_BardingResource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_BardingExceptional = reader.ReadBool();
            m_BardingCrafter = reader.ReadMobile();
            m_HasBarding = reader.ReadBool();
            m_BardingHP = reader.ReadInt();
            m_BardingResource = (CraftResource)reader.ReadInt();

            if (BaseSoundID == -1)
                BaseSoundID = 0xA8;
        }
    }
}
