using System;

namespace Server.Items
{
    public class GargishEmberOfWildfire : SoulGlaive
	{
		public override bool IsArtifact => true;

        [Constructable]
        public GargishEmberOfWildfire()
        {
            Name = "Ember of Wildfire";
            Hue = 2758;

            NegativeAttributes.Antique = 1;
            
            SearingWeapon = true;
            WeaponAttributes.HitFireArea = 70;
            WeaponAttributes.HitLeechStam = 40;
            WeaponAttributes.HitLeechMana = 40;
            Attributes.WeaponSpeed = 25;
            Attributes.WeaponDamage = 50;
        }

        public GargishEmberOfWildfire(Serial serial)
            : base(serial)
        {
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = cold = pois = nrgy = chaos = direct = 0;
            fire = 100;
        }

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class EmberOfWildfire : CompositeBow
	{
		public override bool IsArtifact => true;

        [Constructable]
        public EmberOfWildfire()
        {
            Name = "Ember of Wildfire";
            Hue = 2758;

            NegativeAttributes.Antique = 1;
            
            SearingWeapon = true;
            WeaponAttributes.HitFireArea = 70;
            WeaponAttributes.HitLeechStam = 40;
            WeaponAttributes.HitLeechMana = 40;
            Attributes.WeaponSpeed = 25;
            Attributes.WeaponDamage = 50;
        }

        public EmberOfWildfire(Serial serial)
            : base(serial)
        {
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = cold = pois = nrgy = chaos = direct = 0;
            fire = 100;
        }

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}