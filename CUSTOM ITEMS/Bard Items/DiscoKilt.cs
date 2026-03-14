/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class DiscoKilt : Kilt
    {
        private SkillMod m_SkillMod;

        [Constructable]
        public DiscoKilt()
        {
            Name = "Kilt of Master Discordance";
            Hue = 1284; // Soft Pastel Blue
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            // Green colored line for the specific bonus
            list.Add("<BASEFONT COLOR=#00FF00>Over-Capped Skill Bonus +10 Discordance</BASEFONT>");
        }

        public override bool OnEquip(Mobile from)
        {
            if (base.OnEquip(from))
            {
                // true = absolute offset (allows it to go 120 -> 130)
                m_SkillMod = new DefaultSkillMod(SkillName.Discordance, true, 10.0);
                from.AddSkillMod(m_SkillMod);
                return true;
            }
            return false;
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (m_SkillMod != null)
            {
                m_SkillMod.Remove();
                m_SkillMod = null;
            }
        }

        public DiscoKilt(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
            // Re-applies the skill mod if the player was wearing it when the server restarted
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), ReapplyMod);
        }

        private void ReapplyMod()
        {
            Mobile m = Parent as Mobile;
            if (m != null)
            {
                m_SkillMod = new DefaultSkillMod(SkillName.Discordance, true, 10.0);
                m.AddSkillMod(m_SkillMod);
            }
        }
    }
}
