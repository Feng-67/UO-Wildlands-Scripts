/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Engines.Plants;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
    /* BASE BRAIN SYSTEM */
    public abstract class BaseCubStoreDye : Item, IUsesRemaining
    {
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining 
        { 
            get { return m_UsesRemaining; } 
            set { m_UsesRemaining = value; InvalidateProperties(); } 
        }

        public bool ShowUsesRemaining { get { return true; } set { } }

        public BaseCubStoreDye(int hue) : base(0xEFF)
        {
            Weight = 1.0;
            Hue = hue;
            m_UsesRemaining = 5;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1070929); // Select the item to dye.
                from.Target = new InternalDyeTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
            }
        }

        public override void AddUsesRemainingProperties(ObjectPropertyList list)
        {
            list.Add(1060584, m_UsesRemaining.ToString()); // uses remaining: ~1_val~
        }

        public BaseCubStoreDye(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_UsesRemaining = reader.ReadInt();
        }

        private class InternalDyeTarget : Target
        {
            private BaseCubStoreDye m_Dye;
            public InternalDyeTarget(BaseCubStoreDye dye) : base(8, false, TargetFlags.None) { m_Dye = dye; }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Item item = targeted as Item;

                if (item == null)
                {
                    from.SendLocalizedMessage(1042083); // You cannot dye that.
                    return;
                }

                if (!item.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
                    return;
                }

                if (item is HoodedShroudOfShadows || item is MonkRobe)
                {
                    from.SendLocalizedMessage(1042083); // You cannot dye that.
                    return;
                }

                bool valid = (item is IDyable || item is BaseTalisman ||
                    item is BaseBook || item is BaseClothing ||
                    item is BaseJewel || item is BaseStatuette ||
                    item is BaseWeapon || item is Runebook ||
                    item is Spellbook || item is DecorativePlant || item is ShoulderParrot ||
                    item.IsArtifact || BasePigmentsOfTokuno.IsValidItem(item));

                if (!valid && item is BaseArmor)
                {
                    CraftResourceType restype = CraftResources.GetType(((BaseArmor)item).Resource);
                    if ((CraftResourceType.Leather == restype || CraftResourceType.Metal == restype) &&
                        ArmorMaterialType.Bone != ((BaseArmor)item).MaterialType)
                    {
                        valid = true;
                    }
                }

                if (valid)
                {
                    item.Hue = m_Dye.Hue;
                    from.PlaySound(0x23E);
                    m_Dye.UsesRemaining--;
                    if (m_Dye.UsesRemaining <= 0)
                        m_Dye.Delete();
                }
                else
                {
                    from.SendLocalizedMessage(1042083); // You cannot dye that.
                }
            }
        }
    }

    /* INDIVIDUAL DYE CLASSES */
    // Re-uses Clilocs and Hues from CompassionPigment.cs

    public class CubPhoenixRed : BaseCubStoreDye {
        public override int LabelNumber => 1151651;
        [Constructable] public CubPhoenixRed() : base(1964) { }
        public CubPhoenixRed(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubAuraOfAmber : BaseCubStoreDye {
        public override int LabelNumber => 1152308;
        [Constructable] public CubAuraOfAmber() : base(1967) { }
        public CubAuraOfAmber(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubDeepViolet : BaseCubStoreDye {
        public override int LabelNumber => 1151912;
        [Constructable] public CubDeepViolet() : base(1929) { }
        public CubDeepViolet(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubPolishedBronze : BaseCubStoreDye {
        public override int LabelNumber => 1151909;
        [Constructable] public CubPolishedBronze() : base(1944) { }
        public CubPolishedBronze(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubVibrantCrimson : BaseCubStoreDye {
        public override int LabelNumber => 1153386;
        [Constructable] public CubVibrantCrimson() : base(1964) { }
        public CubVibrantCrimson(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubLavender : BaseCubStoreDye {
        public override int LabelNumber => 1151650;
        [Constructable] public CubLavender() : base(1951) { }
        public CubLavender(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubGleamingFuchsia : BaseCubStoreDye {
        public override int LabelNumber => 1152311;
        [Constructable] public CubGleamingFuchsia() : base(1930) { }
        public CubGleamingFuchsia(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubDeepBlue : BaseCubStoreDye {
        public override int LabelNumber => 1152348;
        [Constructable] public CubDeepBlue() : base(1939) { }
        public CubDeepBlue(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubGlossyFuchsia : BaseCubStoreDye {
        public override int LabelNumber => 1152347;
        [Constructable] public CubGlossyFuchsia() : base(1919) { }
        public CubGlossyFuchsia(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubDarkVoid : BaseCubStoreDye {
        public override int LabelNumber => 1154214;
        [Constructable] public CubDarkVoid() : base(2068) { }
        public CubDarkVoid(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubMurkySeagreen : BaseCubStoreDye {
        public override int LabelNumber => 1152309;
        [Constructable] public CubMurkySeagreen() : base(1992) { }
        public CubMurkySeagreen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubReflectiveShadow : BaseCubStoreDye {
        public override int LabelNumber => 1153387;
        [Constructable] public CubReflectiveShadow() : base(1910) { }
        public CubReflectiveShadow(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubLiquidSunshine : BaseCubStoreDye {
        public override int LabelNumber => 1154213;
        [Constructable] public CubLiquidSunshine() : base(1923) { }
        public CubLiquidSunshine(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubShadowyBlue : BaseCubStoreDye {
        public override int LabelNumber => 1152310;
        [Constructable] public CubShadowyBlue() : base(1960) { }
        public CubShadowyBlue(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubBlackAndGreen : BaseCubStoreDye {
        public override int LabelNumber => 1151911;
        [Constructable] public CubBlackAndGreen() : base(1979) { }
        public CubBlackAndGreen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubGlossyBlue : BaseCubStoreDye {
        public override int LabelNumber => 1151910;
        [Constructable] public CubGlossyBlue() : base(1916) { }
        public CubGlossyBlue(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubHunterGreen : BaseCubStoreDye {
        public override int LabelNumber => 1151649;
        [Constructable] public CubHunterGreen() : base(1936) { }
        public CubHunterGreen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubSlateBlue : BaseCubStoreDye {
        public override int LabelNumber => 1151653;
        [Constructable] public CubSlateBlue() : base(1983) { }
        public CubSlateBlue(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubMotherOfPearl : BaseCubStoreDye {
        public override int LabelNumber => 1154120;
        [Constructable] public CubMotherOfPearl() : base(2720) { }
        public CubMotherOfPearl(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubStarBlue : BaseCubStoreDye {
        public override int LabelNumber => 1154121;
        [Constructable] public CubStarBlue() : base(2723) { }
        public CubStarBlue(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubMurkyAmber : BaseCubStoreDye {
        public override int LabelNumber => 1152350;
        [Constructable] public CubMurkyAmber() : base(1989) { }
        public CubMurkyAmber(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubVibranSeagreen : BaseCubStoreDye {
        public override int LabelNumber => 1152349;
        [Constructable] public CubVibranSeagreen() : base(1970) { }
        public CubVibranSeagreen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubVibrantOcher : BaseCubStoreDye {
        public override int LabelNumber => 1154736;
        [Constructable] public CubVibrantOcher() : base(2725) { }
        public CubVibrantOcher(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubMossyGreen : BaseCubStoreDye {
        public override int LabelNumber => 1154731;
        [Constructable] public CubMossyGreen() : base(2684) { }
        public CubMossyGreen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubOliveGreen : BaseCubStoreDye {
        public override int LabelNumber => 1154733;
        [Constructable] public CubOliveGreen() : base(2709) { }
        public CubOliveGreen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubMottledSunsetBlue : BaseCubStoreDye {
        public override int LabelNumber => 1154734;
        [Constructable] public CubMottledSunsetBlue() : base(2714) { }
        public CubMottledSunsetBlue(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubTyrianPurple : BaseCubStoreDye {
        public override int LabelNumber => 1154735;
        [Constructable] public CubTyrianPurple() : base(2716) { }
        public CubTyrianPurple(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class CubIntenseTeal : BaseCubStoreDye {
        public override int LabelNumber => 1154732;
        [Constructable] public CubIntenseTeal() : base(2691) { }
        public CubIntenseTeal(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}
