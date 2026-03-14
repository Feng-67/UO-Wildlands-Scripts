/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Voxpire)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using Server;
using Server.Regions;

namespace Server.Custom.TownHouses
{
    /// <summary>
    /// Region wrapper for inside checks and ban checks.
    /// </summary>
    public class TownHouseRegion : Region
    {
        public TownHouseController Controller { get; private set; }

        public TownHouseRegion(TownHouseController controller)
            : base(controller.RegionName, controller.Map, controller.RegionPriority, controller.Bounds)
        {
            Controller = controller;
        }

        public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
        {
            if (Controller != null && Controller.IsBanned(m))
                return false;

            return base.OnMoveInto(m, d, newLocation, oldLocation);
        }
    }
}
