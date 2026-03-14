/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Voxpire)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.TownHouses
{
    public class TownHouseBoundsTarget1 : Target
    {
        private readonly Mobile m_GM;
        private readonly TownHouseController m_Controller;

        public TownHouseBoundsTarget1(Mobile gm, TownHouseController c) : base(18, true, TargetFlags.None)
        {
            m_GM = gm;
            m_Controller = c;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Controller == null || m_Controller.Deleted)
                return;

            IPoint3D p = targeted as IPoint3D;

            if (p == null)
                return;

            Point3D p1 = new Point3D(p);
            from.SendMessage("Corner 1 set. Target corner 2.");
            from.Target = new TownHouseBoundsTarget2(m_GM, m_Controller, p1);
        }
    }

    public class TownHouseBoundsTarget2 : Target
    {
        private readonly Mobile m_GM;
        private readonly TownHouseController m_Controller;
        private readonly Point3D m_P1;

        public TownHouseBoundsTarget2(Mobile gm, TownHouseController c, Point3D p1) : base(18, true, TargetFlags.None)
        {
            m_GM = gm;
            m_Controller = c;
            m_P1 = p1;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Controller == null || m_Controller.Deleted)
                return;

            IPoint3D p = targeted as IPoint3D;

            if (p == null)
                return;

            Point3D p2 = new Point3D(p);
            Rectangle2D rect = TownHouseUtil.MakeRect(m_P1, p2);

            int minZ = m_Controller.MinZ;
            int maxZ = m_Controller.MaxZ;

            if (!m_Controller.HasBounds)
            {
                minZ = from.Z - 20;
                maxZ = from.Z + 60;
            }

            m_Controller.SetBounds(rect, minZ, maxZ, "TownHouse_" + m_Controller.Serial.Value);

            from.SendMessage(string.Format("Bounds set: {0}. ZRange: {1}..{2}", rect, minZ, maxZ));
        }
    }
}
