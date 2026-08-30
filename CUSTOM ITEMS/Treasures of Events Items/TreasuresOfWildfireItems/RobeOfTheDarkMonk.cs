using System;

namespace Server.Items
{
    [Flipable(0x2684, 0x2683)]
    public class RobeOfTheDarkMonk : BaseOuterTorso
	{
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool CanBeWornByGargoyles => true;

        [Constructable]
        public RobeOfTheDarkMonk()
            : this(1109)
        {
        }

        [Constructable]
        public RobeOfTheDarkMonk(int hue)
            : base(0x2684, hue)
        {
            Name = "Robe of the Dark Monk";
            Weight = 1.0;
            Hue = 0x455;
            SAAbsorptionAttributes.EaterFire = 6;
            Attributes.SpellDamage = 6;
            Attributes.IncreasedKarmaLoss = 6;
        }

        public RobeOfTheDarkMonk(Serial serial)
            : base(serial)
        {
        }

        public override bool Dye(Mobile from, DyeTub sender)
        {
            from.SendLocalizedMessage(sender.FailMessage);
            return false;
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