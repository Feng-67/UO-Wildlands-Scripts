using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{

	public class CarvedBoneRelicFromHolmes : BaseTalisman
	{
		public override bool ForceShowName{ get{ return true; } }
	
		[Constructable]
		public CarvedBoneRelicFromHolmes() : base( 0x2F59 )
		{			
            Name = "Carved Bone Relic From Holmes";
			Hue = 2962;
			
			Summoner = new TalismanAttribute( typeof( SummonedVorpalBunny ), 0, 1072401 );//Vorpal Bunny
			MaxChargeTime = 1800;			
            SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
            Attributes.EnhancePotions = 15;	
		}
		
		public CarvedBoneRelicFromHolmes( Serial serial ) :  base( serial )
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
