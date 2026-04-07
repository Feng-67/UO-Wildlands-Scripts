/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Spells;

namespace Server.Items
{
    public class RepeatingFlameStrikeItem : Item
    {
        private Timer m_Timer;

        private const double CycleDuration = 20.0;
        private const double FireOffset = 3.0;

        [Constructable]
        public RepeatingFlameStrikeItem() : base(0x1B71)
        {
            Movable = false;
            Visible = false;

            // Start the cycle. First warning at 2s for immediate feedback, then every 20s.
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(CycleDuration), OnWarningTick);
        }

        public RepeatingFlameStrikeItem(Serial serial) : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.Alive && !m.IsStaff())
            {
                // Visual and damage on walk-through
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                m.PlaySound(0x227);

                SpellHelper.Damage(TimeSpan.FromTicks(1), m, m, Utility.RandomMinMax(1, 5));
            }

            return true;
        }

        private void OnWarningTick()
        {
            if (Map == null || Map == Map.Internal)
                return;

            // Play warning sound 0x226
            Effects.PlaySound(Location, Map, 0x226);

            // Schedule the actual FlameStrike 4 seconds later
            Timer.DelayCall(TimeSpan.FromSeconds(FireOffset), OnFireStrike);
        }

        private void OnFireStrike()
        {
            if (Map == null || Map == Map.Internal)
                return;

            // Visual effect
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);

            // Play fire sound 0x227
            Effects.PlaySound(Location, Map, 0x227);
        }

        public override void OnDelete()
        {
            base.OnDelete();

            if (m_Timer != null)
                m_Timer.Stop();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(CycleDuration - FireOffset), TimeSpan.FromSeconds(CycleDuration), OnWarningTick);
        }
    }
}
