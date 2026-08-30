using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class ArtisansEsteem : SilverRing
    {
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
		public ArtisansEsteem() : base()
		{
            Name = "Artisan's Esteem";
			Weight = 1.0;
			Hue = 2758;

            SkillBonuses.SetValues(0, SkillName.EvalInt, 20);
            Attributes.EnhancePotions = 25;
            Attributes.DefendChance = 20;
            Attributes.SpellDamage = 15;
            Attributes.CastRecovery = 3;
		}

        public ArtisansEsteem( Serial serial ) : base( serial )
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