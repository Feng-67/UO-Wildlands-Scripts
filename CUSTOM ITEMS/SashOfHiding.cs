/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;

namespace Server.Items
{
    public class SashOfHiding : BodySash
    {
        [Constructable]
        public SashOfHiding()
        {
            Name = "Sash of Hiding";
            Hue = 21845; // Transparent hue from your shroud
            LootType = LootType.Blessed; // Blessed as requested

            // Grants +100 Hiding
            SkillBonuses.SetValues(0, SkillName.Hiding, 100.0);
        }

        public SashOfHiding(Serial serial) : base(serial)
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
