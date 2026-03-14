/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class PetDyeTubVividCrimson : Item
    {
        // Custom Settings for this specific file
        private int m_TargetHue = 1157; // Vivid Crimson
        private string m_DyeName = "Vivid Crimson";

        [Constructable]
        public PetDyeTubVividCrimson() : base(0xFAB) // Dye Tub Graphic
        {
            Name = "Pet Dye Tub: " + m_DyeName;
            Hue = m_TargetHue;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1114057, "<BASEFONT COLOR=#FFFF00>Single Use Only</BASEFONT>");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 1))
            {
                from.SendMessage("Target the pet you wish to dye {0}.", m_DyeName);
                from.Target = new InternalPetTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }

        private class InternalPetTarget : Target
        {
            private PetDyeTubVividCrimson m_Tub;

            public InternalPetTarget(PetDyeTubVividCrimson tub) : base(1, false, TargetFlags.None)
            {
                m_Tub = tub;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature pet && pet.ControlMaster == from)
                {
                    if (!from.InRange(m_Tub.GetWorldLocation(), 1) || !from.InRange(pet.Location, 1))
                    {
                        from.SendLocalizedMessage(500446);
                    }
                    else
                    {
                        pet.Hue = m_Tub.m_TargetHue;
                        from.PlaySound(0x23E);
                        from.SendMessage("The dye tub vanishes as your pet turns {0}!", m_Tub.m_DyeName);
                        
                        m_Tub.Delete(); // One-time use logic
                    }
                }
                else
                {
                    from.SendMessage("You can only dye your own pets!");
                }
            }
        }

        public PetDyeTubVividCrimson(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
