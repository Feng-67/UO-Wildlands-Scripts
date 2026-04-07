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
using System.Collections.Generic;
using Server.Items;
using Server.Engines.CannedEvil;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the remains of a void gazer")]
    public class Xorvintaal : BaseChampion
    {
        public override Type[] UniqueList { get { return new Type[] { }; } }
        public override Type[] SharedList { get { return new Type[] { }; } }
        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(VoidEssence), typeof(VoidEssenceVials) }; }
        }
        public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { MonsterStatuetteType.Gazer }; } }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Venom; } } // Green Skull

        [Constructable]
        public Xorvintaal() : base(AIType.AI_Mage)
        {
            Name = "Xorvintaal the Void Gazer";
            Body = 1642; // UOholder_Magic
            BaseSoundID = 377;

            // Magic focused stats
            SetStr(500, 600);
            SetDex(100, 120);
            SetInt(1000, 1200);
            SetHits(30000);
            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 50;
        }

        private DateTime m_NextEyeRay;
        public override void OnActionCombat()
        {
            // Fires slightly faster than the melee version (every 10-14 seconds)
            if (Combatant != null && DateTime.UtcNow > m_NextEyeRay)
            {
                DoEyeRayAttack();
                m_NextEyeRay = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            }
            base.OnActionCombat();
        }

        public void DoEyeRayAttack()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x44, false, "GAZE INTO THE ABYSS!");
            foreach (Mobile m in this.GetMobilesInRange(6)) // Slightly larger range
            {
                if (m != null && m != this && m.Player && CanBeHarmful(m))
                {
                    this.DoHarmful(m);

                    int effect = Utility.Random(3);
                    if (effect == 0) m.Damage(45, this); // Slightly more magic damage
                    else if (effect == 1) m.Freeze(TimeSpan.FromSeconds(2));
                    else m.Mana -= 60; // Drains more mana

                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Pool of Void Rewards
            Type[] backpackRewards = new Type[]
            {
        typeof(RiftwalkersPendant),
        typeof(VoidScarredCharm),
        typeof(VoidShieldOfInvulnerability),
        typeof(VoidQuiver)
            };

            // DamageStore logic to match your server core
            List<DamageStore> rights = GetLootingRights();

            for (int i = 0; i < rights.Count && i < 3; ++i)
            {
                Mobile m = rights[i].m_Mobile;

                // This line adds a 20% chance (0.2). 
                // Only if the random roll is successful will the item be created.
                if (m != null && m.Player && m.InRange(this.Location, 20) && Utility.RandomDouble() < 0.20)
                {
                    Type type = backpackRewards[Utility.Random(backpackRewards.Length)];
                    Item reward = Activator.CreateInstance(type) as Item;

                    if (reward != null)
                    {
                        m.AddToBackpack(reward);
                        m.SendMessage(0x35, "You have been rewarded for your bravery!");
                    }
                }
            }
        }

        public Xorvintaal(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
