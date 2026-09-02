/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Commands;
using Server.Network;

namespace Server.Gumps
{
    public class MyCommands
    {
        public static void Initialize()
        {
            // Change AccessLevel.Player to AccessLevel.Administrator
            CommandSystem.Register("mycommands", AccessLevel.Administrator, new CommandEventHandler(MyCommands_OnCommand));
        }

        [Usage("mycommands")]
        [Description("Opens a gump with a clickable list of essential commands.")]
        // Ensure this also matches the registration level
        public static void MyCommands_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new MyCommandsGump(e.Mobile));
        }
    }

    public class MyCommandsGump : Gump
    {
        private const int Width = 350;
        private const int Height = 740;

        private readonly List<string> m_AllCommands = new List<string>
{
    // Admin Commands
    "props", "dupe", "townhouses", "delete", "kill", "add", "tele", "skills",
    "set movable false", "set movable true", "move", "admin", "save", "unhide",
    "addsovs 1000", "addsovs -1000", "AdminBank 2000000000", "AdminBank -2000000000",
    
    // Player Commands
    "wallet", "mystats", "chathistory", "chattoggle", "myhouses", "where", "emote", "gethue", "time"
};

        public MyCommandsGump(Mobile from) : base(50, 50)
        {
            from.CloseGump(typeof(MyCommandsGump));

            AddPage(0);
            AddBackground(0, 0, Width, Height, 9270);

            // HEADER: Gold (#FFD700), Caps, Centered - Styled like Wallet Gump
            AddHtml(0, 25, Width, 25, "<CENTER><BASEFONT COLOR=#FFD700>COMMAND MENU</BASEFONT></CENTER>", false, false);
            AddImageTiled(20, 50, Width - 40, 2, 96);

            int y = 70; // Only declare 'int' here once!

            // No "if" check needed here because the command itself is now Admin-only
            foreach (string cmd in m_AllCommands)
            {
                AddButton(25, y + 3, 1209, 1210, GetButtonID(cmd), GumpButtonType.Reply, 0);

                // Using 0x481 (Blue) for all, or 0x21 (Red) if you want them to look like staff commands
                AddLabel(50, y, 0x481, "[" + cmd);

                y += 25;
            }
        }
        

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (info.ButtonID == 0) return;

            string command = GetCommandFromID(info.ButtonID);
            if (!string.IsNullOrEmpty(command))
            {
                CommandSystem.Handle(from, string.Format("{0}{1}", CommandSystem.Prefix, command));
                from.SendGump(new MyCommandsGump(from));
            }
        }

        private int GetButtonID(string command) => 1000 + Math.Abs(command.GetHashCode() % 10000);

        private string GetCommandFromID(int buttonID)
        {
            // Now looks through the single combined list
            foreach (string cmd in m_AllCommands)
            {
                if (GetButtonID(cmd) == buttonID)
                    return cmd;
            }

            return null;
        }
    }
}
