/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Ravenwolfe)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Customs.Invasion_System
{
    public class InvasionMasterGump : Gump
    {
        public static void Initialize()
        {
            // The single command to rule them all
            CommandSystem.Register("Invasion", AccessLevel.Administrator, (e) => {
                RefreshGump(e.Mobile);
            });
        }

        public static void RefreshGump(Mobile m)
        {
            if (m == null) return;
            if (m.HasGump(typeof(InvasionMasterGump)))
                m.CloseGump(typeof(InvasionMasterGump));
            m.SendGump(new InvasionMasterGump());
        }

        public InvasionMasterGump() : base(50, 50)
        {
            Closable = true;
            Disposable = true;
            Dragable = true;

            // Large canvas to fit both sections
            AddPage(0);
            AddBackground(0, 0, 780, 700, 9270);
            AddAlphaRegion(10, 10, 760, 680);

            // --- SECTION 1: SCHEDULING (TOP) ---
            AddHtml(10, 20, 760, 25, "<BASEFONT COLOR=#FFD700><CENTER>INVASION MASTER CONTROL</CENTER></BASEFONT>", false, false);
            
            // Column 1: Towns
            AddGroup(1);
            AddHtml(30, 55, 150, 20, "<BASEFONT COLOR=#FFD700>1. SELECT TOWN</BASEFONT>", false, false);
            string[] towns = Enum.GetNames(typeof(InvasionTowns));
            for (int i = 0; i < towns.Length; i++) {
                AddRadio(30, 80 + (i * 22), 209, 210, (i == 0), 100 + i);
                AddLabel(55, 80 + (i * 22), 0x481, towns[i]);
            }

            // Column 2: Monsters
            AddGroup(2);
            AddHtml(280, 55, 150, 20, "<BASEFONT COLOR=#FFD700>2. MONSTER TYPE</BASEFONT>", false, false);
            string[] monsters = Enum.GetNames(typeof(TownMonsterType));
            for (int i = 0; i < monsters.Length; i++) {
                AddRadio(280, 80 + (i * 22), 209, 210, (i == 0), 200 + i);
                AddLabel(305, 80 + (i * 22), 0x481, monsters[i]);
            }

            // Column 3: Champions
            AddGroup(3);
            AddHtml(530, 55, 150, 20, "<BASEFONT COLOR=#FFD700>3. CHAMPION</BASEFONT>", false, false);
            string[] champs = Enum.GetNames(typeof(TownChampionType));
            for (int i = 0; i < champs.Length; i++) {
                AddRadio(530, 80 + (i * 22), 209, 210, (i == 0), 300 + i);
                AddLabel(555, 80 + (i * 22), 0x481, champs[i]);
            }

            // Scheduling Controls
            AddImageTiled(20, 380, 740, 1, 9304);
            AddLabel(30, 395, 0x481, "Start Time (UTC):");
            AddBackground(160, 395, 180, 22, 9350);
            AddTextEntry(165, 395, 170, 20, 0x481, 0, DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm"));

            AddButton(360, 392, 247, 248, 1, GumpButtonType.Reply, 0);
            AddLabel(480, 395, 0x42, "CONFIRM SCHEDULE");

            // --- SECTION 2: LIVE MONITOR (BOTTOM) ---
            AddImageTiled(20, 435, 740, 2, 9304);
            AddHtml(20, 445, 740, 20, "<BASEFONT COLOR=#FFD700><CENTER>--- ACTIVE & QUEUED INVASIONS ---</CENTER></BASEFONT>", false, false);

            int listY = 475;
            AddHtml(30, listY, 120, 20, "<BASEFONT COLOR=#FFD700>TOWN</BASEFONT>", false, false);
            AddHtml(180, listY, 120, 20, "<BASEFONT COLOR=#FFD700>BOSS</BASEFONT>", false, false);
            AddHtml(330, listY, 150, 20, "<BASEFONT COLOR=#FFD700>TIME (UTC)</BASEFONT>", false, false);
            AddHtml(530, listY, 100, 20, "<BASEFONT COLOR=#FFD700>STATUS</BASEFONT>", false, false);
            
            AddImageTiled(30, listY + 20, 700, 1, 9304);

            var invasions = InvasionControl.Invasions;
            for (int i = 0; i < invasions.Count; i++)
            {
                int rowY = listY + 30 + (i * 25);
                if (rowY > 660) break; // Prevent overflow

                var inv = invasions[i];
                bool active = DateTime.UtcNow >= inv.StartTime;

                AddLabel(30, rowY, 0x481, inv.InvasionTown.ToString());
                AddLabel(180, rowY, 0x481, inv.TownChampionType.ToString());
                AddLabel(330, rowY, 0x481, inv.StartTime.ToString("MM/dd HH:mm"));
                
                // Color coded status
                string status = active ? "<BASEFONT COLOR=#00FF00>ACTIVE</BASEFONT>" : "<BASEFONT COLOR=#FFFF00>QUEUED</BASEFONT>";
                AddHtml(530, rowY, 100, 20, status, false, false);

                // Stop Button
                AddButton(660, rowY, 4017, 4018, 1000 + i, GumpButtonType.Reply, 0);
                AddLabel(695, rowY, 0x22, "STOP");
            }
        }

public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (info.ButtonID == 1) // Schedule
            {
                // Logic to parse selection and time
                InvasionTowns town = 0; TownMonsterType mob = 0; TownChampionType champ = 0;
                foreach (int sw in info.Switches) {
                    if (sw >= 100 && sw < 200) town = (InvasionTowns)(sw - 100);
                    else if (sw >= 200 && sw < 300) mob = (TownMonsterType)(sw - 200);
                    else if (sw >= 300 && sw < 400) champ = (TownChampionType)(sw - 300);
                }

                TextRelay tr = info.GetTextEntry(0);
                DateTime start;
                if (!DateTime.TryParseExact(tr.Text, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out start))
                    start = DateTime.UtcNow;

                new TownInvasion(town, mob, champ, start);
                
                // PUSH REFRESH to everyone else
                InvasionControl.RefreshAllOpenGumps();

                // RE-OPEN for you so it stays on screen
                RefreshGump(from);
            }
            else if (info.ButtonID >= 1000) // Stop
            {
                int idx = info.ButtonID - 1000;
                if (idx < InvasionControl.Invasions.Count) {
                    InvasionControl.Invasions[idx].OnStop();
                    
                    // PUSH REFRESH to everyone else
                    InvasionControl.RefreshAllOpenGumps();

                    // RE-OPEN for you so it stays on screen
                    RefreshGump(from);
                }
            }
        }
    }
}
