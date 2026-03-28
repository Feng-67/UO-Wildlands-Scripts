/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using Server;
using Server.Gumps;
using Server.Network;

public class DeathTeleportGump : Gump
{
    private Mobile m_Mobile;

    public DeathTeleportGump(Mobile mobile) : base(150, 150)
    {
        m_Mobile = mobile;

        AddPage(0);
        AddBackground(0, 0, 300, 150, 9270);

        AddHtml(10, 20, 280, 60, "<center><basefont color=\"#FFFFFF\">Would you like to travel to the Corpse Retrieval Stone?</basefont></center>", false, false);

        AddButton(50, 100, 247, 248, 1, GumpButtonType.Reply, 0); // OK
        AddButton(180, 100, 241, 242, 0, GumpButtonType.Reply, 0); // Cancel
    }

    public override void OnResponse(NetState state, RelayInfo info)
    {
        if (info.ButtonID == 1)
        {
            // Direct coordinates: 3471 (X), 2601 (Y), 10 (Z)
            // Using Map.Trammel specifically
            m_Mobile.MoveToWorld(new Point3D(3471, 2601, 10), Map.Trammel);
        }
    }
}
