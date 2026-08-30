using System;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    public class SimpleTextGump : Gump
    {
        public SimpleTextGump(Mobile from, Item item, string text1, string text2)
            : this(from, text1, text2)
        {
        }

        public SimpleTextGump(Mobile from, string text1, string text2) : base(150, 150)
        {
            AddPage(0);

            // Dark Stone Background + Alpha (matching WalletGump)
            AddBackground(0, 0, 450, 280, 9270);
            AddAlphaRegion(10, 10, 430, 260);

            // HEADER: Yellow, Caps, Centered
            string title = "LORE";
            if (!String.IsNullOrEmpty(text2) && text2.Length < 30 && text2.Contains("\""))
                title = "INSCRIPTION";

            AddHtml(10, 20, 430, 30, String.Format("<CENTER><BASEFONT COLOR=#FFD700>{0}</BASEFONT></CENTER>", title), false, false);
            //AddImageTiled(20, 55, 410, 2, 96);

            // Main Text (with scrollable region if needed)
            AddHtml(30, 70, 390, 100, String.Format("<BASEFONT COLOR=#FFFFFF>{0}</BASEFONT>", text1), true, true);

            // Second Text / Inscription (if provided)
            if (!String.IsNullOrEmpty(text2))
            {
                // Move separator down to avoid overlapping text
                //AddImageTiled(30, 190, 390, 2, 96);
                // Move text up slightly and increase height
                AddHtml(30, 180, 380, 35, String.Format("<CENTER><BASEFONT COLOR=#87CEEB>{0}</BASEFONT></CENTER>", text2), false, false);
            }

            //AddImageTiled(20, 240, 410, 2, 96);

            // Close Button (matching WalletGump style)
            AddButton(185, 245, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddLabel(220, 245, 0x481, "CLOSE");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            // No action needed for button 0 (close)
        }
    }
}
