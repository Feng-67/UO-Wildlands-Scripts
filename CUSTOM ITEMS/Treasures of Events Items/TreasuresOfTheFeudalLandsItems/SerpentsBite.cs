using System;

namespace Server.Items
{ 
    public class SerpentsBite : BaseTalisman
    {
		public override bool IsArtifact => true;
		public override int LabelNumber => 1161981; //Serpent's Bite
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool ForceShowName => true;
		
        [Constructable]
        public SerpentsBite()
            : base(0x2F59)
        { 
            Hue = 2764;
            Attributes.NightSight = 1;
            Attributes.Luck = 125;
            Attributes.SpellDamage = 8;
            Attributes.LowerManaCost = 5;
            Attributes.LowerRegCost = 10;
        }

        public SerpentsBite(Serial serial)
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