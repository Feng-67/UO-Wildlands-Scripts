using System;
using Server.Mobiles;

namespace Server.Items
{
    public class AnBalXen : BaseTalisman
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public AnBalXen()
            : base(0x9E28)
        {
            Name = "An Bal Xen";
            Slayer = TalismanSlayerName.Demon;
            Attributes.BonusStr = 1;
            Attributes.RegenHits = 2;
            Attributes.WeaponDamage = 20;
            Attributes.AttackChance = 10;
        }

        public AnBalXen(Serial serial)
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