using System;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Accounting;
using Server.Engines.VeteranRewards;

namespace Server.Items
{
	[Flipable( 0xA177, 0xA177 )]
	public class LuckyAbominationSkull : TenthAnniversarySculpture
	{

        [Constructable]
		public LuckyAbominationSkull() : base()
		{
            Name = "Lucky Abomination Skull";
            ItemID = 0xA177;
			Hue = 2500;
            AddSculpture(this);
        }

       
        public LuckyAbominationSkull(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
	}
}
