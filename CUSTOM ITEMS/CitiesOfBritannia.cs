/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Items
{
    public class CitiesOfBritannia : RunicAtlas
    {
        [Constructable]
        public CitiesOfBritannia() : base() 
        {
            Name = "Cities of Britannia (T & F)";
            Hue = 1154; 
            
            this.MaxCharges = 100;
            this.CurCharges = 100;
            this.Quality = BookQuality.Exceptional; 
            this.LootType = LootType.Blessed;

            // --- TRAMMEL CITIES ---
            AddCity("(T) Britain", new Point3D(1496, 1628, 10), Map.Trammel);
            AddCity("(T) Moonglow", new Point3D(4408, 1168, 0), Map.Trammel);
            AddCity("(T) Trinsic", new Point3D(1845, 2745, 0), Map.Trammel);
            AddCity("(T) Skara Brae", new Point3D(591, 2139, 0), Map.Trammel);
            AddCity("(T) Yew", new Point3D(546, 991, 0), Map.Trammel);
            AddCity("(T) Minoc", new Point3D(2511, 572, 0), Map.Trammel);
            AddCity("(T) Vesper", new Point3D(2895, 676, 0), Map.Trammel);
            AddCity("(T) Jhelom", new Point3D(1378, 3817, 0), Map.Trammel);
            AddCity("(T) Magincia", new Point3D(3734, 2222, 20), Map.Trammel);
            AddCity("(T) Nujel'm", new Point3D(3770, 1308, 0), Map.Trammel);
            AddCity("(T) Buccaneer's Den", new Point3D(2713, 2154, 0), Map.Trammel);
            AddCity("(T) Serpent's Hold", new Point3D(2888, 3474, 15), Map.Trammel);
            AddCity("(T) New Haven", new Point3D(3503, 2574, 14), Map.Trammel);

            // --- FELUCCA CITIES ---
            AddCity("(F) Britain", new Point3D(1496, 1628, 10), Map.Felucca);
            AddCity("(F) Moonglow", new Point3D(4408, 1168, 0), Map.Felucca);
            AddCity("(F) Trinsic", new Point3D(1845, 2745, 0), Map.Felucca);
            AddCity("(F) Skara Brae", new Point3D(591, 2139, 0), Map.Felucca);
            AddCity("(F) Yew", new Point3D(546, 991, 0), Map.Felucca);
            AddCity("(F) Minoc", new Point3D(2511, 572, 0), Map.Felucca);
            AddCity("(F) Vesper", new Point3D(2895, 676, 0), Map.Felucca);
            AddCity("(F) Jhelom", new Point3D(1378, 3817, 0), Map.Felucca);
            AddCity("(F) Magincia", new Point3D(3734, 2222, 20), Map.Felucca);
            AddCity("(F) Nujel'm", new Point3D(3770, 1308, 0), Map.Felucca);
            AddCity("(F) Buccaneer's Den", new Point3D(2713, 2154, 0), Map.Felucca);
            AddCity("(F) Serpent's Hold", new Point3D(2888, 3474, 15), Map.Felucca);

            // --- TOKUNO ---
            AddCity("Zento", new Point3D(739, 1255, 30), Map.Tokuno);
        }

        private void AddCity(string cityName, Point3D loc, Map map)
        {
            if (this.Entries != null && this.Entries.Count < this.MaxEntries)
            {
                // FIX: Using RunebookEntry which is the most common base type for travel books
                this.Entries.Add(new RunebookEntry(loc, map, cityName, null));
            }
        }

        public CitiesOfBritannia(Serial serial) : base(serial)
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
