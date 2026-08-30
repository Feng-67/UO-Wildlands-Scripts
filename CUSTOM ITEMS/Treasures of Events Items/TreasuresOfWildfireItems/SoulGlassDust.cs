using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class SoulGlassDust : Item
    {
        [Constructable]
		public SoulGlassDust() : base( 0x3725 )
		{
            Name = "Soul Glass Dust";
			Weight = 1.0;
			Hue = 1195; //TODO: Verify hue!		
		}

        public override void OnDoubleClick(Mobile m)
		{
			string text1 = "This dust remains from the work Yukio did for the Dark Monks under duress. They wished to bind a \"Great Soul.\"";
			string text2 = "The glass shards sometimes form the outline of a man bound by chains.";
			m.SendGump(new SimpleTextGump(m,this,text1,text2));
		}

        public SoulGlassDust( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

    }
}
