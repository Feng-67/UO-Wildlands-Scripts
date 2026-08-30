using System;

namespace Server.Items
{
    public class SplinterFromTheTreeOfStrife : Bokuto
	{
		public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public SplinterFromTheTreeOfStrife()
            : base()
        {
            Name = "Splinter from the Tree of Strife";
            Hue = 2707; //TODO: Verify hue!
            
            NegativeAttributes.Antique = 1;
            WeaponAttributes.SplinteringWeapon = 20;
            WeaponAttributes.HitLeechMana = 60;
            Attributes.SpellDamage = 25; 
            Attributes.WeaponDamage = 50;
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            fire = pois = nrgy = chaos = direct = 0;

            phys = cold = 50;
        }

        public SplinterFromTheTreeOfStrife(Serial serial)
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