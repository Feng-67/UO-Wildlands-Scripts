using System;

namespace Server.Items
{
    public class DrogenisSpellbook : Spellbook
    {
        public override bool IsArtifact => true;

        [Constructable]
        public DrogenisSpellbook()
            : base()
        {
            Hue = 1195;
			Name = "Drogeni's Spellbook";
            Content = UInt64.MaxValue;

            SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            Attributes.DefendChance = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 3;
            Attributes.SpellDamage = 15;
            Attributes.EnhancePotions = 25;
        }

        public DrogenisSpellbook(Serial serial)
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