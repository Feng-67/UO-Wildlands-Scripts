using System;
using Server.Mobiles;

namespace Server.Items
{ 
    public class InCorpManiXen : BaseTalisman
    {
		public override bool IsArtifact { get { return true; } }
        public override bool ForceShowName { get { return true; } }
		
        [Constructable]
        public InCorpManiXen()
            : base(0x9E29)
        { 		
            Name = "In Corp Mani Xen";
            Weight = 1.0;
            Hue = 2755;
            Slayer = TalismanSlayerName.Fey;
            Attributes.BonusStr = 1;
            Attributes.RegenHits = 2;
            Attributes.AttackChance = 10;
            Attributes.WeaponDamage = 20;
        }

        public InCorpManiXen(Serial serial)
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