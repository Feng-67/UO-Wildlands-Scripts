/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;

namespace Server.Items
{
    public class BardUndeadDemonSlayer : Lute
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public BardUndeadDemonSlayer()
        {
            Name = "Bard's Undead & Demon Slayer";
            Hue = 0x59D;

            Slayer = SlayerName.Silver;
            Slayer2 = SlayerName.Exorcism;

            Quality = ItemQuality.Exceptional; 
        }

        public BardUndeadDemonSlayer(Serial serial)
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
