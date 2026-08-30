namespace Server.Items
{
    public class ShugenjasWand : BaseWand
    {
        public override int LabelNumber => 1161977; //Shugenja's Wand
        public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public ShugenjasWand() : base(WandEffect.None, 0, 0)
        {
			Hue = 2764;
            Attributes.SpellChanneling = 1;
            WeaponAttributes.MageWeapon = 30;
            Attributes.RegenMana = 10;
            Attributes.CastRecovery = 2;
        }

        public ShugenjasWand(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}