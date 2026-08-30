using System;

namespace Server.Items
{
	
    public class BriarThornWhip : SpikedWhip
    {
        public override bool IsArtifact => true;

        [Constructable]
        public BriarThornWhip()
		: base()
        {
			Name = "Briar Thorn Whip";
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

        public BriarThornWhip(Serial serial)
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