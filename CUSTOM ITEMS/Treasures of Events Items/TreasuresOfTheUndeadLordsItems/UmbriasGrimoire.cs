using System;

namespace Server.Items
{
    public class UmbriasGrimoire : NecromancerSpellbook
    {
        public override bool IsArtifact => true;

        [Constructable]
        public UmbriasGrimoire()
            : base((ulong)0x1FFFF) //Makes it full
        {
            Hue = 2500; //Same as Juo'nar's Grimoire I think?!
			Name = "Umbria's Grimoire";
            
            Slayer = SlayerName.Silver;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 30;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 5;
        }

        public UmbriasGrimoire(Serial serial)
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