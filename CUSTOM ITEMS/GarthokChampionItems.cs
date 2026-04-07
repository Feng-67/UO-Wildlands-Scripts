/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;

namespace Server.Items
{
    // 1. Garthok's Motivator (Whip)
    public class GarthoksMotivator : BladedWhip
    {
        [Constructable]
        public GarthoksMotivator()
        {
            Name = "Garthok's Motivator";
            Weight = 5.0;
            Hue = 1934;

            this.Slayer = SlayerName.Repond;
            this.Attributes.WeaponSpeed = 325;
            this.WeaponAttributes.HitFatigue = 50;
            this.WeaponAttributes.HitLeechStam = 100;
            this.WeaponAttributes.HitLeechHits = 100;
            this.WeaponAttributes.HitLeechMana = 100;
            this.WeaponAttributes.UseBestSkill = 1;

            this.AosElementDamages.Physical = 0;

            switch (Utility.Random(4))
            {
                case 0: this.AosElementDamages.Fire = 100; this.WeaponAttributes.HitFireArea = 50; break;
                case 1: this.AosElementDamages.Cold = 100; this.WeaponAttributes.HitColdArea = 50; break;
                case 2: this.AosElementDamages.Poison = 100; this.WeaponAttributes.HitPoisonArea = 50; break;
                case 3: this.AosElementDamages.Energy = 100; this.WeaponAttributes.HitEnergyArea = 50; break;
            }
        }

        public GarthoksMotivator(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 2. Garthok's Toothpick (Staff)
    public class GarthoksToothpick : GnarledStaff
    {
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public GarthoksToothpick()
        {
            Name = "Garthok's Toothpick";
            Weight = 3.0;
            Hue = 1934;

            this.WeaponAttributes.HitFatigue = 50;
            this.WeaponAttributes.SplinteringWeapon = 30;

            this.AosElementDamages.Physical = 0;
            switch (Utility.Random(4))
            {
                case 0: AosElementDamages.Fire = 100; break;
                case 1: AosElementDamages.Cold = 100; break;
                case 2: AosElementDamages.Poison = 100; break;
                case 3: AosElementDamages.Energy = 100; break;
            }
        }

        public GarthoksToothpick(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 3. Orc Skin Belt
    public class OrcSkinBelt : HalfApron
    {
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public OrcSkinBelt()
        {
            Name = "Orc Skin Belt";
            Weight = 2.0;
            Hue = 1934;

            Attributes.BonusHits = 5;
            Attributes.EnhancePotions = 25;
            Attributes.DefendChance = 10;
        }

        public OrcSkinBelt(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 4. Orc Champion Visage
    public class OrcChampionVisage : BoneHelm
    {
        public override int InitMinHits { get { return 200; } }
        public override int InitMaxHits { get { return 200; } }
        public override int BasePhysicalResistance { get { return 1; } }
        public override int BaseFireResistance { get { return 1; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 8; } }

        [Constructable]
        public OrcChampionVisage()
        {
            string[] orcTypes = { "Orc", "Orc Captain", "Orc Bomber", "Orc Scout", "Orc Mage", "Orcish Lord", "Orc Brute", "Orc Hound" };
            Name = $"Guise of The {orcTypes[Utility.Random(orcTypes.Length)]}";
            Hue = 1934;
            Weight = 4.0;
            LootType = LootType.Blessed;
        }

        public OrcChampionVisage(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 5. Garthok's Lunch (Deco)
    public class GarthoksLunch : Item
    {
        [Constructable]
        public GarthoksLunch() : base(0x9F1)
        {
            Name = "Garthok's Lunch";
            Weight = 2.0;
            Hue = 0x1D0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendMessage("The smell is enough to turn your stomach. You decide not to eat it.");
                from.PlaySound(0x13D);
            }
            else { from.SendLocalizedMessage(500446); }
        }

        public GarthoksLunch(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 6. Skull of Garthok (Deco)
    public class SkullOfGarthok : Item
    {
        [Constructable]
        public SkullOfGarthok() : base(0x1AE1)
        {
            Name = "Skull of Garthok";
            Weight = 1.0;
            Hue = 1934;
        }

        public SkullOfGarthok(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
