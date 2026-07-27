/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Commands
{
    public class DeathTeleportCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("RezStone", AccessLevel.Player, new CommandEventHandler(RezStone_OnCommand));
        }

        [Usage("RezStone")]
        [Description("Reopens the Corpse Retrieval Stone teleport gump if you are dead.")]
        public static void RezStone_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            // Check if the player is a ghost
            if (!from.Alive)
            {
                // Close any existing gump to prevent stacking
                from.CloseGump(typeof(DeathTeleportGump));

                // Send the teleport gump
                from.SendGump(new DeathTeleportGump(from));

                from.SendMessage("The teleport option has been reopened.");
            }
            else
            {
                from.SendMessage("You must be dead to use this command.");
            }
        }
    }
}
