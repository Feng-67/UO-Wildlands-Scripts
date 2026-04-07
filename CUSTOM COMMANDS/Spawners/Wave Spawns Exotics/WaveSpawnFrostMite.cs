/*
 * UO Wildlands Custom Script
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 *
 * Wave Spawner: Frost Mite Mount
 * Four waves centred on the spawner item within a 10-tile radius.
 * Place with [add WaveSpawnerFrostMiteMount, then GM double-click to start.
 * 1-hour cooldown after Frost Mite Mount dies.
 */

using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class WaveSpawnFrostMiteMount : Item
    {
        // Spawn radius in tiles around this item
        private const int SpawnRadius = 10;

        // --- State ---
        private List<Mobile> _spawned     = new List<Mobile>();
        private int          _currentWave = 0;
        private bool         _active      = false;
        private bool         _bossPhase   = false;
        private DateTime     _cooldownEnd = DateTime.MinValue;
        private Timer        _checkTimer;

        // ---------------------------------------------------------------
        // Wave definitions
        // ---------------------------------------------------------------
        private static readonly Type[][] Waves = BuildWaves();

        private static Type[][] BuildWaves()
        {
            return new Type[][]
            {
                // Wave 1
                BuildWave(
                    (typeof(DreadSpider), 20)),

                // Wave 2
                BuildWave(
                    (typeof(Malefic), 5)),

                // Wave 3
                BuildWave(
                    (typeof(ColossusGuardian), 5)),

                // Wave 4
                BuildWave(
                    (typeof(Silk), 10)),

                // Wave 5
                BuildWave(
                    (typeof(Balron), 10))
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
        public WaveSpawnFrostMiteMount() : base(0xED4)
        {
            Name    = "Wave Spawn: Frost Mite Mount";
            Movable = false;
            Visible = false;
            StartProximityTimer();
        }

        public WaveSpawnFrostMiteMount(Serial serial) : base(serial) { }


        /// ---------------------------------------------------------------
        // Double-click for GM control
        // ---------------------------------------------------------------
        public override void OnDoubleClick(Mobile from)
        {
            if (!from.IsStaff())
                return;

            if (_active)
            {
                ForceStop();
                from.SendMessage("Wave Spawner stopped and all spawned creatures removed.");
            }
            else
            {
                TryStart(from);
            }
        }

        private Timer _proximityTimer;

        private void StartProximityTimer()
        {
            if (_proximityTimer != null)
                return;

            _proximityTimer = Timer.DelayCall(
                TimeSpan.FromSeconds(5.0),
                TimeSpan.FromSeconds(5.0),
                CheckProximity);
        }

        private void CheckProximity()
        {
            if (_active)
                return;

            foreach (NetState ns in NetState.Instances)
            {
                Mobile m = ns.Mobile;
                if (m != null && m.Player && m.Map == Map && m.InRange(Location, 10))
                {
                    TryStart(null);
                    return;
                }
            }
        }

        // ---------------------------------------------------------------
        // Start logic
        // ---------------------------------------------------------------
        public void TryStart(Mobile from)
        {
            if (_active)
            {
                if (from != null) from.SendMessage("Wave Spawner is already active.");
                return;
            }

            if (DateTime.UtcNow < _cooldownEnd)
            {
                TimeSpan remaining = _cooldownEnd - DateTime.UtcNow;
                if (from != null)
                    from.SendMessage(
                        "Wave Spawner is on cooldown for another {0}h {1}m.",
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

            Effects.PlaySound(Location, Map, 0x008);
            Effects.PlaySound(Location, Map, 0x008);
            BroadcastLocal("YOU FEEL A CHILL IN THE AIR...", 0x22);

            foreach (Type t in Waves[waveIndex])
                AddMobile(t);

            StartCheckTimer();
        }

        private void SpawnBoss()
        {
            _bossPhase = true;

            Effects.PlaySound(Location, Map, 0x008);
            AddMobile(typeof(FrostMiteMount));
            StartCheckTimer();
        }

        // ---------------------------------------------------------------
        // Wave check (runs every 10 seconds)
        // ---------------------------------------------------------------
        private void CheckWave()
        {
            _spawned.RemoveAll(m => m == null || m.Deleted || !m.Alive);

            if (_spawned.Count > 0)
                return;

            StopCheckTimer();

            if (_bossPhase)
            {
                OnInvasionComplete();
            }
            else
            {
                int nextWave = _currentWave + 1;
                if (nextWave >= Waves.Length)
                {
                    BroadcastLocal("ALL WAVES DEFEATED! A CREATURE APPROACHES...", 0x22);

                    // Lightning strikes over the next 6 seconds before the boss spawns
                    for (int i = 0; i < 6; i++)
                    {
                        int delay = i * 1;
                        Timer.DelayCall(TimeSpan.FromSeconds(delay), () =>
                        {
                            int x = X + Utility.RandomMinMax(-SpawnRadius, SpawnRadius);
                            int y = Y + Utility.RandomMinMax(-SpawnRadius, SpawnRadius);
                            int z = Map.GetAverageZ(x, y);
                            Point3D strikeLocation = new Point3D(x, y, z);

                            EffectMobile em = EffectMobile.Create(strikeLocation, Map, EffectMobile.DefaultDuration);
                            Effects.SendBoltEffect(em, true, 0, false);
                            Effects.PlaySound(strikeLocation, Map, 0x029);
                        });
                    }

                    Timer.DelayCall(TimeSpan.FromSeconds(8.0), () => SpawnBoss());
                }
                else
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => SpawnWave(nextWave));
                }
            }
        }

        // ---------------------------------------------------------------
        // Complete
        // ---------------------------------------------------------------
        private void OnInvasionComplete()
        {
            _active      = false;
            _bossPhase   = false;
            _cooldownEnd = DateTime.UtcNow + TimeSpan.FromHours(1.0);
            _spawned.Clear();

            //Effects.PlaySound(Location, Map, 0x207);
            //BroadcastLocal("THE DREADHORN HAS BEEN SLAIN! THE FOREST IS SAFE... FOR NOW. ", 0x22);

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

                m.OnBeforeSpawn(loc, Map);
                m.MoveToWorld(loc, Map);
                m.OnAfterSpawn();

                if (m is BaseCreature bc)
                {
                    bc.Tamable = false;
                    bc.Home = loc;
                    bc.RangeHome = 20;
                }
                _spawned.Add(m);
            }
            catch { }
        }

        private Point3D FindSpawnLocation()
        {
            for (int attempt = 0; attempt < 100; attempt++)
            {
                int x = X + Utility.RandomMinMax(-SpawnRadius, SpawnRadius);
                int y = Y + Utility.RandomMinMax(-SpawnRadius, SpawnRadius);
                int z = Map.GetAverageZ(x, y);

                if (Map.CanSpawnMobile(x, y, z))
                    return new Point3D(x, y, z);
            }

            return Point3D.Zero;
        }

        private void BroadcastLocal(string message, int hue = 0)
        {
            foreach (NetState ns in NetState.Instances)
            {
                Mobile m = ns.Mobile;
                if (m != null && m.Map == Map && m.InRange(Location, 30))
                    m.SendMessage(hue, message);
            }
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
            writer.Write(_spawned, true);
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

            if (_active)
                StartCheckTimer();

            StartProximityTimer();
        }
    }
}
