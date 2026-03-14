/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;

namespace Server.Items
{
    public class BardDragonElementalSlayer : Lute
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public BardDragonElementalSlayer()
        {
            Name = "Bard's Dragon & Elemental Slayer";
            Hue = 0x851;

            Slayer = SlayerName.DragonSlaying;
            Slayer2 = SlayerName.ElementalBan;

            Quality = ItemQuality.Exceptional; 
        }

        public BardDragonElementalSlayer(Serial serial)
            : base(serial)
        {
        }

        

        public override int InitMinUses
        {
            get
            {
                return 500;
            }
        }
        public override int InitMaxUses
        {
            get
            {
                return 500;
            }
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
