/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Ravenwolfe)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 *
 * Consolidated file — contains:
 *   Enums                    (was Invasion Settings.cs)
 *   MonsterTownSpawnEntry    (was MonsterTownSpawnEntry.cs)
 *   TownInvasion             (was Invasion System.cs)
 *   InvasionControl          (was InvasionControl.cs)
 *   InvasionPersistence      (was InvasionControl.cs)
 *
 * Changes from original:
 *   - Removed _AlwaysMurderer dead field
 *   - WasDisabledRegion and _FinalStage now serialized (survive server restart)
 *   - 1-hour cooldown per town tracked in InvasionControl
 *   - Town crier per-minute announcement replaced by centralized 5-minute
 *     red system broadcast that groups multiple active invasions into one message
 *   - Champion arrival broadcast added in SpawnChamp
 *   - [Invasions command only (replaces [Invasion and [ListInvasions)
 *   - Double RefreshAllOpenGumps call on start resolved
 */
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Commands;
using Server.Spells.Ninjitsu;

namespace Server.Customs.Invasion_System
{
    // -------------------------------------------------------------------------
    // ENUMS  (was Invasion Settings.cs)
    // -------------------------------------------------------------------------

    public enum TownMonsterType
    {
        Abyss,
        Arachnid,
        DragonKind,
        Elementals,
        Humanoid,
        OrcsandRatmen,
        OreElementals,
        Ophidian,
        Snakes,
        Undead
    }

    public enum TownChampionType
    {
        Barracoon,
        Harrower,
        LordOaks,
        Mephitis,
        Neira,
        Rikktor,
        Semidar,
        Serado
    }

    public enum InvasionTowns
    {
        BuccaneersDen,
        Cove,
        Delucia,
        Jhelom,
        Minoc,
        Moonglow,
        Nujel,
        Ocllo,
        Papua,
        SkaraBrae,
        Vesper,
        Yew
    }

    // -------------------------------------------------------------------------
    // SPAWN ENTRIES  (was MonsterTownSpawnEntry.cs)
    // -------------------------------------------------------------------------

    public class MonsterTownSpawnEntry
    {
        #region MonsterSpawnEntries

        public static MonsterTownSpawnEntry[] Undead = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( Zombie ),          165 ),
            new MonsterTownSpawnEntry( typeof( Skeleton ),         65 ),
            new MonsterTownSpawnEntry( typeof( SkeletalMage ),     40 ),
            new MonsterTownSpawnEntry( typeof( BoneKnight ),       45 ),
            new MonsterTownSpawnEntry( typeof( SkeletalKnight ),   45 ),
            new MonsterTownSpawnEntry( typeof( Lich ),             45 ),
            new MonsterTownSpawnEntry( typeof( Ghoul ),            40 ),
            new MonsterTownSpawnEntry( typeof( BoneMagi ),         40 ),
            new MonsterTownSpawnEntry( typeof( Wraith ),           35 ),
            new MonsterTownSpawnEntry( typeof( RottingCorpse ),    35 ),
            new MonsterTownSpawnEntry( typeof( LichLord ),         55 ),
            new MonsterTownSpawnEntry( typeof( Spectre ),          30 ),
            new MonsterTownSpawnEntry( typeof( Shade ),            30 ),
            new MonsterTownSpawnEntry( typeof( AncientLich ),      50 )
        };

        public static MonsterTownSpawnEntry[] Humanoid = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( Brigand ),          60 ),
            new MonsterTownSpawnEntry( typeof( Executioner ),      30 ),
            new MonsterTownSpawnEntry( typeof( EvilMage ),         70 ),
            new MonsterTownSpawnEntry( typeof( EvilMageLord ),     40 ),
            new MonsterTownSpawnEntry( typeof( Ettin ),            45 ),
            new MonsterTownSpawnEntry( typeof( Ogre ),             45 ),
            new MonsterTownSpawnEntry( typeof( OgreLord ),         40 ),
            new MonsterTownSpawnEntry( typeof( ArcticOgreLord ),   40 ),
            new MonsterTownSpawnEntry( typeof( Troll ),            55 ),
            new MonsterTownSpawnEntry( typeof( Cyclops ),          55 ),
            new MonsterTownSpawnEntry( typeof( Titan ),            40 )
        };

        public static MonsterTownSpawnEntry[] OrcsandRatmen = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( Orc ),              80 ),
            new MonsterTownSpawnEntry( typeof( OrcishMage ),       45 ),
            new MonsterTownSpawnEntry( typeof( OrcishLord ),       55 ),
            new MonsterTownSpawnEntry( typeof( OrcCaptain ),       50 ),
            new MonsterTownSpawnEntry( typeof( OrcBomber ),        55 ),
            new MonsterTownSpawnEntry( typeof( OrcBrute ),         40 ),
            new MonsterTownSpawnEntry( typeof( Ratman ),           80 ),
            new MonsterTownSpawnEntry( typeof( RatmanArcher ),     50 ),
            new MonsterTownSpawnEntry( typeof( RatmanMage ),       45 )
        };

        public static MonsterTownSpawnEntry[] Elementals = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( EarthElemental ),   95 ),
            new MonsterTownSpawnEntry( typeof( AirElemental ),     70 ),
            new MonsterTownSpawnEntry( typeof( FireElemental ),    60 ),
            new MonsterTownSpawnEntry( typeof( WaterElemental ),   60 ),
            new MonsterTownSpawnEntry( typeof( SnowElemental ),    40 ),
            new MonsterTownSpawnEntry( typeof( IceElemental ),     40 ),
            new MonsterTownSpawnEntry( typeof( Efreet ),           45 ),
            new MonsterTownSpawnEntry( typeof( PoisonElemental ),  35 ),
            new MonsterTownSpawnEntry( typeof( BloodElemental ),   35 )
        };

        public static MonsterTownSpawnEntry[] OreElementals = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( DullCopperElemental ),  90 ),
            new MonsterTownSpawnEntry( typeof( CopperElemental ),      80 ),
            new MonsterTownSpawnEntry( typeof( BronzeElemental ),      50 ),
            new MonsterTownSpawnEntry( typeof( ShadowIronElemental ),  60 ),
            new MonsterTownSpawnEntry( typeof( GoldenElemental ),      55 ),
            new MonsterTownSpawnEntry( typeof( AgapiteElemental ),     45 ),
            new MonsterTownSpawnEntry( typeof( VeriteElemental ),      40 ),
            new MonsterTownSpawnEntry( typeof( ValoriteElemental ),    40 )
        };

        public static MonsterTownSpawnEntry[] Ophidian = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( OphidianWarrior ),     100 ),
            new MonsterTownSpawnEntry( typeof( OphidianMage ),         70 ),
            new MonsterTownSpawnEntry( typeof( OphidianArchmage ),     30 ),
            new MonsterTownSpawnEntry( typeof( OphidianKnight ),       35 ),
            new MonsterTownSpawnEntry( typeof( OphidianMatriarch ),    35 )
        };

        public static MonsterTownSpawnEntry[] Arachnid = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( Scorpion ),         75 ),
            new MonsterTownSpawnEntry( typeof( GiantSpider ),      75 ),
            new MonsterTownSpawnEntry( typeof( TerathanDrone ),    45 ),
            new MonsterTownSpawnEntry( typeof( TerathanWarrior ),  30 ),
            new MonsterTownSpawnEntry( typeof( TerathanMatriarch ),45 ),
            new MonsterTownSpawnEntry( typeof( TerathanAvenger ),  45 ),
            new MonsterTownSpawnEntry( typeof( DreadSpider ),      40 ),
            new MonsterTownSpawnEntry( typeof( FrostSpider ),      35 )
        };

        public static MonsterTownSpawnEntry[] Snakes = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( Snake ),            95 ),
            new MonsterTownSpawnEntry( typeof( GiantSerpent ),     95 ),
            new MonsterTownSpawnEntry( typeof( LavaSnake ),        50 ),
            new MonsterTownSpawnEntry( typeof( LavaSerpent ),      55 ),
            new MonsterTownSpawnEntry( typeof( IceSnake ),         50 ),
            new MonsterTownSpawnEntry( typeof( IceSerpent ),       55 ),
            new MonsterTownSpawnEntry( typeof( SilverSerpent ),    40 )
        };

        public static MonsterTownSpawnEntry[] Abyss = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( Gargoyle ),         100 ),
            new MonsterTownSpawnEntry( typeof( StoneGargoyle ),     60 ),
            new MonsterTownSpawnEntry( typeof( FireGargoyle ),      60 ),
            new MonsterTownSpawnEntry( typeof( Daemon ),            60 ),
            new MonsterTownSpawnEntry( typeof( IceFiend ),          50 ),
            new MonsterTownSpawnEntry( typeof( Balron ),            30 )
        };

        public static MonsterTownSpawnEntry[] DragonKind = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry( typeof( Wyvern ),           100 ),
            new MonsterTownSpawnEntry( typeof( Drake ),             60 ),
            new MonsterTownSpawnEntry( typeof( Dragon ),            60 ),
            new MonsterTownSpawnEntry( typeof( WhiteWyrm ),         60 ),
            new MonsterTownSpawnEntry( typeof( ShadowWyrm ),        10 ),
            new MonsterTownSpawnEntry( typeof( AncientWyrm ),       30 )
        };

        #endregion

        private Type m_Monster;
        private int m_Amount;

        public Type Monster { get { return m_Monster; } set { m_Monster = value; } }
        public int Amount   { get { return m_Amount; }  set { m_Amount = value; } }

        public MonsterTownSpawnEntry( Type monster, int amount )
        {
            m_Monster = monster;
            m_Amount  = amount;
        }
    }

    // -------------------------------------------------------------------------
    // TOWN INVASION  (was Invasion System.cs)
    // -------------------------------------------------------------------------

    public class TownInvasion
    {
        public static void Initialize()
        {
            Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(30.0), GlobalSync);
        }

        #region Private Variables

        private int  _MinSpawnZ;
        private int  _MaxSpawnZ;
        private bool _FinalStage;

        private Point3D _Top    = new Point3D(4394, 1058, 30);
        private Point3D _Bottom = new Point3D(4481, 1173, 0);
        private Map     _SpawnMap = Map.Felucca;

        private List<Mobile> _Spawned;

        private TownMonsterType  _TownMonsterType  = TownMonsterType.OrcsandRatmen;
        private TownChampionType _TownChampionType = TownChampionType.Barracoon;
        private InvasionTowns    _InvasionTown     = InvasionTowns.BuccaneersDen;
        private DateTime         _StartTime;

        private string _TownInvaded = "Moonglow";
        private Timer  _SpawnTimer;

        private bool WasDisabledRegion;
        private bool Active;

        #endregion

        #region Public Variables

        public int           MinSpawnZ        { get { return _MinSpawnZ; }        set { _MinSpawnZ = value; } }
        public int           MaxSpawnZ        { get { return _MaxSpawnZ; }        set { _MaxSpawnZ = value; } }
        public Point3D       Top              { get { return _Top; }              set { _Top = value; } }
        public Point3D       Bottom           { get { return _Bottom; }           set { _Bottom = value; } }
        public Map           SpawnMap         { get { return _SpawnMap; }         set { _SpawnMap = value; } }
        public List<Mobile>  Spawned          { get { return _Spawned; }          set { _Spawned = value; } }
        public string        TownInvaded      { get { return _TownInvaded; }      set { _TownInvaded = value; } }
        public Timer         SpawnTimer       { get { return _SpawnTimer; }       set { _SpawnTimer = value; } }
        public bool          IsFinalStage     { get { return _FinalStage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TownMonsterType  TownMonsterType  { get { return _TownMonsterType;  } set { _TownMonsterType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TownChampionType TownChampionType { get { return _TownChampionType; } set { _TownChampionType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public InvasionTowns    InvasionTown     { get { return _InvasionTown;     } set { _InvasionTown = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime         StartTime        { get { return _StartTime;        } set { _StartTime = value; } }

        public bool IsRunning { get { return _SpawnTimer != null && _SpawnTimer.Running; } }

        #endregion

        public static string GetTownName(InvasionTowns town)
        {
            switch (town)
            {
                case InvasionTowns.BuccaneersDen: return "Buccaneer's Den";
                case InvasionTowns.Cove: return "Cove";
                case InvasionTowns.Delucia: return "Delucia";
                case InvasionTowns.Jhelom: return "Jhelom";
                case InvasionTowns.Minoc: return "Minoc";
                case InvasionTowns.Moonglow: return "Moonglow";
                case InvasionTowns.Nujel: return "Nujel'm";
                case InvasionTowns.Ocllo: return "Ocllo";
                case InvasionTowns.Papua: return "Papua";
                case InvasionTowns.SkaraBrae: return "Skara Brae";
                case InvasionTowns.Vesper: return "Vesper";
                case InvasionTowns.Yew: return "Yew";
                default: return town.ToString();
            }
        }

        #region Constructor

        public TownInvasion(InvasionTowns town, TownMonsterType monster, TownChampionType champion, DateTime time)
        {
            _Spawned = new List<Mobile>();
            _InvasionTown = town;
            _TownMonsterType = monster;
            _TownChampionType = champion;
            _StartTime = time;
            _TownInvaded = GetTownName(town);

            InvasionControl.Invasions.Add(this);
        }

        public TownInvasion(GenericReader reader)
        {
            Deserialize(reader);
        }

        #endregion

        public void OnStart()
        {
            if (!IsRunning)
            {
                InvasionTowns invading = InvasionTown;

                switch (invading)
                {
                    case InvasionTowns.BuccaneersDen:
                        Top = new Point3D(2608, 2060, 0); Bottom = new Point3D(2824, 2296, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Buccaneer's Den";
                        break;
                    case InvasionTowns.Cove:
                        Top = new Point3D(2213, 1148, 0); Bottom = new Point3D(2284, 1233, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Cove";
                        break;
                    case InvasionTowns.Delucia:
                        Top = new Point3D(5171, 3980, 41); Bottom = new Point3D(5300, 4040, 39);
                        MinSpawnZ = 29; MaxSpawnZ = 32; SpawnMap = Map.Felucca;
                        TownInvaded = "Delucia";
                        break;
                    case InvasionTowns.Jhelom:
                        Top = new Point3D(1304, 3682, 0); Bottom = new Point3D(1465, 3877, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Jhelom";
                        break;
                    case InvasionTowns.Minoc:
                        Top = new Point3D(2443, 420, 15); Bottom = new Point3D(2520, 539, 0);
                        MinSpawnZ = 10; MaxSpawnZ = 16; SpawnMap = Map.Felucca;
                        TownInvaded = "Minoc";
                        break;
                    case InvasionTowns.Moonglow:
                        Top = new Point3D(4394, 1058, 30); Bottom = new Point3D(4481, 1173, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Moonglow";
                        break;
                    case InvasionTowns.Nujel:
                        Top = new Point3D(3665, 1189, 0); Bottom = new Point3D(3774, 1357, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Nujel'm";
                        break;
                    case InvasionTowns.Ocllo:
                        Top = new Point3D(3617, 2482, 0); Bottom = new Point3D(3712, 2630, 20);
                        MinSpawnZ = 5; MaxSpawnZ = 21; SpawnMap = Map.Felucca;
                        TownInvaded = "Ocllo";
                        break;
                    case InvasionTowns.Papua:
                        Top = new Point3D(5644, 3112, -15); Bottom = new Point3D(5826, 3315, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Papua";
                        break;
                    case InvasionTowns.SkaraBrae:
                        Top = new Point3D(577, 2131, -90); Bottom = new Point3D(634, 2234, -90);
                        MinSpawnZ = 25; MaxSpawnZ = 65; SpawnMap = Map.Felucca;
                        TownInvaded = "Skara Brae";
                        break;
                    case InvasionTowns.Yew:
                        Top = new Point3D(452, 928, 0); Bottom = new Point3D(669, 1104, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Yew";
                        break;
                    case InvasionTowns.Vesper:
                        Top = new Point3D(2835, 656, 0); Bottom = new Point3D(2940, 988, 0);
                        MinSpawnZ = 50; MaxSpawnZ = 61; SpawnMap = Map.Felucca;
                        TownInvaded = "Vesper";
                        break;
                }

                string broadcastMsg = String.Format(
                    "THE CITY OF {0} FELUCCA IS UNDER SIEGE BY {1}! TO ARMS, BRAVE SOULS - DEFEND THE REALM!",
                    TownInvaded, TownMonsterType);
                World.Broadcast(0x22, true, broadcastMsg);

                foreach (Region r in Region.Regions)
                {
                    if (r is GuardedRegion && r.Name == TownInvaded)
                    {
                        WasDisabledRegion = ((GuardedRegion)r).Disabled;
                        ((GuardedRegion)r).Disabled = true;
                    }
                }

                Spawn();
            }
        }

        public void OnStop()
        {
            Despawn();

            if (!WasDisabledRegion)
            {
                foreach (Region r in Region.Regions)
                {
                    if (r is GuardedRegion && r.Name == TownInvaded)
                        ((GuardedRegion)r).Disabled = false;
                }
            }

            if (SpawnTimer != null)
                _SpawnTimer.Stop();

            // Only apply cooldown if the invasion was actually running, not just queued
            if (Active)
                InvasionControl.TownCooldowns[_InvasionTown] = DateTime.UtcNow + TimeSpan.FromHours(1);

            InvasionControl.Invasions.Remove(this);
            InvasionControl.RefreshAllOpenGumps();
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(1); // version
            writer.Write((int)InvasionTown);
            writer.Write((int)TownMonsterType);
            writer.Write((int)TownChampionType);
            writer.Write(StartTime);
            writer.Write(Spawned);

            Active = IsRunning;
            writer.Write(Active);

            // version 1
            writer.Write(WasDisabledRegion);
            writer.Write(_FinalStage);
        }

        public void Deserialize(GenericReader reader)
        {
            var version     = reader.ReadInt();
            InvasionTown    = (InvasionTowns)reader.ReadInt();
            TownMonsterType = (TownMonsterType)reader.ReadInt();
            TownChampionType = (TownChampionType)reader.ReadInt();
            StartTime       = reader.ReadDateTime();
            Spawned         = reader.ReadStrongMobileList();
            Active          = reader.ReadBool();

            if (version >= 1)
            {
                WasDisabledRegion = reader.ReadBool();
                _FinalStage       = reader.ReadBool();
            }

            if (Spawned == null)
                Spawned = new List<Mobile>();

            if (Active)
                InitTimer();
        }

        #region Private Methods

        private static void GlobalSync()
        {
            var index = InvasionControl.Invasions.Count;

            while (--index >= 0)
            {
                if (index >= InvasionControl.Invasions.Count)
                    continue;

                var obj = InvasionControl.Invasions[index];

                if (obj._StartTime <= DateTime.UtcNow && !obj.IsRunning)
                {
                    // Enforce 1-hour per-town cooldown
                    if (InvasionControl.IsOnCooldown(obj._InvasionTown))
                        continue;

                    obj.OnStart();
                    InvasionControl.RefreshAllOpenGumps();
                }
            }
        }

        private void InitTimer()
        {
            if (!IsRunning)
                _SpawnTimer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(15.0), CheckSpawn);
        }

        private void Spawn()
        {
            Despawn();

            MonsterTownSpawnEntry[] entries = null;

            switch (_TownMonsterType)
            {
                default:
                case TownMonsterType.Abyss:        entries = MonsterTownSpawnEntry.Abyss;        break;
                case TownMonsterType.Arachnid:     entries = MonsterTownSpawnEntry.Arachnid;     break;
                case TownMonsterType.DragonKind:   entries = MonsterTownSpawnEntry.DragonKind;   break;
                case TownMonsterType.Elementals:   entries = MonsterTownSpawnEntry.Elementals;   break;
                case TownMonsterType.Humanoid:     entries = MonsterTownSpawnEntry.Humanoid;     break;
                case TownMonsterType.Ophidian:     entries = MonsterTownSpawnEntry.Ophidian;     break;
                case TownMonsterType.OrcsandRatmen:entries = MonsterTownSpawnEntry.OrcsandRatmen;break;
                case TownMonsterType.OreElementals:entries = MonsterTownSpawnEntry.OreElementals;break;
                case TownMonsterType.Snakes:       entries = MonsterTownSpawnEntry.Snakes;       break;
                case TownMonsterType.Undead:       entries = MonsterTownSpawnEntry.Undead;       break;
            }

            for (int i = 0; i < entries.Length; ++i)
                for (int count = 0; count < entries[i].Amount; ++count)
                    AddMonster(entries[i].Monster);

            if (_Spawned.Count == 0)
            {
                OnStop();
                return;
            }

            InitTimer();
        }

        public void CheckSpawn()
        {
            int count = 0;

            for (int i = 0; i < _Spawned.Count; ++i)
                if (_Spawned[i] != null && !_Spawned[i].Deleted && _Spawned[i].Alive)
                    ++count;

            if (!_FinalStage)
            {
                if (count == 0)
                    SpawnChamp();
            }
            else
            {
                if (count == 0)
                    Timer.DelayCall(TimeSpan.FromMinutes(5), OnStop);
            }
        }

        private void Despawn()
        {
            foreach (Mobile m in _Spawned)
                if (m != null && !m.Deleted)
                    m.Delete();

            _Spawned.Clear();
            _FinalStage = false;
        }

        private Point3D FindSpawnLocation()
        {
            int x, y, z;
            var count = 100;

            do
            {
                x = Utility.Random(_Top.X, (_Bottom.X - _Top.X));
                y = Utility.Random(_Top.Y, (_Bottom.Y - _Top.Y));
                z = SpawnMap.GetAverageZ(x, y);
            }
            while (!SpawnMap.CanSpawnMobile(x, y, z) && --count >= 0);

            if (count < 0)
                x = y = z = 0;

            return new Point3D(x, y, z);
        }

        private void AddMonster(Type type)
        {
            object monster = Activator.CreateInstance(type);

            if (monster != null && monster is Mobile)
            {
                Point3D location = FindSpawnLocation();

                if (location == Point3D.Zero)
                    return;

                Mobile from = (Mobile)monster;

                from.OnBeforeSpawn(location, SpawnMap);
                from.MoveToWorld(location, SpawnMap);
                from.OnAfterSpawn();

                if (from is BaseCreature)
                    ((BaseCreature)from).Tamable = false;

                _Spawned.Add(from);
            }
        }

        public void SpawnChamp()
        {
            Despawn();
            _FinalStage = true;

            // Champion arrival announcement
            string champMsg = String.Format(
                "THE CHAMPION OF {0} HAS ARRIVED! {1} now threatens the city — all heroes to arms!",
                TownInvaded.ToUpper(), TownChampionType);
            World.Broadcast(0x22, true, champMsg);

            switch (_TownChampionType)
            {
                default:
                case TownChampionType.Barracoon: AddMonster(typeof(Barracoon)); break;
                case TownChampionType.Harrower:  AddMonster(typeof(Harrower));  break;
                case TownChampionType.LordOaks:  AddMonster(typeof(LordOaks));  break;
                case TownChampionType.Mephitis:  AddMonster(typeof(Mephitis));  break;
                case TownChampionType.Neira:     AddMonster(typeof(Neira));     break;
                case TownChampionType.Rikktor:   AddMonster(typeof(Rikktor));   break;
                case TownChampionType.Semidar:   AddMonster(typeof(Semidar));   break;
                case TownChampionType.Serado:    AddMonster(typeof(Serado));    break;
            }
        }

        #endregion
    }

    // -------------------------------------------------------------------------
    // INVASION CONTROL  (was InvasionControl.cs)
    // -------------------------------------------------------------------------

    public static class InvasionControl
    {
        public static List<TownInvasion> Invasions = new List<TownInvasion>();

        // Tracks when each town's cooldown expires (1 hour after invasion ends)
        public static Dictionary<InvasionTowns, DateTime> TownCooldowns = new Dictionary<InvasionTowns, DateTime>();

        public static void Initialize()
        {
            // Single command to open the invasion gump
            CommandSystem.Register("Invasions", AccessLevel.Administrator, (e) =>
            {
                RefreshGump(e.Mobile);
            });

            // Centralized 5-minute red system broadcast grouping all active invasions
            Timer.DelayCall(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5), AnnounceActiveInvasions);
        }

        public static bool IsOnCooldown(InvasionTowns town)
        {
            DateTime cooldownEnd;
            return TownCooldowns.TryGetValue(town, out cooldownEnd) && cooldownEnd > DateTime.UtcNow;
        }

        public static TimeSpan GetCooldownRemaining(InvasionTowns town)
        {
            DateTime cooldownEnd;
            if (TownCooldowns.TryGetValue(town, out cooldownEnd) && cooldownEnd > DateTime.UtcNow)
                return cooldownEnd - DateTime.UtcNow;
            return TimeSpan.Zero;
        }

        private static void AnnounceActiveInvasions()
        {
            var active = new List<TownInvasion>();
            foreach (var inv in Invasions)
                if (inv.IsRunning) active.Add(inv);

            if (active.Count == 0)
                return;

            string msg;

            if (active.Count == 1)
            {
                var inv = active[0];
                if (inv.IsFinalStage)
                    msg = String.Format(
                        "INVASION ALERT: The champion {0} still threatens {1}! Heroes needed!",
                        inv.TownChampionType, inv.TownInvaded);
                else
                    msg = String.Format(
                        "INVASION ALERT: {0} remains under siege by {1}! Come to the city's defense!",
                        inv.TownInvaded, inv.TownMonsterType);
            }
            else
            {
                var parts = new List<string>();
                foreach (var inv in active)
                    parts.Add(String.Format("{0} ({1})", inv.TownInvaded,
                        inv.IsFinalStage ? inv.TownChampionType.ToString() : inv.TownMonsterType.ToString()));

                msg = String.Format(
                    "INVASION ALERT: Multiple cities are under siege — {0}. Defend the realm!",
                    String.Join(", ", parts.ToArray()));
            }

            World.Broadcast(0x22, true, msg);
        }

        public static void RefreshGump(Mobile m)
        {
            if (m == null) return;
            m.CloseGump(typeof(InvasionMasterGump));
            m.SendGump(new InvasionMasterGump());
        }

        public static void RefreshAllOpenGumps()
        {
            foreach (NetState state in NetState.Instances)
            {
                Mobile m = state.Mobile;
                if (m != null && m.HasGump(typeof(InvasionMasterGump)))
                    RefreshGump(m);
            }
        }
    }

    // -------------------------------------------------------------------------
    // PERSISTENCE  (was InvasionControl.cs)
    // -------------------------------------------------------------------------

    public class InvasionPersistence
    {
        private static string FilePath = System.IO.Path.Combine("Saves", "Invasions", "Persistence.bin");

        public static void Configure()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnSave);
            EventSink.WorldLoad += new WorldLoadEventHandler(OnLoad);
        }

        private static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(FilePath, writer =>
            {
                writer.Write(1); // version

                // Invasions
                writer.Write(InvasionControl.Invasions.Count);
                foreach (var inv in InvasionControl.Invasions)
                    inv.Serialize(writer);

                // Town cooldowns
                writer.Write(InvasionControl.TownCooldowns.Count);
                foreach (var kvp in InvasionControl.TownCooldowns)
                {
                    writer.Write((int)kvp.Key);
                    writer.Write(kvp.Value);
                }
            });
        }

        private static void OnLoad()
        {
            Persistence.Deserialize(FilePath, reader =>
            {
                int version = reader.ReadInt();

                // Invasions
                int count = reader.ReadInt();
                for (int i = 0; i < count; ++i)
                {
                    var invasion = new TownInvasion(reader);
                    InvasionControl.Invasions.Add(invasion);
                }

                // Town cooldowns (version 1+)
                if (version >= 1)
                {
                    int cdCount = reader.ReadInt();
                    for (int i = 0; i < cdCount; i++)
                    {
                        var town    = (InvasionTowns)reader.ReadInt();
                        var expires = reader.ReadDateTime();
                        if (expires > DateTime.UtcNow)
                            InvasionControl.TownCooldowns[town] = expires;
                    }
                }
            });
        }
    }
}
