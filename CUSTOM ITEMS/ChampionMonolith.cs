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
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.CannedEvil; 

namespace Server.Items
{
    public class ChampionMonolith : Item
    {
        [Constructable]
        public ChampionMonolith() : base(41066) // 0xA06A sculpture_wood_skull
        {
            Name = "Champion Monolith";
            Movable = true;
        }

        public ChampionMonolith(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            // 1. Check if the item is locked down (AccessLevel check allows staff to test anywhere)
            if (!this.IsLockedDown && this.Movable && from.AccessLevel == AccessLevel.Player)
            {
                from.SendMessage("The Monolith's power can only be harnessed while it is locked down.");
                return;
            }

            // 2. Check range
            if (!from.InRange(GetWorldLocation(), 3))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            // 3. Open the Gump
            from.CloseGump(typeof(ChampionMonolithGump));
            from.SendGump(new ChampionMonolithGump(from));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ChampionMonolithGump : Gump
    {
        private Mobile m_From;
        private List<ChampionSpawn> m_Spawns;

        // The Master List of Coordinates
        private static readonly Dictionary<string, Point3D> m_TeleportMap = new Dictionary<string, Point3D>(StringComparer.OrdinalIgnoreCase)
        {
            { "ABYSSAL",                new Point3D(7051, 727, 32) },
            { "BEDLAM",                 new Point3D(153, 1610, 0) },
            { "CITY-OF-THE-DEAD",       new Point3D(5241, 3640, -1) },
            { "DAMWIN-THICKET",         new Point3D(5324, 3366, 0) },
            { "DECEIT",                 new Point3D(5155, 676, 0) },
            { "DESERT",                 new Point3D(5636, 2952, 17) },
            { "DESPISE",                new Point3D(5522, 837, 50) },
            { "DESTARD",                new Point3D(5212, 846, 0) },
            { "DRAGON-TURTLE-EODON",    new Point3D(473, 1720, 40) },
            { "DRAGON-TURTLE-FEL",      new Point3D(7067, 1916, 40) },
            { "FIRE",                   new Point3D(5820, 1388, 0) },
            { "HOPPERS-BOG",            new Point3D(5976, 3448, 3) },
            { "HUMILTY",                new Point3D(430, 926, -83) }, // Matches your spelling
            { "ICE-EAST",               new Point3D(5993, 2425, 26) },
            { "ICE-WEST",               new Point3D(5548, 2359, 19) },
            { "KHALDUN-DUNGEON-FEL",    new Point3D(6012, 3780, 18) },
            { "KHALDUN-DUNGEON-TRAM",   new Point3D(6012, 3780, 19) },
            { "KHALDUN-OVERLAND",       new Point3D(5949, 3886, 19) },
            { "MARBLE",                 new Point3D(5228, 3179, 9) },
            { "OAKS",                   new Point3D(5597, 3759, 28) },
            { "OAKS-ILSHENAR",          new Point3D(1687, 1104, 5) },
            { "OASIS",                  new Point3D(5559, 2669, -5) },
            { "PRIMEVAL-LICH",          new Point3D(6960, 1030, -15) },
            { "SLEEPING-DRAGON",        new Point3D(915, 409, 12) },
            { "TERA-KEEP",              new Point3D(5135, 1589, 0) },
            { "TERRA-SANCTUM",          new Point3D(6022, 2914, 16) },
            { "TORTOISE",               new Point3D(5747, 4015, 1) },
            { "TWISTED-WEALD",          new Point3D(1450, 1475, -26) },
            { "VALOR",                  new Point3D(338, 308, -54) },
            { "ORC-DUNGEON",            new Point3D(5294, 2031, 0) },
            { "HYTHLOTH",               new Point3D(6049, 48, 0) }
        };

        public ChampionMonolithGump(Mobile from) : base(100, 100)
        {
            m_From = from;
            m_Spawns = new List<ChampionSpawn>();

            foreach (Item item in World.Items.Values)
            {
                if (item is ChampionSpawn)
                    m_Spawns.Add((ChampionSpawn)item);
            }

            m_Spawns.Sort((a, b) => 
            {
                string nameA = a.Name ?? (a.Champion != null ? a.Champion.Name : "Unknown");
                string nameB = b.Name ?? (b.Champion != null ? b.Champion.Name : "Unknown");
                return nameA.CompareTo(nameB);
            });

            int entryHeight = 25;
            int totalHeight = 100 + (m_Spawns.Count * entryHeight) + 20;
            int width = 380; 

            AddPage(0);
            AddBackground(0, 0, width, totalHeight, 9270);
            AddAlphaRegion(10, 10, width - 20, totalHeight - 20);
            AddHtml(10, 20, width - 20, 25, "<CENTER><BASEFONT COLOR=#FFD700>CHAMPION MONOLITH</BASEFONT></CENTER>", false, false);
            AddImageTiled(20, 45, width - 40, 2, 96);

            AddLabel(55, 55, 0x35, "REGION");
            AddLabel(240, 55, 0x35, "STATUS");

            int y = 80;

            for (int i = 0; i < m_Spawns.Count; i++)
            {
                ChampionSpawn spawn = m_Spawns[i];
                string name = spawn.Name ?? "Unknown Altar";
                
                // Truncate for display if excessively long
                string displayName = name;
                if (displayName.Length > 20) displayName = displayName.Substring(0, 18) + "..";

                string statusText;
                int hue;

                if (spawn.Active)
                {
                    statusText = "ACTIVE";
                    hue = 63; // Green
                }
                else if (DateTime.UtcNow >= spawn.RestartTime)
                {
                    statusText = "READY";
                    hue = 0x481; // White/Gold
                }
                else
                {
                    TimeSpan ts = spawn.RestartTime - DateTime.UtcNow;
                    statusText = String.Format("COOLDOWN ({0}m)", (int)ts.TotalMinutes);
                    hue = 2401; // Grey
                }

                AddButton(25, y + 3, 2224, 2224, i + 1, GumpButtonType.Reply, 0);
                AddLabel(55, y, hue, displayName);
                AddLabel(240, y, hue, statusText);

                y += entryHeight;
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (info.ButtonID <= 0)
                return;

            int index = info.ButtonID - 1;

            if (index >= 0 && index < m_Spawns.Count)
            {
                ChampionSpawn target = m_Spawns[index];

                if (target != null && !target.Deleted)
                {
                    Map map = target.Map;
                    string name = target.Name;
                    Point3D destination;

                    // 1. Check if we have a hardcoded spot for this specific name
                    if (name != null && m_TeleportMap.ContainsKey(name))
                    {
                        destination = m_TeleportMap[name];
                    }
                    else
                    {
                        // 2. Fallback: Use "Safe Radius" if name is not in our list
                        Point3D loc = target.Location;
                        destination = new Point3D(loc.X + 10, loc.Y + 10, map.GetAverageZ(loc.X + 10, loc.Y + 10));
                        from.SendMessage(33, "Warning: Precision coordinates not found. Using approximate location.");
                    }

                    from.MoveToWorld(destination, map);
                    from.SendMessage(0x22, "You travel to the {0} region.", name ?? "Champion");
                    from.PlaySound(0x1FE); 
                }
            }
        }
    }
}
