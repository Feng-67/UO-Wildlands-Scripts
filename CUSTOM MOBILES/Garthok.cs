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
    public class Garthok : BaseChampion
    {
        public override Type[] UniqueList { get { return new Type[] { }; } }
        public override Type[] SharedList { get { return new Type[] { }; } }
        public override Type[] DecorativeList { get { return new Type[] { typeof(GarthoksLunch), typeof(SkullOfGarthok) }; } }

        public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { MonsterStatuetteType.Orc }; } }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Pain; } }

        [Constructable]
        public Garthok() : base(AIType.AI_Melee)
        {
            Name = "Garthok the Champion";
            Body = 1575;
            BaseSoundID = 0x45A;

            SetStr(1200);
            SetDex(250);
            SetInt(600);
            SetHits(30000);
            SetDamage(28, 35);

            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);

            Fame = 25000;
            Karma = -25000;
        }

        public override TribeType Tribe { get { return TribeType.Orc; } }

        private DateTime m_NextWhip;
        public override void OnActionCombat()
        {
            if (Combatant != null && DateTime.UtcNow > m_NextWhip)
            {
                DoWhipAttack();
                m_NextWhip = DateTime.UtcNow + TimeSpan.FromSeconds(15.0);
            }
            base.OnActionCombat();
        }

        public void DoWhipAttack()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B, false, "DIE, WEAKLINGS!");
            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != null && m != this && m.Alive && CanBeHarmful(m))
                {
                    this.DoHarmful(m);
                    m.Damage(Utility.RandomMinMax(30, 45), this);
                    m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 8);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            Type[] backpackRewards = new Type[]
            {
                typeof(GarthoksMotivator),
                typeof(GarthoksToothpick),
                typeof(OrcSkinBelt),
                typeof(OrcChampionVisage),
                typeof(OrcHoundStatue),
                typeof(BraveKnightOfTheBritannia)
            };

            // FIX: Changed DamageEntry to DamageStore and m_Mobile to Mobile
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
                        
        

        public Garthok(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
