/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    // --- HUMAN / ELF ITEMS ---

    public class StoreValoriteCloak : Cloak
    {
        [Constructable]
        public StoreValoriteCloak()
        {
            Name = "A Cloak hued in Valorite";
            Hue = 2210; // Valorite Hue
            LootType = LootType.Blessed;
        }
        public StoreValoriteCloak(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class StoreValoriteRobe : Robe
    {
        [Constructable]
        public StoreValoriteRobe()
        {
            Name = "A Robe hued in Valorite";
            Hue = 2210;
            LootType = LootType.Blessed;
        }
        public StoreValoriteRobe(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class StoreValoriteDress : PlainDress
    {
        [Constructable]
        public StoreValoriteDress()
        {
            Name = "A Dress hued in Valorite";
            Hue = 2210;
            LootType = LootType.Blessed;
        }
        public StoreValoriteDress(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // --- GARGOYLE ITEMS ---

    public class StoreGargishValoriteRobe : GargishRobe
    {
        [Constructable]
        public StoreGargishValoriteRobe()
        {
            Name = "A Gargish Robe hued in Valorite";
            Hue = 2210;
            LootType = LootType.Blessed;
        }
        public StoreGargishValoriteRobe(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class StoreGargishValoriteFancyRobe : GargishFancyRobe
    {
        [Constructable]
        public StoreGargishValoriteFancyRobe()
        {
            Name = "A Gargish Fancy Robe hued in Valorite";
            Hue = 2210;
            LootType = LootType.Blessed;
        }
        public StoreGargishValoriteFancyRobe(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
