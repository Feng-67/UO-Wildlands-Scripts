using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefTinkering), typeof(GargishSolariasSecretPoisons))]
    public class SolariasSecretPoisons : GoldEarrings
	{		
		public override bool IsArtifact { get { return true; } }

        public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		
		[Constructable]
		public SolariasSecretPoisons()
		{
            Name = "Solaria's Secret Poisons";
			Hue = 2758;  

            SkillBonuses.SetValues(0, SkillName.Ninjitsu, 10);
            Attributes.AttackChance = 10;
		}
		
		public SolariasSecretPoisons(Serial serial) : base(serial)
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
	
	public class GargishSolariasSecretPoisons : GargishEarrings
	{		
		public override bool IsArtifact { get { return true; } }
		
		public override int InitMinHits{ get{ return 255; } }
        public override int InitMaxHits{ get{ return 255; } }

        public override int PhysicalResistance { get { return 15; } }
        public override int FireResistance { get { return 18; } }
        public override int ColdResistance { get { return 15; } }
        public override int PoisonResistance { get { return 18; } }
        public override int EnergyResistance { get { return 15; } }

        [Constructable]
        public GargishSolariasSecretPoisons()
        {
            Name = "Solaria's Secret Poisons";
            Hue = 2758;

            SkillBonuses.SetValues(0, SkillName.Ninjitsu, 10);
            Attributes.AttackChance = 10;
        }
		
		public GargishSolariasSecretPoisons(Serial serial) : base(serial)
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