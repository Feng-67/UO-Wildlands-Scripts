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
    public class PetDyeSampleVividCrimson : Item
    {
        private int m_TargetHue = 1157; // Vivid Crimson
        private string m_DyeName = "Vivid Crimson";

        [Constructable]
        public PetDyeSampleVividCrimson() : base(0xEFB) // Graphic: Potion/Powder Pot
        {
            Name = "Pet Dye Sample: " + m_DyeName;
            Hue = m_TargetHue;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            // Yellow font for the sample text
            list.Add(1114057, "<BASEFONT COLOR=#FFFF00>30 sec Sample Pet Dye</BASEFONT>");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 1))
            {
                from.SendMessage("Target the pet you wish to sample the {0} dye on.", m_DyeName);
                from.Target = new InternalPetTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(500446);
            }
        }

        private class InternalPetTarget : Target
        {
            private PetDyeSampleVividCrimson m_Sample;

            public InternalPetTarget(PetDyeSampleVividCrimson sample) : base(1, false, TargetFlags.None)
            {
                m_Sample = sample;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature pet && pet.ControlMaster == from)
                {
                    if (!from.InRange(m_Sample.GetWorldLocation(), 1) || !from.InRange(pet.Location, 1))
                    {
                        from.SendLocalizedMessage(500446);
                    }
                    else
                    {
                        int oldHue = pet.Hue; // Store the original color
                        pet.Hue = m_Sample.m_TargetHue;
                        from.PlaySound(0x23E);
                        from.SendMessage("The sample is applied! Your pet will revert in 30 seconds.");
                        
                        // Start the 30-second timer to revert the color
                        Timer.DelayCall(TimeSpan.FromSeconds(30.0), new TimerStateCallback(RevertHue), new object[] { pet, oldHue });

                        m_Sample.Delete(); 
                    }
                }
                else
                {
                    from.SendMessage("You can only dye your own pets!");
                }
            }

            private void RevertHue(object state)
            {
                object[] states = (object[])state;
                BaseCreature pet = (BaseCreature)states[0];
                int oldHue = (int)states[1];

                if (pet != null && !pet.Deleted)
                {
                    pet.Hue = oldHue;
                    if (pet.ControlMaster != null)
                        pet.ControlMaster.SendMessage("The {0} sample has worn off.", m_Sample != null ? m_Sample.m_DyeName : "dye");
                }
            }
        }

        public PetDyeSampleVividCrimson(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
