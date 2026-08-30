using Server.Engines.Craft;
using System;
using System.Collections.Generic;

namespace Server.Items
{
    [Furniture]
    public class KeysToYukiosHoldingCell : FurnitureContainer
    {
        public override int DefaultGumpID{ get{ return 0x9; } }

        [Constructable]
        public KeysToYukiosHoldingCell()
            : base(0xA0C4)
        {
            Name = "Keys To Yukio's Holding Cell";
            Weight = 1.0;
        }

        public KeysToYukiosHoldingCell(Serial serial)
            : base(serial)
        {
        }
  
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = (InheritsItem ? 0 : reader.ReadInt()); // Required for FurnitureContainer insertion
        }
    }
}