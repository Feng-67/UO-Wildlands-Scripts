/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team] (Original Author Rutibex)
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an orcish corpse")]
    public class OrcBruteBoss : BaseCreature
    {
        [Constructable]
        public OrcBruteBoss()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 189;

            Name = "Orc Brute Boss";
            BaseSoundID = 0x45A;

            SetStr(767, 945);
            SetDex(150, 160);
            SetInt(46, 70);

            SetHits(1800, 2000);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 45, 55);
            SetResistance(ResistanceType.Energy, 45, 55);

            SetSkill(SkillName.Macing, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 125.1, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;

            Item ore = new ShadowIronOre(25);
            ore.ItemID = 0x19B9;
            PackItem(ore);
            PackItem(new IronIngot(10));

            if (0.05 > Utility.RandomDouble())
                PackItem(new OrcishKinMask());

            if (0.2 > Utility.RandomDouble())
                PackItem(new BolaBall());

            PackItem(new Yeast());
        }

        public OrcBruteBoss(Serial serial)
            : base(serial)
        {
        }

        public override bool BardImmune
        {
            get
            {
                return !Core.AOS;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override int Meat
        {
            get
            {
                return 2;
            }
        }

        public override TribeType Tribe { get { return TribeType.Orc; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.SavagesAndOrcs;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override bool AutoDispel
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            Item item = aggressor.FindItemOnLayer(Layer.Helm);

            if (item is OrcishKinMask)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }

        public override int Damage(int amount, Mobile from, bool informMount, bool checkDisrupt)
        {
            var damage = base.Damage(amount, from, informMount, checkDisrupt);

            if (from != null && from != this && !Controlled && !Summoned && Utility.RandomDouble() <= 0.2)
            {
                SpawnOrcLord(from);
            }

            return damage;
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
                       
            // Add 3 mixed Peerless Ingredients
            for (int i = 0; i < 3; i++)
            {
                switch (Utility.Random(6))
                {
                    case 0: c.DropItem(new Corruption()); break;
                    case 1: c.DropItem(new Taint()); break;
                    case 2: c.DropItem(new Blight()); break;
                    case 3: c.DropItem(new Putrefaction()); break;
                    case 4: c.DropItem(new Muculent()); break;
                    case 5: c.DropItem(new Scourge()); break;
                }
            }

            if (Paragon.ChestChance > Utility.RandomDouble())
                c.DropItem(new ParagonChest(Name, 5));
        }

        public void SpawnOrcLord(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int orcs = 0;
            IPooledEnumerable eable = GetMobilesInRange(10);

            foreach (Mobile m in eable)
            {
                if (m is OrcishLord)
                    ++orcs;
            }

            eable.Free();

            if (orcs < 10)
            {
                BaseCreature orc = new SpawnedOrcishLord();

                orc.Team = Team;

                Point3D loc = target.Location;
                bool validLocation = false;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = target.X + Utility.Random(3) - 1;
                    int y = target.Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                orc.MoveToWorld(loc, map);

                orc.Combatant = target;
            }
        }

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
