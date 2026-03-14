using System;
using Server.Gumps;

namespace Server.Mobiles
{
    public class AlaskanMalamuteStatue : Item, ICreatureStatuette
    {
        //public override int LabelNumber { get { return 1124685; } } // Windrunner

        public Type CreatureType { get { return typeof(AlaskanMalamute); } }

        [Constructable]
        public AlaskanMalamuteStatue() 
            : base(0xA76C)
        {
			Name = "Alaskan Malamute [Companion]";
            LootType = LootType.Blessed;
        }
        public AlaskanMalamuteStatue(Serial serial)
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
