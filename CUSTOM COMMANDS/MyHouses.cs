/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Tresdni)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Multis;
using Server.Targeting;
using Server.Accounting;
using Server.Commands;
using Server.Spells;
using Server.Network; // Added to resolve NetState error

namespace Server.Gumps
{
    public class MyHousesGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("MyHouses", AccessLevel.Player, new CommandEventHandler(ViewHouses_OnCommand));
        }

        [Usage("MyHouses")]
        [Description("Displays a menu listing all houses of the account.")]
        public static void ViewHouses_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new MyHousesGump(e.Mobile, GetMyHouses(e.Mobile), null));
        }

        public static List<BaseHouse> GetMyHouses(Mobile owner)
        {
            List<BaseHouse> list = new List<BaseHouse>();
            Account acct = owner.Account as Account;

            if (acct == null)
            {
                list.AddRange(BaseHouse.GetHouses(owner));
            }
            else
            {
                for (int i = 0; i < acct.Length; ++i)
                {
                    Mobile mob = acct[i];
                    if (mob != null)
                        list.AddRange(BaseHouse.GetHouses(mob));
                }
            }

            list.Sort(MyHouseComparer.Instance);
            return list;
        }

        private Mobile m_From;
        private List<BaseHouse> m_List;
        private BaseHouse m_Selection;

        public MyHousesGump(Mobile from, List<BaseHouse> list, BaseHouse sel) : base(50, 40)
        {
            m_From = from;
            m_List = list;
            m_Selection = sel;

            from.CloseGump(typeof(MyHousesGump));

            AddPage(0);
            AddBackground(0, 0, 420, 500, 5054);
            AddBlackAlpha(10, 10, 400, 480);

            if (sel == null || sel.Deleted)
            {
                m_Selection = null;
                AddHtml(20, 20, 380, 25, Color(Center("My House Listings"), White), false, false);

                if (list.Count == 0)
                    AddHtml(20, 60, 380, 40, Color(Center("You have no houses in the world."), White), false, false);

                int page = 0;
                for (int i = 0; i < list.Count; ++i)
                {
                    if ((i % 15) == 0)
                    {
                        if (page > 0)
                            AddButton(320, 460, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page);

                        AddPage(++page);

                        if (i + 15 < list.Count)
                            AddButton(360, 460, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 1);
                    }

                    object name = FindMyHouseName(list[i]);
                    int yOffset = 60 + ((i % 15) * 25);

                    AddHtml(30, yOffset, 30, 20, Color(String.Format("{0}.", i + 1), White), false, false);

                    if (name is int)
                        AddHtmlLocalized(60, yOffset, 280, 20, (int)name, White16, false, false);
                    else if (name is string)
                        AddHtml(60, yOffset, 280, 20, Color((string)name, White), false, false);

                    AddButton(360, yOffset - 1, 4005, 4007, i + 1, GumpButtonType.Reply, 0);
                }
            }
            else
            {
                RenderHouseDetails(sel);
            }
        }

        private void RenderHouseDetails(BaseHouse sel)
        {
            string houseName = (sel.Sign == null) ? "An Unnamed House" : sel.Sign.GetName();
            string owner = (sel.Owner == null) ? "nobody" : sel.Owner.Name;
            Map map = sel.Map;

            AddHtml(10, 20, 400, 25, Color(Center("House Properties"), White), false, false);

            int startY = 60;
            int stepY = 25;

            string[] labels = { "Facet:", "Location:", "Owner:", "Name:", "Friends:", "Co-Owners:", "Bans:", "Decays:", "Decay Level:" };
            string[] values = {
                map == null ? "(null)" : map.Name,
                sel.Location.ToString(),
                owner,
                houseName,
                sel.Friends.Count.ToString(),
                sel.CoOwners.Count.ToString(),
                sel.Bans.Count.ToString(),
                sel.CanDecay ? "Yes" : "No",
                sel.DecayLevel.ToString()
            };

            for (int i = 0; i < labels.Length; i++)
            {
                AddHtml(30, startY + (i * stepY), 150, 20, Color(labels[i], White), false, false);
                AddHtml(180, startY + (i * stepY), 210, 20, Color(Right(values[i]), White), false, false);
            }

            AddButton(30, 350, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtml(65, 350, 300, 20, Color("Go to this house", White), false, false);

            AddButton(30, 385, 4005, 4007, 3, GumpButtonType.Reply, 0);
            AddHtml(65, 385, 300, 20, Color("Demolish house", White), false, false);

            AddButton(30, 420, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddHtml(65, 420, 300, 20, Color("Refresh house", White), false, false);

            AddButton(30, 455, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddHtml(65, 455, 300, 20, Color("Back to list", White), false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Selection == null)
            {
                int v = info.ButtonID - 1;

                if (v >= 0 && v < m_List.Count)
                    m_From.SendGump(new MyHousesGump(m_From, m_List, m_List[v]));
            }
            else if (!m_Selection.Deleted)
            {
                switch (info.ButtonID)
                {
                    case 0:
                        {
                            m_From.SendGump(new MyHousesGump(m_From, m_List, null));
                            break;
                        }
                    case 1:
                        {
                            Map map = m_Selection.Map;

                            if (m_From.Region is Regions.Jail)
                            {
                                m_From.SendMessage("You cannot escape jail so easily foolish one.");
                                m_From.SendGump(new MyHousesGump(m_From, m_List, m_Selection));
                                return;
                            }
                            if (SpellHelper.CheckCombat(m_From) || m_From.Combatant != null)
                            {
                                m_From.SendMessage("Wouldst thou flee during the heat of battle?");
                                m_From.SendGump(new MyHousesGump(m_From, m_List, m_Selection));
                                return;
                            }
                            if (m_From.Criminal)
                            {
                                m_From.SendMessage("A criminal may not escape so easily.");
                                m_From.SendGump(new MyHousesGump(m_From, m_List, m_Selection));
                                return;
                            }

                            m_From.MoveToWorld(m_Selection.BanLocation, map);
                            m_From.SendGump(new MyHousesGump(m_From, m_List, m_Selection));
                            break;
                        }
                    case 3:
                        {
                            m_From.SendGump(new MyHousesGump(m_From, m_List, m_Selection));
                            m_From.SendGump(new HouseDemolishGump(m_From, m_Selection));
                            break;
                        }
                    case 4:
                        {
                            m_Selection.RefreshDecay();
                            m_From.SendGump(new MyHousesGump(m_From, m_List, m_Selection));
                            break;
                        }
                }
            }
        }

        public object FindMyHouseName(BaseHouse house)
        {
            int multiID = house.ItemID & 0x3FFF;
            HousePlacementEntry[][] allEntries = { HousePlacementEntry.ClassicHouses, HousePlacementEntry.TwoStoryFoundations, HousePlacementEntry.ThreeStoryFoundations };

            foreach (var entries in allEntries)
            {
                for (int i = 0; i < entries.Length; ++i)
                    if (entries[i].MultiID == multiID) return entries[i].Description;
            }
            return house.GetType().Name;
        }

        private const int White16 = 0x7FFF;
        private const int White = 0xFFFFFF;

        public string Right(string text) => String.Format("<div align=right>{0}</div>", text);
        public string Center(string text) => String.Format("<CENTER>{0}</CENTER>", text);
        public string Color(string text, int color) => String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);

        public void AddBlackAlpha(int x, int y, int width, int height)
        {
            AddImageTiled(x, y, width, height, 2624);
            AddAlphaRegion(x, y, width, height);
        }

        private class MyHouseComparer : IComparer<BaseHouse>
        {
            public static readonly IComparer<BaseHouse> Instance = new MyHouseComparer();
            public int Compare(BaseHouse x, BaseHouse y) => x.BuiltOn.CompareTo(y.BuiltOn);
        }
    }
}
