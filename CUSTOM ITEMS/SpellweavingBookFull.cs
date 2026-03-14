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
    public class SpellweavingBookFull : SpellweavingBook
    {
        [Constructable]
        public SpellweavingBookFull() : base((ulong)0xFF3F) // Explicit cast to ulong fixes the ambiguity
        {
            Name = "Spellweaving Spellbook";
            LootType = LootType.Blessed;
        }

        public SpellweavingBookFull(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
