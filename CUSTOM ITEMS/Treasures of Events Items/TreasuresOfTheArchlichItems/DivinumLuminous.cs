namespace Server.Items
{
    public class DivinumLuminous : BaseTalisman
    {
        public override int LabelNumber => 1160471;
        public override bool IsArtifact => true;

        [Constructable]
        public DivinumLuminous()
            : base(0x2F5A)
        {
            Name = "Divinum Luminous";
            Hue = 1158;
            Attributes.Luck = 100;
            Attributes.ReflectPhysical = 15;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.WeaponDamage = 20;
        }

        public DivinumLuminous(Serial serial)
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