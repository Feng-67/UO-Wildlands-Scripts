using System;
using Server.Gumps;

namespace Server.Mobiles
{
    public class RottweilerStatue : Item, ICreatureStatuette
    {
        //public override int LabelNumber { get { return 1124685; } } // Windrunner

        public Type CreatureType { get { return typeof(Rottweiler); } }

        [Constructable]
        public RottweilerStatue() 
            : base(0xA770)
        {
			Name = "Rottweiler [Companion]";
            LootType = LootType.Blessed;
        }
        public RottweilerStatue(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
                from.SendGump(new ConfirmMountStatuetteGump(this));
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
