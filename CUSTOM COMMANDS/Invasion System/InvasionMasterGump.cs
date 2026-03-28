/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Ravenwolfe)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 *
 * Changes from original:
 *   - Initialize / command registration moved to InvasionControl ([Invasions)
 *   - Cooldown indicator shown next to towns on cooldown in scheduling panel
 *   - Cooldown check enforced on schedule confirmation
 *   - Gump modernised: consistent header style, cleaner column layout
 */
using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Customs.Invasion_System
{
    public class InvasionMasterGump : Gump
    {
        public InvasionMasterGump() : base(50, 50)
        {
            Closable  = true;
            Disposable = true;
            Dragable  = true;

            AddPage(0);
            AddBackground(0, 0, 780, 700, 9270);
            AddAlphaRegion(10, 10, 760, 680);

            // --- HEADER ---
            AddHtml(10, 20, 760, 25,
                "<CENTER><BASEFONT COLOR=#FFD700>INVASION MASTER CONTROL</BASEFONT></CENTER>",
                false, false);

            // ----------------------------------------------------------------
            // SECTION 1: SCHEDULING
            // ----------------------------------------------------------------

            // Column 1: Towns
            AddGroup(1);
            AddHtml(30, 55, 150, 20,
                "<BASEFONT COLOR=#FFD700>1. SELECT TOWN</BASEFONT>", false, false);

            string[] towns = Enum.GetNames(typeof(InvasionTowns));
            for (int i = 0; i < towns.Length; i++)
            {
                AddRadio(30, 80 + (i * 22), 209, 210, (i == 0), 100 + i);
                AddLabel(55, 80 + (i * 22), 0x481, towns[i]);

                // Show cooldown remaining in red next to towns on cooldown
                if (InvasionControl.IsOnCooldown((InvasionTowns)i))
                {
                    TimeSpan rem = InvasionControl.GetCooldownRemaining((InvasionTowns)i);
                    AddLabel(160, 80 + (i * 22), 33,
                        String.Format("({0}m)", (int)rem.TotalMinutes + 1));
                }
            }

            // Column 2: Monsters
            AddGroup(2);
            AddHtml(280, 55, 150, 20,
                "<BASEFONT COLOR=#FFD700>2. MONSTER TYPE</BASEFONT>", false, false);

            string[] monsters = Enum.GetNames(typeof(TownMonsterType));
            for (int i = 0; i < monsters.Length; i++)
            {
                AddRadio(280, 80 + (i * 22), 209, 210, (i == 0), 200 + i);
                AddLabel(305, 80 + (i * 22), 0x481, monsters[i]);
            }

            // Column 3: Champions
            AddGroup(3);
            AddHtml(530, 55, 150, 20,
                "<BASEFONT COLOR=#FFD700>3. CHAMPION</BASEFONT>", false, false);

            string[] champs = Enum.GetNames(typeof(TownChampionType));
            for (int i = 0; i < champs.Length; i++)
            {
                AddRadio(530, 80 + (i * 22), 209, 210, (i == 0), 300 + i);
                AddLabel(555, 80 + (i * 22), 0x481, champs[i]);
            }

            // Scheduling controls
            AddImageTiled(20, 380, 740, 1, 9304);
            AddLabel(30, 395, 0x481, "Start Time (UTC):");
            AddBackground(160, 395, 180, 22, 9350);
            AddTextEntry(165, 395, 170, 20, 0x481, 0, DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm"));

            AddButton(360, 392, 247, 248, 1, GumpButtonType.Reply, 0);
            AddLabel(480, 395, 0x42, "CONFIRM SCHEDULE");

            // ----------------------------------------------------------------
            // SECTION 2: LIVE MONITOR
            // ----------------------------------------------------------------
            AddImageTiled(20, 435, 740, 2, 9304);
            AddHtml(20, 445, 740, 20,
                "<BASEFONT COLOR=#FFD700><CENTER>--- ACTIVE & QUEUED INVASIONS ---</CENTER></BASEFONT>",
                false, false);

            int listY = 475;
            AddHtml(30,  listY, 120, 20, "<BASEFONT COLOR=#FFD700>TOWN</BASEFONT>",   false, false);
            AddHtml(180, listY, 120, 20, "<BASEFONT COLOR=#FFD700>BOSS</BASEFONT>",   false, false);
            AddHtml(330, listY, 150, 20, "<BASEFONT COLOR=#FFD700>TIME (UTC)</BASEFONT>", false, false);
            AddHtml(530, listY, 100, 20, "<BASEFONT COLOR=#FFD700>STATUS</BASEFONT>", false, false);

            AddImageTiled(30, listY + 20, 700, 1, 9304);

            var invasions = InvasionControl.Invasions;
            for (int i = 0; i < invasions.Count; i++)
            {
                int rowY = listY + 30 + (i * 25);
                if (rowY > 660) break;

                var inv    = invasions[i];
                bool active = DateTime.UtcNow >= inv.StartTime;

                AddLabel(30,  rowY, 0x481, inv.TownInvaded);
                AddLabel(180, rowY, 0x481, inv.TownChampionType.ToString());
                AddLabel(330, rowY, 0x481, inv.StartTime.ToString("MM/dd HH:mm"));

                string status = active
                    ? (inv.IsFinalStage
                        ? "<BASEFONT COLOR=#FF4500>CHAMPION</BASEFONT>"
                        : "<BASEFONT COLOR=#00FF00>ACTIVE</BASEFONT>")
                    : "<BASEFONT COLOR=#FFFF00>QUEUED</BASEFONT>";

                AddHtml(530, rowY, 120, 20, status, false, false);

                AddButton(660, rowY, 4017, 4018, 1000 + i, GumpButtonType.Reply, 0);
                AddLabel(695, rowY, 0x22, "STOP");
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (info.ButtonID == 1) // Schedule
            {
                InvasionTowns    town  = 0;
                TownMonsterType  mob   = 0;
                TownChampionType champ = 0;
                foreach (int sw in info.Switches)
                {
                    if (sw >= 100 && sw < 200) town = (InvasionTowns)(sw - 100);
                    else if (sw >= 200 && sw < 300) mob = (TownMonsterType)(sw - 200);
                    else if (sw >= 300 && sw < 400) champ = (TownChampionType)(sw - 300);
                }

                // Cap at 7 simultaneous invasions
                if (InvasionControl.Invasions.Count >= 7)
                {
                    from.SendMessage(0x22, "The maximum number of simultaneous invasions (7) has been reached.");
                    InvasionControl.RefreshGump(from);
                    return;
                }

                // Cooldown check — block scheduling if town is still cooling down
                if (InvasionControl.IsOnCooldown(town))
                {
                    TimeSpan rem = InvasionControl.GetCooldownRemaining(town);
                    from.SendMessage(0x22, String.Format(
                        "{0} is on cooldown. Available again in {1} minute(s).",
                        town, (int)rem.TotalMinutes + 1));
                    InvasionControl.RefreshGump(from);
                    return;
                }

                // Block scheduling if this town already has an active or queued invasion
                foreach (var existing in InvasionControl.Invasions)
                {
                    if (existing.InvasionTown == town)
                    {
                        from.SendMessage(0x22, String.Format(
                            "{0} already has an invasion scheduled or active.", town));
                        InvasionControl.RefreshGump(from);
                        return;
                    }
                }

                TextRelay tr = info.GetTextEntry(0);
                DateTime start;
                if (!DateTime.TryParseExact(tr.Text, "MM/dd/yyyy HH:mm", null,
                    System.Globalization.DateTimeStyles.None, out start))
                    start = DateTime.UtcNow;

                new TownInvasion(town, mob, champ, start);

                InvasionControl.RefreshAllOpenGumps();
                InvasionControl.RefreshGump(from);
            }
            else if (info.ButtonID >= 1000) // Stop
            {
                int idx = info.ButtonID - 1000;
                if (idx < InvasionControl.Invasions.Count)
                    InvasionControl.Invasions[idx].OnStop();

                InvasionControl.RefreshAllOpenGumps();
                InvasionControl.RefreshGump(from);
            }
        }
    }
}
