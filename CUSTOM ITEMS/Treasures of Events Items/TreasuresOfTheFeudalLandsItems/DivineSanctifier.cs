using System;

namespace Server.Items
{
    public class DivineSanctifier : HammerPick
    {
        public override int LabelNumber => 1161982; //Divine Sanctifier
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool IsArtifact => false;

        [Constructable]
        public DivineSanctifier()
            : base()
        {
            Hue = 2764;
            WeaponAttributes.HitLeechMana = 100;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.HitLeechStam = 50;
            WeaponAttributes.HitFatigue = 70;
            WeaponAttributes.HitLowerDefend = 50;
            WeaponAttributes.HitLightning = 70;
            Attributes.WeaponDamage = 50;
        }

        public DivineSanctifier(Serial serial)
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

    public class DivineSanctifierGargish : DiscMace
    {
        public override int LabelNumber => 1161982; //Divine Sanctifier
        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
        public override bool IsArtifact => false;

        [Constructable]
        public DivineSanctifierGargish()
            : base()
        {
            Hue = 2764;
            WeaponAttributes.HitLeechMana = 100;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.HitLeechStam = 50;
            WeaponAttributes.HitFatigue = 70;
            WeaponAttributes.HitLowerDefend = 50;
            WeaponAttributes.HitLightning = 70;
            Attributes.WeaponDamage = 50;
        }

        public DivineSanctifierGargish(Serial serial)
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