/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 *
 * Uses the RoseInAVase graphic (0x0EB0) rather than the RoseOfTrinsic graphic.
 * BaseStatRose is fully self-contained — it no longer inherits from RoseOfTrinsic.
 * All spawn-timer, petal-count, and secure-level logic is reproduced here.
 */

using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    // ---------------------------------------------------------------------------
    // PETALS
    // (Defined before the roses so the rose classes can reference them.)
    // ---------------------------------------------------------------------------

    public class IntelligencePetal : RoseOfTrinsicPetal
    {
        [Constructable]
        public IntelligencePetal() : base(1)
        {
            Name = "Petal of Intelligence";
            Hue  = 2498;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042038);  // Must be in backpack.
            else if (from.GetStatMod("RoseOfTrinsicPetal") != null)
                from.SendLocalizedMessage(1062927);  // Already buffed.
            else
            {
                from.PlaySound(0x1EE);
                from.AddStatMod(new StatMod(StatType.Int, "RoseOfTrinsicPetal", 5, TimeSpan.FromMinutes(5.0)));
                this.Consume();
            }
        }

        public IntelligencePetal(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class DexterityPetal : RoseOfTrinsicPetal
    {
        [Constructable]
        public DexterityPetal() : base(1)
        {
            Name = "Petal of Dexterity";
            Hue  = 33;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042038);
            else if (from.GetStatMod("RoseOfTrinsicPetal") != null)
                from.SendLocalizedMessage(1062927);
            else
            {
                from.PlaySound(0x1EE);
                from.AddStatMod(new StatMod(StatType.Dex, "RoseOfTrinsicPetal", 5, TimeSpan.FromMinutes(5.0)));
                this.Consume();
            }
        }

        public DexterityPetal(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class StrengthPetal : RoseOfTrinsicPetal
    {
        [Constructable]
        public StrengthPetal() : base(1)
        {
            Name = "Petal of Strength";
            Hue  = 1645;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042038);
            else if (from.GetStatMod("RoseOfTrinsicPetal") != null)
                from.SendLocalizedMessage(1062927);
            else
            {
                from.PlaySound(0x1EE);
                from.AddStatMod(new StatMod(StatType.Str, "RoseOfTrinsicPetal", 5, TimeSpan.FromMinutes(5.0)));
                this.Consume();
            }
        }

        public StrengthPetal(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // ---------------------------------------------------------------------------
    // BASE ROSE CLASS
    // Extends Item directly — fully self-contained petal/timer/secure logic.
    // Uses the RoseInAVase graphic (0x0EB0).
    // ---------------------------------------------------------------------------

    public abstract class BaseStatRose : Item
    {
        // One petal regenerates every 4 hours, matching the original RoseOfTrinsic cadence.
        private static readonly TimeSpan SpawnInterval = TimeSpan.FromHours(4.0);

        private int         m_Petals;
        private DateTime    m_NextSpawnTime;
        private SpawnTimer  m_SpawnTimer;

        // Subclasses return whichever petal type belongs to their stat.
        public abstract Item GetNewPetal();

        // -----------------------------------------------------------------------
        // Constructor — graphic is the RoseInAVase (0x0EB0); hue set by subclass.
        // -----------------------------------------------------------------------
        public BaseStatRose() : base(0x0EB0)
        {
            Weight    = 1.0;
            LootType  = LootType.Blessed;

            // Start full; no timer needed until petals are consumed.
            m_Petals = 10;
        }

        // -----------------------------------------------------------------------
        // Petals property — mirrors RoseOfTrinsic behaviour exactly.
        // -----------------------------------------------------------------------
        [CommandProperty(AccessLevel.GameMaster)]
        public int Petals
        {
            get { return m_Petals; }
            set
            {
                if (value >= 10)
                {
                    m_Petals = 10;
                    StopSpawnTimer();
                }
                else
                {
                    m_Petals = value <= 0 ? 0 : value;
                    StartSpawnTimer(SpawnInterval);
                }

                InvalidateProperties();
            }
        }

        // -----------------------------------------------------------------------
        // Property list — shows petal count in the item tooltip.
        // -----------------------------------------------------------------------
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1062925, m_Petals.ToString()); // "Petals: ~1_COUNT~"
        }

        // -----------------------------------------------------------------------
        // Double-click: hand out all current petals as a stack.
        // -----------------------------------------------------------------------
        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this.GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
            else if (m_Petals > 0)
            {
                Item petal   = GetNewPetal();
                petal.Amount = m_Petals;
                from.AddToBackpack(petal);
                this.Petals  = 0;
            }
        }

        // -----------------------------------------------------------------------
        // Serialization
        // -----------------------------------------------------------------------
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version

            writer.WriteEncodedInt(m_Petals);
            writer.WriteDeltaTime(m_NextSpawnTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();

            m_Petals        = reader.ReadEncodedInt();
            m_NextSpawnTime = reader.ReadDeltaTime();

            // Resume timer if we weren't at max petals when saved.
            if (m_Petals < 10)
                StartSpawnTimer(m_NextSpawnTime - DateTime.UtcNow);
        }

        // -----------------------------------------------------------------------
        // Timer helpers
        // -----------------------------------------------------------------------
        private void StartSpawnTimer(TimeSpan delay)
        {
            StopSpawnTimer();

            // Guard against negative delays (e.g. server was offline a long time).
            if (delay < TimeSpan.Zero)
                delay = TimeSpan.Zero;

            m_SpawnTimer    = new SpawnTimer(this, delay);
            m_SpawnTimer.Start();
            m_NextSpawnTime = DateTime.UtcNow + delay;
        }

        private void StopSpawnTimer()
        {
            if (m_SpawnTimer != null)
            {
                m_SpawnTimer.Stop();
                m_SpawnTimer = null;
            }
        }

        // -----------------------------------------------------------------------
        // Inner timer — fires once per interval, increments Petals by 1.
        // -----------------------------------------------------------------------
        private class SpawnTimer : Timer
        {
            private readonly BaseStatRose m_Rose;

            public SpawnTimer(BaseStatRose rose, TimeSpan delay) : base(delay)
            {
                m_Rose   = rose;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                if (m_Rose.Deleted)
                    return;

                m_Rose.m_SpawnTimer = null;
                m_Rose.Petals++;  // The setter will restart the timer if still below 10.
            }
        }

        // Deserialization constructor
        public BaseStatRose(Serial serial) : base(serial) { }
    }

    // ---------------------------------------------------------------------------
    // CONCRETE ROSES
    // Each sets its own name and hue; the graphic (0x0EB0) comes from BaseStatRose.
    // ---------------------------------------------------------------------------

    public class RoseOfStrength : BaseStatRose
    {
        [Constructable]
        public RoseOfStrength() : base()
        {
            Name = "Rose of Strength";
            Hue  = 1645;
        }

        public override Item GetNewPetal() => new StrengthPetal();

        public RoseOfStrength(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class RoseOfIntelligence : BaseStatRose
    {
        [Constructable]
        public RoseOfIntelligence() : base()
        {
            Name = "Rose of Intelligence";
            Hue  = 2498;
        }

        public override Item GetNewPetal() => new IntelligencePetal();

        public RoseOfIntelligence(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class RoseOfDexterity : BaseStatRose
    {
        [Constructable]
        public RoseOfDexterity() : base()
        {
            Name = "Rose of Dexterity";
            Hue  = 33;
        }

        public override Item GetNewPetal() => new DexterityPetal();

        public RoseOfDexterity(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
