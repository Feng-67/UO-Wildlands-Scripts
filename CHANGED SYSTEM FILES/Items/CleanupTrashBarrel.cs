using System;
using System.Collections.Generic;
using System.Linq;
using Server.Engines.Points;
using Server.Mobiles;

namespace Server.Items
{
    public class CleanupTrashBarrel : BaseTrash
    {
        [Constructable]
        public CleanupTrashBarrel()
            : base(0xFAE)
        {
            Hue = 2500;
            Movable = false;
            Name = "Trash - Keep Britannia Clean";
            m_Cleanup = new List<CleanupArray>();
        }

        public CleanupTrashBarrel(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultMaxWeight => 0;      // 0 = unlimited weight
        public override bool IsDecoContainer => false;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Ensure m_Cleanup is initialized (inherited from BaseTrash)
            if (m_Cleanup == null)
                m_Cleanup = new List<CleanupArray>();
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Blessed items are rejected
            if (dropped.LootType == LootType.Blessed)
            {
                PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, 1075256);
                return false;
            }

            // Get Clean Up Britannia points using CleanUpBritanniaData
            double points = CleanUpBritanniaData.GetPoints(dropped);

            // Record the item for this mobile
            m_Cleanup.Add(new CleanupArray { mobiles = from, points = points, confirm = true });

            // Delete the item
            dropped.Delete();

            // Process and send reward message
            Empty();

            return true;
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            // Same logic as OnDragDrop
            if (item.LootType == LootType.Blessed)
            {
                PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, 1075256);
                return false;
            }

            double points = CleanUpBritanniaData.GetPoints(item);

            m_Cleanup.Add(new CleanupArray { mobiles = from, points = points, confirm = true });

            item.Delete();

            Empty();

            return true;
        }

        public void Empty()
        {
            if (m_Cleanup.Count == 0)
                return;

            // Group by mobile and sum points
            var groups = m_Cleanup
                .Where(x => x.mobiles != null)
                .GroupBy(x => x.mobiles)
                .Select(g => new { Mobile = g.Key, TotalPoints = g.Sum(x => x.points), Count = g.Count() });

            foreach (var group in groups)
            {
                // Send message even if points = 0
                group.Mobile.SendLocalizedMessage(1151280,
                    String.Format("{0}\t{1}", group.TotalPoints.ToString(), group.Count.ToString()));

                // Award points (0 for non-eligible items)
                PointsSystem.CleanUpBritannia.AwardPoints(group.Mobile, group.TotalPoints);
            }

            m_Cleanup.Clear();
        }
    }
}
