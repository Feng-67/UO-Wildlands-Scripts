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
    public class RunicAtlasExceptional : RunicAtlas
    {
        [Constructable]
        public RunicAtlasExceptional() : base() 
        {
            // Set the name as requested
            Name = "Runic Atlas";
            
            // Set the capacity and current charges
            this.MaxCharges = 100;
            this.CurCharges = 100;
            
            // Apply Exceptional quality
            this.Quality = BookQuality.Exceptional;
            
            // Standard practice for high-end travel books
            this.LootType = LootType.Blessed;
        }

        public RunicAtlasExceptional(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
