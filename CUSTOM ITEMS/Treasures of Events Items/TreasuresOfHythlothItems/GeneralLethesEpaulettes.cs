namespace Server.Items
{

    public class GeneralLethesEpaulettes : BaseOuterTorso
    {
        public override bool IsArtifact => true;

        [Constructable]
        public GeneralLethesEpaulettes()
            : base(0x9985, 0)
        {
            Name = "General Lethe's Epaulettes";
            Hue = 2642;
            Attributes.BonusMana = 8;
            Attributes.RegenMana = 1;
            Attributes.CastRecovery = 1;
            Attributes.LowerRegCost = 10;
        }

        public GeneralLethesEpaulettes(Serial serial)
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
    public class GargishGeneralLethesEpaulettes : BaseOuterTorso
    {
        public override bool IsArtifact => true;
        public override bool CanBeWornByGargoyles => true;
        public override Race RequiredRace => Race.Gargoyle;

        [Constructable]
        public GargishGeneralLethesEpaulettes()
            : base(0x9986, 0)
        {
            Name = "General Lethe's Epaulettes";
            Hue = 2642;
            Attributes.BonusMana = 8;
            Attributes.RegenMana = 1;
            Attributes.CastRecovery = 1;
            Attributes.LowerRegCost = 10;
        }

        public GargishGeneralLethesEpaulettes(Serial serial)
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