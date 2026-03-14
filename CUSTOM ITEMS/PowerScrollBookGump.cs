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
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PowerScrollBookGump : Gump
    {
        private const int Width = 560;
        private const int Height = 590;
        private const int RowsPerPage = 17;

        private static readonly double[] Tiers = { 105.0, 110.0, 115.0, 120.0 };
        private static readonly int[] TierCols = { 190, 280, 370, 460 };

        private readonly PowerScrollBook m_Book;
        private readonly Mobile m_From;
        private readonly int m_Page;
        private static readonly List<SkillName> AllSkills;
        private readonly Dictionary<(SkillName, double), int> m_Counts = new Dictionary<(SkillName, double), int>();

        static PowerScrollBookGump()
        {
            AllSkills = new List<SkillName>();

            // 1. Collect all unique skills from the book's dictionary
            if (PowerScrollBook._SkillInfo != null)
            {
                foreach (var list in PowerScrollBook._SkillInfo.Values)
                {
                    foreach (SkillName sn in list)
                    {
                        if (!AllSkills.Contains(sn))
                            AllSkills.Add(sn);
                    }
                }
            }

            // 2. Sort them A-Z based on how they actually look in the Gump
            AllSkills.Sort((x, y) => string.Compare(SkillLabel(x), SkillLabel(y)));
        }

        public PowerScrollBookGump(Mobile from, PowerScrollBook book, int page = 0) : base(50, 50)
        {
            m_From = from;
            m_Book = book;
            m_Page = page;

            BuildIndex();

            int totalPages = Math.Max(1, (int)Math.Ceiling(AllSkills.Count / (double)RowsPerPage));

            AddPage(0);
            AddBackground(0, 0, Width, Height, 9270);

            // Header - Style consistent with MyCommands
            AddHtml(0, 25, Width, 25, "<CENTER><BASEFONT COLOR=#FFD700>POWER SCROLL STORAGE</BASEFONT></CENTER>", false, false);
            AddImageTiled(20, 50, Width - 40, 2, 96);

            // Column Headers
            foreach (int i in new[] { 0, 1, 2, 3 })
                AddHtml(TierCols[i], 60, 80, 20, $"<CENTER><BASEFONT COLOR=#FFFFFF>{(int)Tiers[i]}</BASEFONT></CENTER>", false, false);

            int start = page * RowsPerPage;
            int end = Math.Min(start + RowsPerPage, AllSkills.Count);
            int y = 85;

            for (int si = start; si < end; si++)
            {
                SkillName skill = AllSkills[si];

                
                AddImageTiled(20, y + 22, Width - 40, 1, 96);

                // 2. SKILL NAME
                AddHtml(30, y, 200, 22, $"<BASEFONT COLOR=#FFD700>{SkillLabel(skill)}</BASEFONT>", false, false);

                for (int ti = 0; ti < Tiers.Length; ti++)
                {
                    double val = Tiers[ti];
                    int count = 0;
                    m_Counts.TryGetValue((skill, val), out count);

                    if (count > 0)
                    {
                        // Button (Option 2 - small marble)
                        AddButton(TierCols[ti] + 5, y + 5, 0x837, 0x838, 100 + (si * 4) + ti, GumpButtonType.Reply, 0);

                        // Number
                        AddHtml(TierCols[ti], y, 80, 22, $"<CENTER><BASEFONT COLOR=#00FF00>{count}</BASEFONT></CENTER>", false, false);
                    }
                    else
                    {
                        AddHtml(TierCols[ti], y, 80, 22, "<CENTER><BASEFONT COLOR=#444444>-</BASEFONT></CENTER>", false, false);
                    }
                }

                // Increment Y - added 2 extra pixels for padding between the line and the next skill
                y += 26;
            }

            // Navigation
            if (page > 0)
                AddButton(20, Height - 35, 4014, 4016, 2, GumpButtonType.Reply, 0);

            if (page + 1 < totalPages)
                AddButton(Width - 50, Height - 35, 4005, 4007, 3, GumpButtonType.Reply, 0);
        }

        private void BuildIndex()
        {
            if (m_Book.Entries == null) return;
            foreach (var item in m_Book.Entries)
            {
                if (item is PowerScroll ps)
                {
                    var key = (ps.Skill, ps.Value);
                    if (m_Counts.ContainsKey(key)) m_Counts[key]++;
                    else m_Counts[key] = 1;
                }
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            if (m_Book == null || m_Book.Deleted || info.ButtonID == 0) return;

            if (info.ButtonID == 2)
            {
                from.SendGump(new PowerScrollBookGump(from, m_Book, m_Page - 1));
            }
            else if (info.ButtonID == 3)
            {
                from.SendGump(new PowerScrollBookGump(from, m_Book, m_Page + 1));
            }
            else if (info.ButtonID >= 100)
            {
                int id = info.ButtonID - 100;
                int sIdx = id / 4;
                int tIdx = id % 4;

                if (sIdx >= 0 && sIdx < AllSkills.Count)
                {
                    SkillName sk = AllSkills[sIdx];
                    double val = Tiers[tIdx];

                    Item toRemove = m_Book.Entries.Find(x => x is PowerScroll ps && ps.Skill == sk && ps.Value == val);

                    if (toRemove != null)
                    {
                        if (from.Backpack != null && from.Backpack.TryDropItem(from, toRemove, false))
                        {
                            m_Book.Entries.Remove(toRemove);
                            from.SendMessage("You withdraw the scroll.");
                            m_Book.InvalidateProperties();
                        }
                        else
                        {
                            from.SendMessage("Your backpack is full.");
                        }
                    }

                    // Refresh
                    from.CloseGump(typeof(PowerScrollBookGump));
                    from.SendGump(new PowerScrollBookGump(from, m_Book, m_Page));
                }
            }
        }

        private static string SkillLabel(SkillName skill)
        {
            string s = skill.ToString();
            return System.Text.RegularExpressions.Regex.Replace(s, "(?<!^)([A-Z])", " $1");
        }
    }
}
