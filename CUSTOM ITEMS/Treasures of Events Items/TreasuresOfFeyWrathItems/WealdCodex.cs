using System;

namespace Server.Items
{
    public class WealdCodex : Spellbook
    {
        public override bool IsArtifact => true;

        [Constructable]
        public WealdCodex()
            : base()
        {
            Hue = 2758;
			Name = "Weald Codex";
            Content = UInt64.MaxValue;
            
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 50;
            Attributes.CastSpeed = 1;
            Attributes.LowerRegCost = 10;            
        }

        public WealdCodex(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}