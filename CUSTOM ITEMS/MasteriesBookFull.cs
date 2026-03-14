/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Authors Unknown)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MasteriesBookFull : BookOfMasteries
    {
        [Constructable]
        public MasteriesBookFull() : base((ulong)0x1FFFFFFFFFFF) // All 45 Mastery skills
        {
            Name = "Masteries Spellbook";
            LootType = LootType.Blessed;
        }

        public MasteriesBookFull(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
