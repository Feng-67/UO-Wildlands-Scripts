/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Accounting;
using Server.Engines.UOStore;
using Server.Engines.Points;
using Server.Commands;

namespace Server.Gumps
{
    public class WalletGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("Wallet", AccessLevel.Player, e => 
            {
                RefreshGump(e.Mobile);
            });
        }

        public static void RefreshGump(Mobile from)
        {
            if (from == null) return;
            from.CloseGump(typeof(WalletGump));
            from.SendGump(new WalletGump(from));
        }

        public WalletGump(Mobile from) : base(150, 150)
        {
            Account acct = from.Account as Account;
            if (acct == null) return;

            // 1. Get Account Balances
            double totalGold = 0;
            int goldStub;
            acct.GetGoldBalance(out goldStub, out totalGold);
            long sovereigns = UltimaStore.GetCurrency(from);

            // 2. Get Character CUB Points
            double cubPoints = PointsSystem.CleanUpBritannia.GetPoints(from);

            AddPage(0);
            
            // Dark Stone Background + Alpha
            AddBackground(0, 0, 400, 260, 9270); 
            AddAlphaRegion(10, 10, 380, 240);

            // HEADER: Yellow, Caps, Centered
            AddHtml(10, 25, 380, 25, "<CENTER><BASEFONT COLOR=#FFD700>ACCOUNT WALLET</BASEFONT></CENTER>", false, false);
            AddImageTiled(20, 50, 360, 2, 96);

            // --- ACCOUNT GOLD ---
            AddLabel(40, 70, 0x35, "ACCOUNT GOLD"); 
            AddLabel(360 - GetWidth(totalGold), 70, 0x481, totalGold.ToString("N0")); 

            // --- ACCOUNT SOVS ---
            AddLabel(40, 105, 0x35, "ACCOUNT SOVS"); 
            AddLabel(360 - GetWidth(sovereigns), 105, 0x481, sovereigns.ToString("N0"));

            // --- CUB POINTS ---
            AddLabel(40, 140, 0x35, "CUB POINTS"); 
            AddLabel(360 - GetWidth(cubPoints), 140, 0x481, cubPoints.ToString("N0"));

            AddImageTiled(20, 180, 360, 2, 96);

            // BUTTONS
            // Refresh Button (ButtonID 1)
            AddButton(40, 205, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(75, 205, 0x481, "REFRESH");

            // Close Button (ButtonID 0)
            AddButton(250, 205, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddLabel(285, 205, 0x481, "CLOSE");
        }

        // Improved logic to calculate text width for standard UO font
        private int GetWidth(double val)
        {
            string s = val.ToString("N0");
            int width = 0;

            foreach (char c in s)
            {
                if (c == ',' || c == '.') width += 3; // Commas are very thin
                else if (c == '1') width += 5;        // Ones are thin
                else width += 8;                      // Standard numbers (0, 2-9)
            }
            return width;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1) // Refresh
            {
                RefreshGump(sender.Mobile);
            }
        }
    }
}
