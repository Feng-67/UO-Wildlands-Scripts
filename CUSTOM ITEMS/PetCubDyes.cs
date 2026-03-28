using System;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    /* BASE CLASSES */

    public abstract class BasePetCubDye : Item
    {
        public abstract int TargetHue { get; }
        public abstract string DyeName { get; }
        public virtual bool IsSample => false;

        public BasePetCubDye(int itemID) : base(itemID)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 1))
            {
                from.SendMessage("Target the pet you wish to {0} the {1} dye on.", IsSample ? "sample" : "apply", DyeName);
                from.Target = new InternalPetTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }

        private class InternalPetTarget : Target
        {
            private readonly BasePetCubDye m_Dye;

            public InternalPetTarget(BasePetCubDye dye) : base(1, false, TargetFlags.None)
            {
                m_Dye = dye;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature pet && pet.ControlMaster == from)
                {
                    if (!from.InRange(m_Dye.GetWorldLocation(), 1) || !from.InRange(pet.Location, 1))
                    {
                        from.SendLocalizedMessage(500446);
                    }
                    else
                    {
                        int oldHue = pet.Hue;
                        pet.Hue = m_Dye.TargetHue;
                        from.PlaySound(0x23E);

                        if (m_Dye.IsSample)
                        {
                            string dyeName = m_Dye.DyeName; // Capture the name before deleting the item
                            from.SendMessage("The sample is applied! Your pet will revert in 30 seconds.");
                            Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                            {
                                if (pet != null && !pet.Deleted)
                                {
                                    pet.Hue = oldHue;
                                    if (pet.ControlMaster != null)
                                        pet.ControlMaster.SendMessage("The {0} sample has worn off.", dyeName);
                                }
                            });
                        }
                        else
                        {
                            from.SendMessage("The dye tub vanishes as your pet turns {0}!", m_Dye.DyeName);
                        }

                        m_Dye.Delete();
                    }
                }
                else
                {
                    from.SendMessage("You can only dye your own pets!");
                }
            }
        }

        public BasePetCubDye(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public abstract class BasePetCubPermanent : BasePetCubDye
    {
        public override bool IsSample => false;
        public BasePetCubPermanent(int hue) : base(0xFAB) { Hue = hue; }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1114057, "<BASEFONT COLOR=#FFFF00>Single Use Only</BASEFONT>");
        }
        public BasePetCubPermanent(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public abstract class BasePetCubSample : BasePetCubDye
    {
        public override bool IsSample => true;
        public BasePetCubSample(int hue) : base(0xEFB) { Hue = hue; }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1114057, "<BASEFONT COLOR=#FFFF00>30 sec Sample Pet Dye</BASEFONT>");
        }
        public BasePetCubSample(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    /* DEFINITIONS */

    // Phoenix Red (1964)
    public class PetCubPhoenixRed : BasePetCubPermanent { public override int TargetHue => 1964; public override string DyeName => "Phoenix Red"; [Constructable] public PetCubPhoenixRed() : base(1964) { Name = "Pet Dye Tub: Phoenix Red"; } public PetCubPhoenixRed(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubPhoenixRed : BasePetCubSample { public override int TargetHue => 1964; public override string DyeName => "Phoenix Red"; [Constructable] public SamplePetCubPhoenixRed() : base(1964) { Name = "Pet Dye Sample: Phoenix Red"; } public SamplePetCubPhoenixRed(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Aura Of Amber (1967)
    public class PetCubAuraOfAmber : BasePetCubPermanent { public override int TargetHue => 1967; public override string DyeName => "Aura Of Amber"; [Constructable] public PetCubAuraOfAmber() : base(1967) { Name = "Pet Dye Tub: Aura Of Amber"; } public PetCubAuraOfAmber(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubAuraOfAmber : BasePetCubSample { public override int TargetHue => 1967; public override string DyeName => "Aura Of Amber"; [Constructable] public SamplePetCubAuraOfAmber() : base(1967) { Name = "Pet Dye Sample: Aura Of Amber"; } public SamplePetCubAuraOfAmber(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Deep Violet (1929)
    public class PetCubDeepViolet : BasePetCubPermanent { public override int TargetHue => 1929; public override string DyeName => "Deep Violet"; [Constructable] public PetCubDeepViolet() : base(1929) { Name = "Pet Dye Tub: Deep Violet"; } public PetCubDeepViolet(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubDeepViolet : BasePetCubSample { public override int TargetHue => 1929; public override string DyeName => "Deep Violet"; [Constructable] public SamplePetCubDeepViolet() : base(1929) { Name = "Pet Dye Sample: Deep Violet"; } public SamplePetCubDeepViolet(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Polished Bronze (1944)
    public class PetCubPolishedBronze : BasePetCubPermanent { public override int TargetHue => 1944; public override string DyeName => "Polished Bronze"; [Constructable] public PetCubPolishedBronze() : base(1944) { Name = "Pet Dye Tub: Polished Bronze"; } public PetCubPolishedBronze(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubPolishedBronze : BasePetCubSample { public override int TargetHue => 1944; public override string DyeName => "Polished Bronze"; [Constructable] public SamplePetCubPolishedBronze() : base(1944) { Name = "Pet Dye Sample: Polished Bronze"; } public SamplePetCubPolishedBronze(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Vibrant Crimson (1964)
    public class PetCubVibrantCrimson : BasePetCubPermanent { public override int TargetHue => 1964; public override string DyeName => "Vibrant Crimson"; [Constructable] public PetCubVibrantCrimson() : base(1964) { Name = "Pet Dye Tub: Vibrant Crimson"; } public PetCubVibrantCrimson(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubVibrantCrimson : BasePetCubSample { public override int TargetHue => 1964; public override string DyeName => "Vibrant Crimson"; [Constructable] public SamplePetCubVibrantCrimson() : base(1964) { Name = "Pet Dye Sample: Vibrant Crimson"; } public SamplePetCubVibrantCrimson(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Lavender (1951)
    public class PetCubLavender : BasePetCubPermanent { public override int TargetHue => 1951; public override string DyeName => "Lavender"; [Constructable] public PetCubLavender() : base(1951) { Name = "Pet Dye Tub: Lavender"; } public PetCubLavender(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubLavender : BasePetCubSample { public override int TargetHue => 1951; public override string DyeName => "Lavender"; [Constructable] public SamplePetCubLavender() : base(1951) { Name = "Pet Dye Sample: Lavender"; } public SamplePetCubLavender(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Gleaming Fuchsia (1930)
    public class PetCubGleamingFuchsia : BasePetCubPermanent { public override int TargetHue => 1930; public override string DyeName => "Gleaming Fuchsia"; [Constructable] public PetCubGleamingFuchsia() : base(1930) { Name = "Pet Dye Tub: Gleaming Fuchsia"; } public PetCubGleamingFuchsia(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubGleamingFuchsia : BasePetCubSample { public override int TargetHue => 1930; public override string DyeName => "Gleaming Fuchsia"; [Constructable] public SamplePetCubGleamingFuchsia() : base(1930) { Name = "Pet Dye Sample: Gleaming Fuchsia"; } public SamplePetCubGleamingFuchsia(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Deep Blue (1939)
    public class PetCubDeepBlue : BasePetCubPermanent { public override int TargetHue => 1939; public override string DyeName => "Deep Blue"; [Constructable] public PetCubDeepBlue() : base(1939) { Name = "Pet Dye Tub: Deep Blue"; } public PetCubDeepBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubDeepBlue : BasePetCubSample { public override int TargetHue => 1939; public override string DyeName => "Deep Blue"; [Constructable] public SamplePetCubDeepBlue() : base(1939) { Name = "Pet Dye Sample: Deep Blue"; } public SamplePetCubDeepBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Glossy Fuchsia (1919)
    public class PetCubGlossyFuchsia : BasePetCubPermanent { public override int TargetHue => 1919; public override string DyeName => "Glossy Fuchsia"; [Constructable] public PetCubGlossyFuchsia() : base(1919) { Name = "Pet Dye Tub: Glossy Fuchsia"; } public PetCubGlossyFuchsia(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubGlossyFuchsia : BasePetCubSample { public override int TargetHue => 1919; public override string DyeName => "Glossy Fuchsia"; [Constructable] public SamplePetCubGlossyFuchsia() : base(1919) { Name = "Pet Dye Sample: Glossy Fuchsia"; } public SamplePetCubGlossyFuchsia(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Dark Void (2068)
    public class PetCubDarkVoid : BasePetCubPermanent { public override int TargetHue => 2068; public override string DyeName => "Dark Void"; [Constructable] public PetCubDarkVoid() : base(2068) { Name = "Pet Dye Tub: Dark Void"; } public PetCubDarkVoid(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubDarkVoid : BasePetCubSample { public override int TargetHue => 2068; public override string DyeName => "Dark Void"; [Constructable] public SamplePetCubDarkVoid() : base(2068) { Name = "Pet Dye Sample: Dark Void"; } public SamplePetCubDarkVoid(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Murky Seagreen (1992)
    public class PetCubMurkySeagreen : BasePetCubPermanent { public override int TargetHue => 1992; public override string DyeName => "Murky Seagreen"; [Constructable] public PetCubMurkySeagreen() : base(1992) { Name = "Pet Dye Tub: Murky Seagreen"; } public PetCubMurkySeagreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubMurkySeagreen : BasePetCubSample { public override int TargetHue => 1992; public override string DyeName => "Murky Seagreen"; [Constructable] public SamplePetCubMurkySeagreen() : base(1992) { Name = "Pet Dye Sample: Murky Seagreen"; } public SamplePetCubMurkySeagreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Reflective Shadow (1910)
    public class PetCubReflectiveShadow : BasePetCubPermanent { public override int TargetHue => 1910; public override string DyeName => "Reflective Shadow"; [Constructable] public PetCubReflectiveShadow() : base(1910) { Name = "Pet Dye Tub: Reflective Shadow"; } public PetCubReflectiveShadow(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubReflectiveShadow : BasePetCubSample { public override int TargetHue => 1910; public override string DyeName => "Reflective Shadow"; [Constructable] public SamplePetCubReflectiveShadow() : base(1910) { Name = "Pet Dye Sample: Reflective Shadow"; } public SamplePetCubReflectiveShadow(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Liquid Sunshine (1923)
    public class PetCubLiquidSunshine : BasePetCubPermanent { public override int TargetHue => 1923; public override string DyeName => "Liquid Sunshine"; [Constructable] public PetCubLiquidSunshine() : base(1923) { Name = "Pet Dye Tub: Liquid Sunshine"; } public PetCubLiquidSunshine(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubLiquidSunshine : BasePetCubSample { public override int TargetHue => 1923; public override string DyeName => "Liquid Sunshine"; [Constructable] public SamplePetCubLiquidSunshine() : base(1923) { Name = "Pet Dye Sample: Liquid Sunshine"; } public SamplePetCubLiquidSunshine(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Shadowy Blue (1960)
    public class PetCubShadowyBlue : BasePetCubPermanent { public override int TargetHue => 1960; public override string DyeName => "Shadowy Blue"; [Constructable] public PetCubShadowyBlue() : base(1960) { Name = "Pet Dye Tub: Shadowy Blue"; } public PetCubShadowyBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubShadowyBlue : BasePetCubSample { public override int TargetHue => 1960; public override string DyeName => "Shadowy Blue"; [Constructable] public SamplePetCubShadowyBlue() : base(1960) { Name = "Pet Dye Sample: Shadowy Blue"; } public SamplePetCubShadowyBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Black And Green (1979)
    public class PetCubBlackAndGreen : BasePetCubPermanent { public override int TargetHue => 1979; public override string DyeName => "Black And Green"; [Constructable] public PetCubBlackAndGreen() : base(1979) { Name = "Pet Dye Tub: Black And Green"; } public PetCubBlackAndGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubBlackAndGreen : BasePetCubSample { public override int TargetHue => 1979; public override string DyeName => "Black And Green"; [Constructable] public SamplePetCubBlackAndGreen() : base(1979) { Name = "Pet Dye Sample: Black And Green"; } public SamplePetCubBlackAndGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Glossy Blue (1916)
    public class PetCubGlossyBlue : BasePetCubPermanent { public override int TargetHue => 1916; public override string DyeName => "Glossy Blue"; [Constructable] public PetCubGlossyBlue() : base(1916) { Name = "Pet Dye Tub: Glossy Blue"; } public PetCubGlossyBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubGlossyBlue : BasePetCubSample { public override int TargetHue => 1916; public override string DyeName => "Glossy Blue"; [Constructable] public SamplePetCubGlossyBlue() : base(1916) { Name = "Pet Dye Sample: Glossy Blue"; } public SamplePetCubGlossyBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Hunter Green (1936)
    public class PetCubHunterGreen : BasePetCubPermanent { public override int TargetHue => 1936; public override string DyeName => "Hunter Green"; [Constructable] public PetCubHunterGreen() : base(1936) { Name = "Pet Dye Tub: Hunter Green"; } public PetCubHunterGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubHunterGreen : BasePetCubSample { public override int TargetHue => 1936; public override string DyeName => "Hunter Green"; [Constructable] public SamplePetCubHunterGreen() : base(1936) { Name = "Pet Dye Sample: Hunter Green"; } public SamplePetCubHunterGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Slate Blue (1983)
    public class PetCubSlateBlue : BasePetCubPermanent { public override int TargetHue => 1983; public override string DyeName => "Slate Blue"; [Constructable] public PetCubSlateBlue() : base(1983) { Name = "Pet Dye Tub: Slate Blue"; } public PetCubSlateBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubSlateBlue : BasePetCubSample { public override int TargetHue => 1983; public override string DyeName => "Slate Blue"; [Constructable] public SamplePetCubSlateBlue() : base(1983) { Name = "Pet Dye Sample: Slate Blue"; } public SamplePetCubSlateBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Mother Of Pearl (2720)
    public class PetCubMotherOfPearl : BasePetCubPermanent { public override int TargetHue => 2720; public override string DyeName => "Mother Of Pearl"; [Constructable] public PetCubMotherOfPearl() : base(2720) { Name = "Pet Dye Tub: Mother Of Pearl"; } public PetCubMotherOfPearl(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubMotherOfPearl : BasePetCubSample { public override int TargetHue => 2720; public override string DyeName => "Mother Of Pearl"; [Constructable] public SamplePetCubMotherOfPearl() : base(2720) { Name = "Pet Dye Sample: Mother Of Pearl"; } public SamplePetCubMotherOfPearl(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Star Blue (2723)
    public class PetCubStarBlue : BasePetCubPermanent { public override int TargetHue => 2723; public override string DyeName => "Star Blue"; [Constructable] public PetCubStarBlue() : base(2723) { Name = "Pet Dye Tub: Star Blue"; } public PetCubStarBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubStarBlue : BasePetCubSample { public override int TargetHue => 2723; public override string DyeName => "Star Blue"; [Constructable] public SamplePetCubStarBlue() : base(2723) { Name = "Pet Dye Sample: Star Blue"; } public SamplePetCubStarBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Murky Amber (1989)
    public class PetCubMurkyAmber : BasePetCubPermanent { public override int TargetHue => 1989; public override string DyeName => "Murky Amber"; [Constructable] public PetCubMurkyAmber() : base(1989) { Name = "Pet Dye Tub: Murky Amber"; } public PetCubMurkyAmber(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubMurkyAmber : BasePetCubSample { public override int TargetHue => 1989; public override string DyeName => "Murky Amber"; [Constructable] public SamplePetCubMurkyAmber() : base(1989) { Name = "Pet Dye Sample: Murky Amber"; } public SamplePetCubMurkyAmber(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Vibran Seagreen (1970)
    public class PetCubVibranSeagreen : BasePetCubPermanent { public override int TargetHue => 1970; public override string DyeName => "Vibran Seagreen"; [Constructable] public PetCubVibranSeagreen() : base(1970) { Name = "Pet Dye Tub: Vibran Seagreen"; } public PetCubVibranSeagreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubVibranSeagreen : BasePetCubSample { public override int TargetHue => 1970; public override string DyeName => "Vibran Seagreen"; [Constructable] public SamplePetCubVibranSeagreen() : base(1970) { Name = "Pet Dye Sample: Vibran Seagreen"; } public SamplePetCubVibranSeagreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Vibrant Ocher (2725)
    public class PetCubVibrantOcher : BasePetCubPermanent { public override int TargetHue => 2725; public override string DyeName => "Vibrant Ocher"; [Constructable] public PetCubVibrantOcher() : base(2725) { Name = "Pet Dye Tub: Vibrant Ocher"; } public PetCubVibrantOcher(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubVibrantOcher : BasePetCubSample { public override int TargetHue => 2725; public override string DyeName => "Vibrant Ocher"; [Constructable] public SamplePetCubVibrantOcher() : base(2725) { Name = "Pet Dye Sample: Vibrant Ocher"; } public SamplePetCubVibrantOcher(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Mossy Green (2684)
    public class PetCubMossyGreen : BasePetCubPermanent { public override int TargetHue => 2684; public override string DyeName => "Mossy Green"; [Constructable] public PetCubMossyGreen() : base(2684) { Name = "Pet Dye Tub: Mossy Green"; } public PetCubMossyGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubMossyGreen : BasePetCubSample { public override int TargetHue => 2684; public override string DyeName => "Mossy Green"; [Constructable] public SamplePetCubMossyGreen() : base(2684) { Name = "Pet Dye Sample: Mossy Green"; } public SamplePetCubMossyGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Olive Green (2709)
    public class PetCubOliveGreen : BasePetCubPermanent { public override int TargetHue => 2709; public override string DyeName => "Olive Green"; [Constructable] public PetCubOliveGreen() : base(2709) { Name = "Pet Dye Tub: Olive Green"; } public PetCubOliveGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubOliveGreen : BasePetCubSample { public override int TargetHue => 2709; public override string DyeName => "Olive Green"; [Constructable] public SamplePetCubOliveGreen() : base(2709) { Name = "Pet Dye Sample: Olive Green"; } public SamplePetCubOliveGreen(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Mottled Sunset Blue (2714)
    public class PetCubMottledSunsetBlue : BasePetCubPermanent { public override int TargetHue => 2714; public override string DyeName => "Mottled Sunset Blue"; [Constructable] public PetCubMottledSunsetBlue() : base(2714) { Name = "Pet Dye Tub: Mottled Sunset Blue"; } public PetCubMottledSunsetBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubMottledSunsetBlue : BasePetCubSample { public override int TargetHue => 2714; public override string DyeName => "Mottled Sunset Blue"; [Constructable] public SamplePetCubMottledSunsetBlue() : base(2714) { Name = "Pet Dye Sample: Mottled Sunset Blue"; } public SamplePetCubMottledSunsetBlue(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Tyrian Purple (2716)
    public class PetCubTyrianPurple : BasePetCubPermanent { public override int TargetHue => 2716; public override string DyeName => "Tyrian Purple"; [Constructable] public PetCubTyrianPurple() : base(2716) { Name = "Pet Dye Tub: Tyrian Purple"; } public PetCubTyrianPurple(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubTyrianPurple : BasePetCubSample { public override int TargetHue => 2716; public override string DyeName => "Tyrian Purple"; [Constructable] public SamplePetCubTyrianPurple() : base(2716) { Name = "Pet Dye Sample: Tyrian Purple"; } public SamplePetCubTyrianPurple(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }

    // Intense Teal (2691)
    public class PetCubIntenseTeal : BasePetCubPermanent { public override int TargetHue => 2691; public override string DyeName => "Intense Teal"; [Constructable] public PetCubIntenseTeal() : base(2691) { Name = "Pet Dye Tub: Intense Teal"; } public PetCubIntenseTeal(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
    public class SamplePetCubIntenseTeal : BasePetCubSample { public override int TargetHue => 2691; public override string DyeName => "Intense Teal"; [Constructable] public SamplePetCubIntenseTeal() : base(2691) { Name = "Pet Dye Sample: Intense Teal"; } public SamplePetCubIntenseTeal(Serial serial) : base(serial) { }  public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); } public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); } }
}
