using Server;

namespace Server.Items
{
    public class WildfireLantern : GoldRing
    {

        [Constructable]
        public WildfireLantern()
        {
            Name = "Wildfire Lantern";
            Attributes.SpellChanneling = 1;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 5;
            Attributes.CastRecovery = 2;
            Light = LightType.Circle300;
			Weight = 1.0;
			ItemID = 0xA76A;
			Layer = Layer.TwoHanded;            
        }

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( this.ItemID == 0xA769 ){ list.Add( 1049644, "Double-Click to Unequip"); }
			else { list.Add( 1049644, "Double-Click to Equip"); }
        } 

		public override bool AllowEquipedCast( Mobile from )
		{
			return true;
		}

		public override bool OnEquip( Mobile from )
		{
			this.ItemID = 0xA769;
			return base.OnEquip( from );
		}

		public override void OnRemoved( object parent )
		{
			this.ItemID = 0xA76A;
			base.OnRemoved( parent );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Item lantern = from.FindItemOnLayer( Layer.TwoHanded );
			if ( lantern == this )
			{
				from.AddToBackpack(this);
				this.ItemID = 0xA76A;
				from.PlaySound( 0x4BB );
				base.OnRemoved( from );
			}
			else if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				if ( from.FindItemOnLayer( Layer.TwoHanded ) != null )
				{
					from.AddToBackpack( from.FindItemOnLayer( Layer.TwoHanded ) );
				}
				from.SendMessage( "You put the lantern in your left hand." );
				from.AddItem(this);
				this.ItemID = 0xA769;
				from.PlaySound( 0x47 );
				base.OnEquip( from );
			}
		}

        public WildfireLantern(Serial serial)
            : base(serial)
        {
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