using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class IdolOfWildfire : Item
    {
        [Constructable]
		public IdolOfWildfire() : base( 0x1F18 )
		{
            Name = "Idol Of Wildfire";
			Weight = 1.0;
			Hue = 2758; //TODO: Verify hue!
		}

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);            
            list.Add("Inscribed By The Dark Monks");
        }

        public override void OnDoubleClick(Mobile m)
		{
			string text1 = "By my blood the Wildfire burns.<br/>By Wildfire the Path is lit.<br/>By the path the Guide returns.";
			string text2 = "Kal Vas Xen Corp";
			m.SendGump(new SimpleTextGump(m,this,text1,text2));
		}

        public IdolOfWildfire( Serial serial ) : base( serial )
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
