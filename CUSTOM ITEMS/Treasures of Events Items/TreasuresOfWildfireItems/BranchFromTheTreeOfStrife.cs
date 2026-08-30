using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;


namespace Server.Items
{
    public class BranchFromTheTreeOfStrife : Item
    {
        [Constructable]
		public BranchFromTheTreeOfStrife() : base( 0x0D3B )
		{
            Name = "Branch From The Tree Of Strife";
			Weight = 1.0;
			Hue = 2707; //TODO: Verify hue!
		}

		public override void OnDoubleClick(Mobile m)
		{
			string text1 = "Ilshen's folk believed the world was helf by three serpents. Great Mistas died during the Shattering.";
			string text2 = "Ord and Rel became tangled in eternal conflict. Now the world sites in the branches of the Tree of Strife forever.";
			m.SendGump(new SimpleTextGump(m,this,text1,text2));
		}

        public BranchFromTheTreeOfStrife( Serial serial ) : base( serial )
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
