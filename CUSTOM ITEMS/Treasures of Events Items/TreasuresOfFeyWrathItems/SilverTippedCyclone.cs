using System;

namespace Server.Items
{
    public class SilverTippedCyclone : Cyclone
    {
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public SilverTippedCyclone()
            : base()
        {
            Name = "Silver Tipped Cyclone";
            //Hue = ; TODO: NEED HUE!

            WeaponAttributes.HitLeechHits = 40;
            WeaponAttributes.HitLeechMana = 40;
            WeaponAttributes.HitLightning = 35;
            ExtendedWeaponAttributes.Bane = 1;
            Attributes.AttackChance = 10;
            Attributes.WeaponDamage = 25;
            AosElementDamages.Fire = 100;            
        }

        public SilverTippedCyclone(Serial serial)
            : base(serial)
        {
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