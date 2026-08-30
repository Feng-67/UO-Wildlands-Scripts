using System;

namespace Server.Items
{
    public class MantleOfTheArchlich : Robe
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public MantleOfTheArchlich() 
            : base()
        {
            Name = "Mantle of the Archlich";
            Hue = 2702;
            SkillBonuses.SetValues(0, SkillName.MagicResist, 10);
            Attributes.SpellDamage = 8;
            Attributes.CastSpeed = 1;
        }

        public MantleOfTheArchlich(Serial serial)
            : base(serial)
        {
        }
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool CanFortify => true;
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