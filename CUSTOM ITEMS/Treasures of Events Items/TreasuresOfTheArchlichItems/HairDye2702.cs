using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    public class HairDye2702 : Item
    {
        [Constructable]
        public HairDye2702()
            : base(0xEFF)
        {
            Hue = 2702;
        }

        public HairDye2702(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1041088; // Hair Dye
        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                if (from is PlayerMobile)
                    BaseGump.SendGump(new HairDyeConfirmGump(from as PlayerMobile, Hue, this));
            }
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
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