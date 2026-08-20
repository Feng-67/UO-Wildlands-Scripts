/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;

namespace Server.Items
{
    public class HooksShield : HeaterShield
    {
        public override bool IsArtifact { get { return true; } }

        // Override resistances here
        public override int BasePhysicalResistance { get { return 0; } }
        public override int BaseFireResistance { get { return 10; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 10; } }


        [Constructable]
        public HooksShield()
        {
            Name = "Hook's Shield";
            ItemID = 0xA64A; // Sets the graphic to Shield_Pirate
            //Hue = 1175; // Matches the dark pirate theme
            
            // Set Durability here
            this.MaxHitPoints = 255;
            this.HitPoints = 255;

            // Primary Attributes
            Attributes.SpellChanneling = 1;
            Attributes.CastSpeed = 1;         // Faster Casting 1
            Attributes.SpellDamage = 10;      // Spell Damage Increase 10%
            Attributes.DefendChance = 15;     // Defense Chance Increase 15%
                                   
            // Requirement adjustment
            ArmorAttributes.LowerStatReq = 100;
        }
          public override void GetProperties(ObjectPropertyList list)
{
          base.GetProperties(list);

          // This adds the "Artifact" tag to the tooltip if desired
  //        list.Add(1061078, "{0}\t{1}","Artifact", "Hook's Shield"); 
}
        public HooksShield(Serial serial) : base(serial)
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
