using System;

namespace Server.Items
{
    public class DrusillasAthame : Kryss
	{
		public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public DrusillasAthame()
            : base()
        {
            Name = "Drusilla's Athame";
            Hue = 2753;
            
            Slayer3 = TalismanSlayerName.Flame; //Flame Slayer
            NegativeAttributes.Antique = 1;
            Attributes.SpellChanneling = 1;
            Attributes.SpellDamage = 25; 
            Attributes.CastSpeed = 1;
            Attributes.WeaponDamage = 20;
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            fire = pois = nrgy = chaos = direct = 0;

            phys = cold = 50;
        }

        public DrusillasAthame(Serial serial)
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

    public class GargishDrusillasAthame : GargishKryss
	{
		public override bool IsArtifact => true;
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public GargishDrusillasAthame()
            : base()
        {
            Name = "Drusilla's Athame";
            Hue = 2753;
            
            Slayer3 = TalismanSlayerName.Flame; //Flame Slayer
            NegativeAttributes.Antique = 1;
            Attributes.SpellChanneling = 1;
            Attributes.SpellDamage = 25; 
            Attributes.CastSpeed = 1;
            Attributes.WeaponDamage = 20;
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            fire = pois = nrgy = chaos = direct = 0;

            phys = cold = 50;
        }

        public GargishDrusillasAthame(Serial serial)
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