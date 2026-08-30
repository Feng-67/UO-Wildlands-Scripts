namespace Server.Items
{
    public class CingulumOfJuonar : GargishApron
    {
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool CanFortify => true;

        [Constructable]
        public CingulumOfJuonar()
            : base(0x2F5A)
        {
            Name = "Cingulum Of Juo'nar";

            SkillBonuses.SetValues(0, SkillName.Necromancy, 5);
            Attributes.BonusHits = 5;
            Attributes.SpellDamage = 5;
            Attributes.CastSpeed = 1;
        }

        public CingulumOfJuonar(Serial serial)
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