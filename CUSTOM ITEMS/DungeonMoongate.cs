/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
#region References
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Server.Items
{
    public class DungeonMoongate : Item
    {
        public static List<DungeonMoongate> Moongates { get; private set; }

        static DungeonMoongate()
        {
            Moongates = new List<DungeonMoongate>();
        }

        public static void Initialize()
        {
            CommandSystem.Register("DungeonMoonGen", AccessLevel.Administrator, DungeonMoonGen_OnCommand);
            CommandSystem.Register("DungeonMoonGenDelete", AccessLevel.Administrator, DungeonMoonGenDelete_OnCommand);
        }

        [Usage("DungeonMoonGen")]
        [Description("Generates dungeon moongates. Removes all old dungeon moongates.")]
        public static void DungeonMoonGen_OnCommand(CommandEventArgs e)
        {
            DeleteAll();

            int count = 0;

            // Simplified generation logic
            count += MoonGen(DMList.Trammel);
            count += MoonGen(DMList.Felucca);
            count += MoonGen(DMList.Ilshenar);
            count += MoonGen(DMList.Malas);
            count += MoonGen(DMList.Tokuno);
            count += MoonGen(DMList.TerMur);

            World.Broadcast(0x35, true, "{0} dungeon moongates generated.", count);
        }

        private static int MoonGen(DMList list)
        {
            if (list == null) return 0;

            foreach (DMEntry entry in list.Entries)
            {
                DungeonMoongate o = new DungeonMoongate();
                o.MoveToWorld(entry.Location, list.Map);
            }

            return list.Entries.Length;
        }

        [Usage("DungeonMoonGenDelete")]
        [Description("Removes all dungeon moongates.")]
        public static void DungeonMoonGenDelete_OnCommand(CommandEventArgs e)
        {
            DeleteAll();
        }

        public static void DeleteAll()
        {
            int index = Moongates.Count;

            while (--index >= 0)
            {
                if (index < Moongates.Count)
                    Moongates[index].Delete();
            }

            Moongates.Clear();
        }

        public override int LabelNumber => 1023952; // Blue Moongate
        public override bool HandlesOnMovement => true;
        public override bool ForceShowProperties => true;

        [Constructable]
        public DungeonMoongate() : base(0xF6C)
        {
            Name = "Dungeon Moongate";
            Hue = 126; // Custom hue to differentiate from Public gates
            Movable = false;
            Light = LightType.Circle300;
            Moongates.Add(this);
        }

        public DungeonMoongate(Serial serial) : base(serial)
        {
            Moongates.Add(this);
        }

        public override void OnDelete()
        {
            base.OnDelete();
            Moongates.Remove(this);
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (m.InRange(GetWorldLocation(), 1))
            {
                UseGate(m);
            }
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player && m.CanSee(this))
            {
                UseGate(m);
            }

            return m.Map == Map && m.InRange(this, 1);
        }

        public virtual bool CanUseGate(Mobile m)
        {
            if (m.IsStaff()) return true;

            if (m.Criminal)
            {
                m.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal...
                return false;
            }

            if (SpellHelper.CheckCombat(m))
            {
                m.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee...
                return false;
            }

            if (m.Spell != null)
            {
                m.SendLocalizedMessage(1049616); // You are too busy...
                return false;
            }

            return true;
        }

        public bool UseGate(Mobile m)
        {
            if (!CanUseGate(m))
            {
                return false;
            }

            m.CloseGump(typeof(DMoongateGump));
            m.SendGump(new DMoongateGump(m, this));
            
            // Play the "Open Gate" sound
            Effects.PlaySound(m.Location, m.Map, 0x20E); 

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class DMEntry
    {
        public Point3D Location { get; private set; }
        public Map Map { get; private set; }
        public TextDefinition Number { get; private set; }

        public DMEntry(Point3D loc, TextDefinition number)
        {
            Location = loc;
            Number = number;
        }
    }

    public class DMList
    {
        public static readonly DMList Trammel = new DMList(
            1012000, 1012012, Map.Trammel, new[]
            {
                new DMEntry(new Point3D(2499, 919, 0), "Covetous"),
                new DMEntry(new Point3D(4111, 432, 5), "Deceit"),
                new DMEntry(new Point3D(1298, 1080, 0), "Despise"),
                new DMEntry(new Point3D(1176, 2637, 0), "Destard"),
                new DMEntry(new Point3D(4721, 3822, 0), "Hythloth"),
                new DMEntry(new Point3D(514, 1561, 0), "Shame"),
                new DMEntry(new Point3D(2043, 238, 10), "Wrong"),
                new DMEntry(new Point3D(5426, 3120, -60), "Terathan Keep"),
                new DMEntry(new Point3D(5760, 2908, 15), "Fire"),
                new DMEntry(new Point3D(5210, 2322, 30), "Ice"),
                new DMEntry(new Point3D(1716, 2993, 0), "Painted Caves"),
                new DMEntry(new Point3D(5576, 3018, 25), "Palace of Paroxysmus"),
                new DMEntry(new Point3D(3784, 1097, 14), "Prism of Light"),
                new DMEntry(new Point3D(764, 1646, 0), "Sanctuary"),
                new DMEntry(new Point3D(586, 1643, -5), "Blighted Grove"),
                new DMEntry(new Point3D(1016, 1434, 0), "Orc Dungeon"),
		new DMEntry(new Point3D(1482, 1474, 0), "Blackthorns Dungeon"),
		new DMEntry(new Point3D(6038 , 3796, 0), "Khaldun Dungeon")
            });

        public static readonly DMList Felucca = new DMList(
            1012001, 1012013, Map.Felucca, new[]
            {
                new DMEntry(new Point3D(2499, 919, 0), "Covetous"),
                new DMEntry(new Point3D(4111, 432, 5), "Deceit"),
                new DMEntry(new Point3D(1298, 1080, 0), "Despise"),
                new DMEntry(new Point3D(1176, 2637, 0), "Destard"),
                new DMEntry(new Point3D(4721, 3822, 0), "Hythloth"),
                new DMEntry(new Point3D(514, 1561, 0), "Shame"),
                new DMEntry(new Point3D(2043, 238, 10), "Wrong"),
                new DMEntry(new Point3D(5426, 3120, -60), "Terathan Keep"),
                new DMEntry(new Point3D(5760, 2908, 15), "Fire"),
                new DMEntry(new Point3D(5210, 2322, 30), "Ice"),
                new DMEntry(new Point3D(1716, 2993, 0), "Painted Caves"),
                new DMEntry(new Point3D(5576, 3018, 25), "Palace of Paroxysmus"),
                new DMEntry(new Point3D(3784, 1097, 14), "Prism of Light"),
                new DMEntry(new Point3D(764, 1646, 0), "Sanctuary"),
                new DMEntry(new Point3D(586, 1643, -5), "Blighted Grove"),
		new DMEntry(new Point3D(1016, 1434, 0), "Orc Dungeon"),
                new DMEntry(new Point3D(1526, 1419, 15), "Blackthorns Dungeon"),
		new DMEntry(new Point3D(6038 , 3796, 0), "Khaldun Dungeon")
            });

        public static readonly DMList Ilshenar = new DMList(
            1012002, 1012014, Map.Ilshenar, new[]
            {
                new DMEntry(new Point3D(1746, 1193, -1), "Blood Dungeon"),
                new DMEntry(new Point3D(555, 463, -25), "Sorcerer's Dungeon"),
                new DMEntry(new Point3D(832, 778, -80), "Exodus Dungeon"),
                new DMEntry(new Point3D(1420, 911, -12), "Spider Cave"),
                new DMEntry(new Point3D(1363, 1033, -13), "Spectre Dungeon"),
                new DMEntry(new Point3D(1788, 574, 72), "Rock Dungeon"),
                new DMEntry(new Point3D(651, 1303, -60), "Wisp Dungeon"),
                new DMEntry(new Point3D(576, 1153, -114), "Ankh Dungeon"),
		new DMEntry(new Point3D(1450, 1476, -29), "Twisted Weald"),
		new DMEntry(new Point3D(940, 499, -30), "Volcanic Lair"),
		new DMEntry(new Point3D(1032, 1154, -24), "Ratman Cave")
            });

        public static readonly DMList Malas = new DMList(
            1060643, 1062039, Map.Malas, new[]
            {
                new DMEntry(new Point3D(2367, 1268, -85), "Doom"),
		new DMEntry(new Point3D(1732, 975, -75), "Labyrinth"),
		new DMEntry(new Point3D(2067, 1375, -75), "Bedlam")
            });

        public static readonly DMList Tokuno = new DMList(
            1063258, 1063415, Map.Tokuno, new[]
            {
                new DMEntry(new Point3D(977, 218, 23), "Fan Dancer's Dojo"),
		new DMEntry(new Point3D(1346, 763, 20), "The Citadel"),
                new DMEntry(new Point3D(259, 785, 64), "Yomotsu Mines")
            });

        public static readonly DMList TerMur = new DMList(
            1113602, 1113604, Map.TerMur, new[]
            {
                new DMEntry(new Point3D(498, 2186, 45), "Shadowguard"),
		new DMEntry(new Point3D(997, 3848, -41), "Tomb of Kings"),
		new DMEntry(new Point3D(1128, 1208, 0), "The Underworld")
            });

        public static readonly DMList[] Lists = { Trammel, Felucca, Ilshenar, Malas, Tokuno, TerMur };
        public Map Map { get; private set; }
        public TextDefinition Number { get; private set; }
        public TextDefinition SelNumber { get; private set; }
        public DMEntry[] Entries { get; private set; }

        public DMList(TextDefinition number, TextDefinition selNumber, Map map, DMEntry[] entries)
        {
            Number = number;
            SelNumber = selNumber;
            Map = map;
            Entries = entries;
        }
    }

    public class DMoongateGump : Gump
    {
        private readonly Mobile m_Mobile;
        private readonly Item m_Moongate;
        private readonly DMList[] m_Lists;

        public DMoongateGump(Mobile mobile, Item moongate) : base(100, 100)
        {
            m_Mobile = mobile;
            m_Moongate = moongate;
            m_Lists = DMList.Lists;

            AddPage(0);

            // Standard Stone Background (5054)
            AddBackground(0, 0, 380, 495, 5054);

            // OK / Cancel Buttons at bottom
            AddButton(10, 210, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(45, 210, 140, 25, 1011036, false, false); // OKAY
            
            AddButton(10, 235, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(45, 235, 425, 25, 1011012, false, false); // CANCEL
            
            AddHtmlLocalized(25, 5, 200, 20, 1012011, false, false); // Pick your destination:

            // Left Side: Facet Buttons (Trammel, Felucca, etc.)
            for (int i = 0; i < m_Lists.Length; ++i)
            {
                // Note: Page ID is i + 1
                AddButton(25, 35 + (i * 25), 2117, 2118, 0, GumpButtonType.Page, i + 1);

                if (m_Lists[i].Number.Number > 0)
                    AddHtmlLocalized(43, 35 + (i * 25), 150, 20, m_Lists[i].Number.Number, false, false);
                else
                    AddHtml(43, 35 + (i * 25), 150, 20, m_Lists[i].Number.String, false, false);
            }

            // Render content pages for each Facet
            for (int i = 0; i < m_Lists.Length; ++i)
            {
                RenderPage(i);
            }
        }

        private void RenderPage(int index)
        {
            AddPage(index + 1);
            DMList list = m_Lists[index];

            // Right Side: List Header (e.g. "Trammel Dungeons")
            if (list.SelNumber.Number > 0)
                AddHtmlLocalized(43, 35 + (index * 25), 150, 20, list.SelNumber.Number, false, false);
            else
                AddHtml(43, 35 + (index * 25), 150, 20, list.SelNumber.String, false, false);

            // Right Side: Radio Buttons for Dungeons
            for (int i = 0; i < list.Entries.Length; ++i)
            {
                // Switch ID logic: (ListIndex * 100) + EntryIndex
                AddRadio(200, 35 + (i * 25), 210, 211, false, (index * 100) + i);

                if (list.Entries[i].Number.Number > 0)
                    AddHtmlLocalized(225, 35 + (i * 25), 150, 20, list.Entries[i].Number.Number, false, false);
                else
                    AddHtml(225, 35 + (i * 25), 150, 20, list.Entries[i].Number.String, false, false);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            // ButtonID 0 is Cancel
            if (info.ButtonID == 0) return;

            // Retrieve the active radio switch
            int[] switches = info.Switches;

            if (switches.Length == 0) return;

            int switchID = switches[0];
            int listIdx = switchID / 100;
            int entryIdx = switchID % 100;

            if (listIdx >= 0 && listIdx < m_Lists.Length && entryIdx >= 0 && entryIdx < m_Lists[listIdx].Entries.Length)
            {
                DMEntry entry = m_Lists[listIdx].Entries[entryIdx];
                Map map = m_Lists[listIdx].Map;

                if (m_Mobile.Map == map && m_Mobile.InRange(entry.Location, 1))
                {
                    m_Mobile.SendLocalizedMessage(1019003); // You are already there.
                    return;
                }

                BaseCreature.TeleportPets(m_Mobile, entry.Location, map);
                m_Mobile.MoveToWorld(entry.Location, map);
                
                // Play Teleport Sound
                Effects.PlaySound(entry.Location, map, 0x1FE);
            }
        }
    }
}
