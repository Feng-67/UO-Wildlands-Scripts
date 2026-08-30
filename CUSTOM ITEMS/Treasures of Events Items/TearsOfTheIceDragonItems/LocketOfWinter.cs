using System;
using Server.Mobiles;

namespace Server.Items
{ 
    public class LocketOfWinter : BaseTalisman
    {
		public override bool IsArtifact { get { return true; } }
        public override bool ForceShowName { get { return true; } }
		
        [Constructable]
        public LocketOfWinter()
            : base(0x9E2B)
        { 		
            Name = "Locket Of Winter";
            Weight = 1.0;
            Hue = 2729;
            Slayer = TalismanSlayerName.Ice;
            Summoner = new TalismanAttribute( typeof( GiantIceWorm ), 0, 1072483 );//Giant Ice Worm
            MaxChargeTime = 1800;
        }

        public LocketOfWinter(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
			list.Add("Giant Ice Worm Summoning");
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