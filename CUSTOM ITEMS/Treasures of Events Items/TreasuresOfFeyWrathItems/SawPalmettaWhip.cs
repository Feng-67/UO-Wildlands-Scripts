using System;

namespace Server.Items
{
	
    public class SawPalmettaWhip : BladedWhip
    {
        public override bool IsArtifact => true;

        [Constructable]
        public SawPalmettaWhip()
		: base()
        {
			Name = "Saw Palmetta Whip";
			Weight = 5.0;
			Hue = 2755;

            WeaponAttributes.HitHarm = 35;
            WeaponAttributes.HitLowerDefend = 30;
            WeaponAttributes.HitFireArea = 50;
            WeaponAttributes.HitLeechStam = 40;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.HitLeechMana = 81;
            Attributes.WeaponDamage = 25;
        }

        public SawPalmettaWhip(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
   
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}