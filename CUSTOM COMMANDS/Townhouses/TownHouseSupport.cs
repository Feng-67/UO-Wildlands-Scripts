/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Voxpire)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Items;

namespace Server.Custom.TownHouses
{
    public enum TownHouseState
    {
        Purchasable,
        Rentable,
        Transferable
    }

    public enum TownHouseWipePolicy
    {
        LeaveAsIs,
        WipeItemsInBounds // deletes Items (not map statics/tiles) in bounds
    }

    public static class TownHouseUtil
    {
        public static Rectangle2D MakeRect(Point3D a, Point3D b)
        {
            int x1 = Math.Min(a.X, b.X);
            int y1 = Math.Min(a.Y, b.Y);
            int x2 = Math.Max(a.X, b.X);
            int y2 = Math.Max(a.Y, b.Y);

            return new Rectangle2D(x1, y1, (x2 - x1) + 1, (y2 - y1) + 1);
        }

        public static bool HasBounds(Rectangle2D r)
        {
            return r.Width > 0 && r.Height > 0;
        }

        public static bool In3DRange(Point3D p, Rectangle2D r, int minZ, int maxZ)
        {
            if (!r.Contains(p))
                return false;

            return p.Z >= minZ && p.Z <= maxZ;
        }

        public static void ForEachItemInBounds(Map map, Rectangle2D rect, int minZ, int maxZ, Action<Item> action)
        {
            if (map == null || map == Map.Internal || !HasBounds(rect))
                return;

            IPooledEnumerable eable = map.GetItemsInBounds(rect);
            try
            {
                foreach (Item item in eable)
                {
                    if (item == null || item.Deleted)
                        continue;

                    if (!In3DRange(item.Location, rect, minZ, maxZ))
                        continue;

                    action(item);
                }
            }
            finally
            {
                eable.Free();
            }
        }
    }

    /// <summary>
    /// Temporary highlight tile used for GM visualization.
    /// Auto-deletes after Duration.
    /// </summary>
    public class TownHouseHighlightTile : Item
    {
        private Timer m_Timer;

        public override bool Decays { get { return false; } }

        [Constructable]
        public TownHouseHighlightTile() : base(0x1766) // cut cloth
        {
            Movable = false;
            Hue = 1266;
            Name = "boundary marker";
        }

        public TownHouseHighlightTile(Serial serial) : base(serial) { }

        public void Start(TimeSpan duration)
        {
            Stop();

            m_Timer = Timer.DelayCall(duration, Delete);
            m_Timer.Start();
        }

        public void Stop()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }
        }

        public override void OnDelete()
        {
            Stop();
            base.OnDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
