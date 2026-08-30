namespace Server.Items
{

    public class LordMorphiusEpaulettes : BaseOuterTorso
    {
        public override bool IsArtifact => true;

        [Constructable]
        public LordMorphiusEpaulettes()
            : base(0x9985, 0)
        {
            Name = "Lord Morphius' Epaulettes";
            Hue = 2742;
            Attributes.BonusStam = 8;
            Attributes.RegenStam = 2;
            Attributes.LowerManaCost = 3;
            Attributes.WeaponSpeed = 10;
        }

        public LordMorphiusEpaulettes(Serial serial)
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
    public class GargishLordMorphiusEpaulettes : BaseOuterTorso
    {
        public override bool IsArtifact => true;
        public override bool CanBeWornByGargoyles => true;
        public override Race RequiredRace => Race.Gargoyle;

        [Constructable]
        public GargishLordMorphiusEpaulettes()
            : base(0x9986, 0)
        {
            Name = "Lord Morphius' Epaulettes";
            Hue = 2742;
            Attributes.BonusStam = 8;
            Attributes.RegenStam = 2;
            Attributes.LowerManaCost = 3;
            Attributes.WeaponSpeed = 10;
        }

        public GargishLordMorphiusEpaulettes(Serial serial)
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