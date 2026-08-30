using System;

namespace Server.Items
{
    public class GrimoireOfNature : Spellbook
    {
        public override bool IsArtifact => true;

        [Constructable]
        public GrimoireOfNature()
            : base()
        {
            Hue = 2755;
			Name = "Grimoire Of Nature";
            Content = UInt64.MaxValue;
            
            Slayer = SlayerName.Fey;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 30;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 5;
        }

        public GrimoireOfNature(Serial serial)
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