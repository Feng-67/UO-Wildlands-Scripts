/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a juvenile umbrascale corpse")]
    public class JuvenileUmbrascale : BaseMount
    {
        [Constructable]
        public JuvenileUmbrascale() : this("juvenile umbrascale")
        {
        }

        [Constructable]
        public JuvenileUmbrascale(string name) : base(name, 1409, 16093, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // BodyValue: 1409 | MountID: 16093 — preserved from original

            BaseSoundID = 0x16A;

            // --- Taming ---
            Tamable = true;
            ControlSlots = 3;       
            MinTameSkill = 108.0;

            // --- Attributes (fixed values — zero spread per bestiary) ---
            SetStr(760);
            SetDex(165);
            SetInt(300);

            SetHits(450);
            SetStam(150);
            SetMana(300);

            // --- Damage Profile ---
            SetDamage(18, 24);
            SetDamageType(ResistanceType.Physical,  0);
            SetDamageType(ResistanceType.Fire,      50);
            SetDamageType(ResistanceType.Energy,    50);

            // --- Resistances (fixed values — zero spread per bestiary) ---
            SetResistance(ResistanceType.Physical, 60);
            SetResistance(ResistanceType.Fire,     60);
            SetResistance(ResistanceType.Cold,     50);
            SetResistance(ResistanceType.Poison,   50);
            SetResistance(ResistanceType.Energy,   50);

            // --- Skills ---
            // Wrestling is overcapped: tamed range 110.0–130.0, pre-tame cap 130.0
            SetSkill(SkillName.Wrestling,   110.0, 130.0);
            SetSkill(SkillName.Tactics,     100.1, 115.0);
            SetSkill(SkillName.MagicResist,  80.0, 100.0);

            // Healing is listed as an innate ability; Anatomy is required for it to function
            SetSkill(SkillName.Healing,  80.1, 100.0);
            SetSkill(SkillName.Anatomy,  80.1, 100.0);
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();

            // Enforce the overcapped Wrestling cap post-spawn so it applies correctly
            Skills[SkillName.Wrestling].Cap = 130.0;
        }
                
        public override bool CanAngerOnTame { get { return true; } }
        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public JuvenileUmbrascale(Serial serial) : base(serial)
        {
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
