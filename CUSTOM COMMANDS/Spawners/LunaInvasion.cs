/*
 * UO Wildlands Custom Script
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 *
 * Luna Invasion - Four elemental waves followed by Clockwork Exodus.
 * Place with [add LunaInvasion anywhere in the world (it does not need
 * to be inside Luna itself). Double-click as GM to start, or use
 * the [LunaInvasionStart command. 24-hour cooldown after Exodus dies.
 */

using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class LunaInvasion : Item
    {
        // --- Spawn area covers the interior of Luna city, Malas ---
        private static readonly Point3D SpawnTop    = new Point3D(917,  463, -90);
        private static readonly Point3D SpawnBottom = new Point3D(1067, 579, -55);
        private static readonly Map     SpawnMap    = Map.Malas;

        // --- State ---
        private List<Mobile> _spawned     = new List<Mobile>();
        private int          _currentWave = 0;
        private bool         _active      = false;
        private bool         _bossPhase   = false;
        private DateTime     _cooldownEnd = DateTime.MinValue;
        private Timer        _checkTimer;

        // ---------------------------------------------------------------
        // Wave definitions – four elemental waves, weakest to strongest
        // ---------------------------------------------------------------
        private static readonly Type[][] Waves = BuildWaves();

        private static Type[][] BuildWaves()
        {
            return new Type[][]
            {
        // Wave 1 – Earth & Water
        BuildWave(
            (typeof(EarthElemental), 30),
            (typeof(WaterElemental), 30),
            (typeof(AirElemental),   30)),

        // Wave 2 – Fire & Ice
        BuildWave(
            (typeof(FireElemental),  30),
            (typeof(IceElemental),   30),
            (typeof(SnowElemental),  30)),

        // Wave 3 – Air, Poison & Blood
        BuildWave(
            (typeof(AirElemental),    30),
            (typeof(PoisonElemental), 30),
            (typeof(BloodElemental),  30),
            (typeof(Efreet),          30)),

        // Wave 4 – Ore elementals & Efreet
        BuildWave(
            (typeof(ValoriteElemental), 30),
            (typeof(VeriteElemental),   30),
            (typeof(AgapiteElemental),  30),
            (typeof(GoldenElemental),   30),
            (typeof(Efreet),            30))
            };
        }

        private static Type[] BuildWave(params (Type type, int count)[] entries)
        {
            var list = new List<Type>();
            foreach (var (type, count) in entries)
                for (int i = 0; i < count; i++)
                    list.Add(type);
            return list.ToArray();
        }

        // ---------------------------------------------------------------
        // Construction
        // ---------------------------------------------------------------
        [Constructable]
        public LunaInvasion() : base(0xED4)
        {
            Name     = "Luna Invasion Controller";
            Visible  = false;
            Movable  = false;
        }

        public LunaInvasion(Serial serial) : base(serial) { }

        // ---------------------------------------------------------------
        // Server initialisation – registers GM command
        // ---------------------------------------------------------------
        public static void Initialize()
        {
            CommandSystem.Register("LunaInvasionStart", AccessLevel.GameMaster, e =>
            {
                foreach (Item item in World.Items.Values)
                {
                    if (item is LunaInvasion li && !li.Deleted)
                    {
                        li.TryStart(e.Mobile);
                        return;
                    }
                }
                e.Mobile.SendMessage("No LunaInvasion controller was found in the world.");
            });

            CommandSystem.Register("LunaInvasionStop", AccessLevel.GameMaster, e =>
            {
                foreach (Item item in World.Items.Values)
                {
                    if (item is LunaInvasion li && !li.Deleted)
                    {
                        li.ForceStop();
                        e.Mobile.SendMessage("Luna Invasion stopped and all spawned creatures removed.");
                        return;
                    }
                }
                e.Mobile.SendMessage("No LunaInvasion controller was found in the world.");
            });
        }

        // ---------------------------------------------------------------
        // Double-click for GM control
        // ---------------------------------------------------------------
        public override void OnDoubleClick(Mobile from)
        {
            if (!from.IsStaff())
                return;

            if (_active)
            {
                from.SendMessage(
                    "Luna Invasion is already running. {0}",
                    _bossPhase
                        ? "Clockwork Exodus has been summoned!"
                        : string.Format("Wave {0} of {1} is active ({2} creatures remaining).",
                            _currentWave + 1, Waves.Length, _spawned.Count));
                return;
            }

            TryStart(from);
        }

        // ---------------------------------------------------------------
        // Start logic
        // ---------------------------------------------------------------
        public void TryStart(Mobile from)
        {
            if (_active)
            {
                if (from != null) from.SendMessage("Luna Invasion is already active.");
                return;
            }

            if (DateTime.UtcNow < _cooldownEnd)
            {
                TimeSpan remaining = _cooldownEnd - DateTime.UtcNow;
                if (from != null)
                    from.SendMessage(
                        "Luna Invasion is on cooldown for another {0}h {1}m.",
                        (int)remaining.TotalHours, remaining.Minutes);
                return;
            }

            _active      = true;
            _bossPhase   = false;
            _currentWave = 0;
            _spawned     = new List<Mobile>();

            SpawnWave(0);
        }

        // ---------------------------------------------------------------
        // Wave spawning
        // ---------------------------------------------------------------
        private void SpawnWave(int waveIndex)
        {
            _currentWave = waveIndex;

            World.Broadcast(0x22, true,
                string.Format(
                    "LUNA IS UNDER ELEMENTAL SIEGE! WAVE {0} OF {1} HAS BEGUN - DEFEND THE CITY OF LUNA!",
                    waveIndex + 1, Waves.Length));

            foreach (Type t in Waves[waveIndex])
                AddMobile(t);

            StartCheckTimer();
        }

        private void SpawnBoss()
        {
            _bossPhase = true;

            World.Broadcast(0x22, true,
                "THE ELEMENTAL WAVES HAVE BEEN DEFEATED - BUT CLOCKWORK EXODUS RISES FROM THE VOID! " +
                "ALL HEROES TO LUNA - THE CITY MUST NOT FALL!");

            AddMobile(typeof(ClockworkExodus));
            StartCheckTimer();
        }

        // ---------------------------------------------------------------
        // Wave check (runs every 10 seconds)
        // ---------------------------------------------------------------
        private void CheckWave()
        {
            // Prune dead / deleted creatures
            _spawned.RemoveAll(m => m == null || m.Deleted || !m.Alive);

            if (_spawned.Count > 0)
                return; // Wave still has survivors

            StopCheckTimer();

            if (_bossPhase)
            {
                // Exodus is dead – invasion complete
                OnInvasionComplete();
            }
            else
            {
                int nextWave = _currentWave + 1;

                if (nextWave >= Waves.Length)
                {
                    // All four waves cleared – summon the boss
                    World.Broadcast(0x35, true,
                        "ALL FOUR ELEMENTAL WAVES HAVE BEEN DEFEATED! " +
                        "BRACE YOURSELVES - SOMETHING FAR WORSE STIRS BENEATH LUNA...");

                    Timer.DelayCall(TimeSpan.FromSeconds(8.0), () => SpawnBoss());
                }
                else
                {
                    // Short breather between waves
                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => SpawnWave(nextWave));
                }
            }
        }

        // ---------------------------------------------------------------
        // Invasion complete
        // ---------------------------------------------------------------
        private void OnInvasionComplete()
        {
            _active    = false;
            _bossPhase = false;
            _spawned.Clear();
            _cooldownEnd = DateTime.UtcNow + TimeSpan.FromHours(24.0);

            World.Broadcast(0x35, true,
                "CLOCKWORK EXODUS HAS BEEN DEFEATED! LUNA IS SAFE... FOR NOW. " +
                "THE ELEMENTALS WILL RETURN IN 24 HOURS.");

            InvalidateProperties();
        }

        // ---------------------------------------------------------------
        // GM force-stop
        // ---------------------------------------------------------------
        public void ForceStop()
        {
            StopCheckTimer();

            foreach (Mobile m in _spawned)
                if (m != null && !m.Deleted)
                    m.Delete();

            _spawned.Clear();
            _active    = false;
            _bossPhase = false;

            InvalidateProperties();
        }

        // ---------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------
        private void AddMobile(Type type)
        {
            try
            {
                Mobile m = (Mobile)Activator.CreateInstance(type);
                Point3D loc = FindSpawnLocation();

                if (loc == Point3D.Zero)
                {
                    m.Delete();
                    return;
                }

                m.OnBeforeSpawn(loc, SpawnMap);
                m.MoveToWorld(loc, SpawnMap);
                m.OnAfterSpawn();

                if (m is BaseCreature bc)
                    bc.Tamable = false;

                _spawned.Add(m);
            }
            catch { }
        }

        private Point3D FindSpawnLocation()
        {
            for (int attempt = 0; attempt < 100; attempt++)
            {
                int x = Utility.Random(SpawnTop.X, SpawnBottom.X - SpawnTop.X);
                int y = Utility.Random(SpawnTop.Y, SpawnBottom.Y - SpawnTop.Y);
                int z = SpawnMap.GetAverageZ(x, y);

                if (SpawnMap.CanSpawnMobile(x, y, z))
                    return new Point3D(x, y, z);
            }

            return Point3D.Zero;
        }

        private void StartCheckTimer()
        {
            StopCheckTimer();
            _checkTimer = Timer.DelayCall(
                TimeSpan.FromSeconds(10.0),
                TimeSpan.FromSeconds(10.0),
                CheckWave);
        }

        private void StopCheckTimer()
        {
            if (_checkTimer != null)
            {
                _checkTimer.Stop();
                _checkTimer = null;
            }
        }

        // ---------------------------------------------------------------
        // Tooltip
        // ---------------------------------------------------------------
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (_active)
            {
                if (_bossPhase)
                    list.Add("<BASEFONT COLOR=#FF0000>ACTIVE – Clockwork Exodus is loose!</BASEFONT>");
                else
                    list.Add(string.Format(
                        "<BASEFONT COLOR=#FF6600>ACTIVE – Wave {0} of {1} ({2} alive)</BASEFONT>",
                        _currentWave + 1, Waves.Length, _spawned.Count));
            }
            else if (DateTime.UtcNow < _cooldownEnd)
            {
                TimeSpan remaining = _cooldownEnd - DateTime.UtcNow;
                list.Add(string.Format(
                    "<BASEFONT COLOR=#AAAAAA>Cooldown: {0}h {1}m remaining</BASEFONT>",
                    (int)remaining.TotalHours, remaining.Minutes));
            }
            else
            {
                list.Add("<BASEFONT COLOR=#00FF00>[LunaInvasionStart</BASEFONT>");
            }
        }

        // ---------------------------------------------------------------
        // Serialization
        // ---------------------------------------------------------------
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            writer.Write(_active);
            writer.Write(_currentWave);
            writer.Write(_bossPhase);
            writer.Write(_cooldownEnd);
            writer.Write(_spawned, true); // tidy=true removes nulls on write
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            _active      = reader.ReadBool();
            _currentWave = reader.ReadInt();
            _bossPhase   = reader.ReadBool();
            _cooldownEnd = reader.ReadDateTime();
            _spawned     = reader.ReadStrongMobileList();

            if (_spawned == null)
                _spawned = new List<Mobile>();

            // Resume the check timer if the server restarted mid-invasion
            if (_active)
                StartCheckTimer();
        }
    }
}
