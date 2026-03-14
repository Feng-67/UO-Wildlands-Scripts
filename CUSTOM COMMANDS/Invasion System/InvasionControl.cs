/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Ravenwolfe)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using System.IO;
using Server.Commands;
using Server.Network;

namespace Server.Customs.Invasion_System
{
    public static class InvasionControl
    {
        public static List<TownInvasion> Invasions = new List<TownInvasion>();

        public static void Initialize()
        {
            // We can keep this command as a shortcut, but it now opens the Master Gump
            CommandSystem.Register("ListInvasions", AccessLevel.Administrator, (e) => {
                InvasionMasterGump.RefreshGump(e.Mobile);
            });
        }

        // This is the magic "Live Update" method
        public static void RefreshAllOpenGumps()
        {
            foreach (NetState state in NetState.Instances)
            {
                Mobile m = state.Mobile;
                if (m != null && m.HasGump(typeof(InvasionMasterGump)))
                {
                    InvasionMasterGump.RefreshGump(m);
                }
            }
        }
    }

    public class InvasionPersistence
    {
        private static string FilePath = Path.Combine("Saves", "Invasions", "Persistence.bin");

        public static void Configure()
        {
            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        private static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(FilePath, writer =>
            {
                writer.Write(0); // version
                writer.Write(InvasionControl.Invasions.Count);
                foreach (var m in InvasionControl.Invasions) m.Serialize(writer);
            });
        }

        private static void OnLoad()
        {
            Persistence.Deserialize(FilePath, reader =>
            {
                var version = reader.ReadInt();
                if (version == 0)
                {
                    var count = reader.ReadInt();
                    for (var i = 0; i < count; ++i)
                    {
                        var invasion = new TownInvasion(reader);
                        InvasionControl.Invasions.Add(invasion);
                    }
                }
            });
        }
    }
}
