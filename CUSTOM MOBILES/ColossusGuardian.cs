/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 *
 * Hostile variant of the Rising Colossus for use in wave spawners.
 * Stats are fixed at the high end of the summon scaling range.
 */
using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a colossus guardian corpse")]
    public class ColossusGuardian : BaseCreature
    {
        [Constructable]
        public ColossusGuardian()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.5)
        {
            Name = "a colossus guardian";
            Body = 829;
            Hue = 1771; // Dark green

            // Stats fixed at high-end summon equivalent
            SetStr(750, 800);
            SetDex(110, 130);
            SetInt(125, 150);

            SetHits(450, 520);

            SetDamage(18, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 65, 70);
            SetResistance(ResistanceType.Fire,     50, 55);
            SetResistance(ResistanceType.Cold,     50, 55);
            SetResistance(ResistanceType.Poison,   100);
            SetResistance(ResistanceType.Energy,   65, 70);

            SetSkill(SkillName.MagicResist,  110.0, 120.0);
            SetSkill(SkillName.Tactics,      110.0, 120.0);
            SetSkill(SkillName.Wrestling,    110.0, 120.0);
            SetSkill(SkillName.Anatomy,      110.0, 120.0);
            SetSkill(SkillName.Mysticism,    110.0, 120.0);
            SetSkill(SkillName.Focus,        110.0, 120.0);
            SetSkill(SkillName.EvalInt,      110.0, 120.0);
            SetSkill(SkillName.DetectHidden,  70.0);

            VirtualArmor = 58;

            Fame  = 22500;
            Karma = -22500;

            SetWeaponAbility(WeaponAbility.ArmorIgnore);
            SetWeaponAbility(WeaponAbility.CrushingBlow);
        }

        public override bool BleedImmune   { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return false; } }

        public override int GetAttackSound() { return 0x627; }
        public override int GetHurtSound()   { return 0x629; }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 2);
        }

        public ColossusGuardian(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
