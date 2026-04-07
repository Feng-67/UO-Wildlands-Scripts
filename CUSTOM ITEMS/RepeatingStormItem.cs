/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server;
//using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
    public class RepeatingStormItem : Item
    {
        private Timer m_Timer;
        private int m_StrikeCount;

        private const double CycleDuration = 18.0;
        private const double ThunderOffset = 5.8;
        private const int StrikeRadius = 6;

        // Every 4th cycle fires a Chain Lightning burst; others fire single Lightning strikes.
        private const int ChainLightningCycle = 4;

        [Constructable]
        public RepeatingStormItem() : base(0x1B71)
        {
            Movable = false;
            Visible = false;
            m_StrikeCount = 0;

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(CycleDuration - ThunderOffset), TimeSpan.FromSeconds(CycleDuration), OnThunderTick);
        }

        public RepeatingStormItem(Serial serial) : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.Alive && !m.IsStaff())
            {
                //SpellHelper.Damage(TimeSpan.FromTicks(1), m, m, Utility.RandomMinMax(1, 5));
            }

            return true;
        }

        private void OnThunderTick()
        {
            if (Map == null || Map == Map.Internal)
                return;

            // Thunder warning sound before strike
            Effects.PlaySound(Location, Map, 0x206);

            Timer.DelayCall(TimeSpan.FromSeconds(ThunderOffset), FireLightningSequence);
        }

        private void FireLightningSequence()
        {
            if (Map == null || Map == Map.Internal)
                return;

            m_StrikeCount++;

            if (m_StrikeCount >= ChainLightningCycle)
            {
                // Chain Lightning burst — hits up to 3 nearby mobiles and fires ambient strikes
                FireChainLightning();
                m_StrikeCount = 0;
            }
            else
            {
                // Single Lightning strike — hits one nearby mobile or fires ambient
                FireSingleLightning();
            }
        }

        // Mirrors Lightning spell: picks one target in range, bolts and damages it.
        // Falls back to an ambient decorative strike if no valid target is found.
        private void FireSingleLightning()
        {
            Mobile target = GetRandomTarget(StrikeRadius);

            if (target != null)
            {
                Effects.SendBoltEffect(target, true, 0, false);
                Effects.PlaySound(target.Location, Map, 0x29);
                //SpellHelper.Damage(TimeSpan.FromTicks(1), target, null, Utility.RandomMinMax(10, 23), 0, 0, 0, 0, 100);
            }
            else
            {
                FireAmbientStrike(GetRandomLocation(StrikeRadius));
            }
        }

        // Mirrors Chain Lightning spell: hits up to 3 targets in range, bolts and damages each.
        // Fills any remaining strikes with ambient decorative bolts.
        private void FireChainLightning()
        {
            List<Mobile> targets = GetNearbyTargets(StrikeRadius, 6);
            int ambientCount = 6 - targets.Count;

            foreach (Mobile target in targets)
            {
                double damage = Utility.RandomMinMax(20, 51);

                if (targets.Count > 2)
                    damage = (damage * 2) / targets.Count;

                Effects.SendBoltEffect(target, true, 0, false);
                Effects.PlaySound(target.Location, Map, 0x29);
                //SpellHelper.Damage(TimeSpan.FromTicks(1), target, null, damage, 0, 0, 0, 0, 100);
            }

            for (int i = 0; i < ambientCount; i++)
                FireAmbientStrike(GetRandomLocation(StrikeRadius));
        }

        // Decorative bolt with no damage target — uses a temporary visible Item so
        // SendBoltEffect passes CanSee for regular players (EffectMobile is hidden and fails).
        private void FireAmbientStrike(Point3D loc)
        {
            Item anchor = new Item(1);
            anchor.Movable = false;
            anchor.MoveToWorld(loc, Map);
            Effects.SendBoltEffect(anchor, true, 0, false);
            Effects.PlaySound(loc, Map, 0x29);
            anchor.Delete();
        }

        // Returns a single random valid hostile mobile within radius.
        private Mobile GetRandomTarget(int radius)
        {
            List<Mobile> pool = GetNearbyTargets(radius, 1);
            return pool.Count > 0 ? pool[0] : null;
        }

        // Returns up to maxCount valid hostile mobiles within radius, in random order.
        private List<Mobile> GetNearbyTargets(int radius, int maxCount)
        {
            List<Mobile> result = new List<Mobile>();

            foreach (Mobile m in Map.GetMobilesInRange(Location, radius))
            {
                if (result.Count >= maxCount)
                    break;

                if (m.Alive && !m.IsStaff() && !m.Hidden)
                    result.Add(m);
            }

            return result;
        }

        private Point3D GetRandomLocation(int radius)
        {
            int x = X + Utility.RandomMinMax(-radius, radius);
            int y = Y + Utility.RandomMinMax(-radius, radius);
            int z = Map.GetAverageZ(x, y);

            return new Point3D(x, y, z);
        }

        public override void OnDelete()
        {
            base.OnDelete();
            if (m_Timer != null) m_Timer.Stop();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write((int)m_StrikeCount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_StrikeCount = reader.ReadInt();

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(CycleDuration - ThunderOffset), TimeSpan.FromSeconds(CycleDuration), OnThunderTick);
        }
    }
}
