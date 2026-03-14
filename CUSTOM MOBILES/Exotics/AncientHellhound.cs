/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Dragonslayer2)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */

using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ancient hellhound corpse")]
    public class AncientHellhound : BaseMount
    {
        public override double HealChance { get { return 1.0; } }

        [Constructable]
        public AncientHellhound()
            : this("Ancient Hellhound")
        {
        }

        [Constructable]
        public AncientHellhound(string name)
            : base(name, 0x42D, 0x3EC9, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xE5; // Grey Wolf sounds

            SetStr(1200, 1300);
            SetDex(284, 384);
            SetInt(226, 250);

            SetHits(1200, 1250);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 25, 45);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Wrestling, 90.1, 105.8);
            SetSkill(SkillName.Tactics, 89.3, 98.3);
            SetSkill(SkillName.MagicResist, 59.3, 69.0);
            SetSkill(SkillName.Anatomy, 55.5, 70.4);
            SetSkill(SkillName.Healing, 72.2, 98.9);

            Fame = 24000;
            Karma = -24000;

            Tamable = true;
            ControlSlots = 3;
            ControlSlotsMax = 5;
            MinTameSkill = 108;

            SetWeaponAbility(WeaponAbility.MortalStrike);
        }

        public AncientHellhound(Serial serial)
            : base(serial)
        {
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override bool StatLossAfterTame { get { return true; } }
        public override int Hides { get { return 20; } }
        public override int Meat { get { return 16; } }
        public override HideType HideType { get { return HideType.Horned; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("<BASEFONT COLOR=#FFD700>Exotic</BASEFONT>");
        }
                
        public override void OnAfterTame(Mobile tamer)
        {
            if (Owners.Count == 0 && PetTrainingHelper.Enabled)
            {
                if (RawStr > 0)
                    RawStr = (int)Math.Max(1, RawStr * 0.5);

                if (RawDex > 0)
                    RawDex = (int)Math.Max(1, RawDex * 0.5);

                if (HitsMaxSeed > 0)
                    HitsMaxSeed = (int)Math.Max(1, HitsMaxSeed * 0.5);

                Hits = Math.Min(HitsMaxSeed, Hits);
                Stam = Math.Min(RawDex, Stam);
            }
            else
            {
                base.OnAfterTame(tamer);
            }
        }

        public override int GetIdleSound() { return 0xE5; }
        public override int GetAttackSound() { return 0xE5; }
        public override int GetAngerSound() { return 0xE5; }
        public override int GetHurtSound() { return 0xE5; }
        public override int GetDeathSound() { return 0xE5; }

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
