using System;

namespace Server.Items
{
    public class ExporMalasFlamus : BladedStaff
	{
        public override int LabelNumber => 1160477; //Expor Malas Flamus
		public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public ExporMalasFlamus()
            : base()
        {
            Hue = 2702;

            SearingWeapon = true;
            WeaponAttributes.HitFireArea = 70;
            WeaponAttributes.HitLeechStam = 50;
            WeaponAttributes.HitLeechMana = 100;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.HitLowerAttack = 50;
            Attributes.WeaponDamage = 30;
        }

        public ExporMalasFlamus(Serial serial)
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

    public class GargishExporMalasFlamus : StoneWarSword
	{
        public override int LabelNumber => 1160477; //Expor Malas Flamus
		public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public GargishExporMalasFlamus()
            : base()
        {
            Hue = 2702;

            SearingWeapon = true;
            WeaponAttributes.HitFireArea = 70;
            WeaponAttributes.HitLeechStam = 50;
            WeaponAttributes.HitLeechMana = 100;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.HitLowerAttack = 50;
            Attributes.WeaponDamage = 30;
        }
        public GargishExporMalasFlamus(Serial serial)
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