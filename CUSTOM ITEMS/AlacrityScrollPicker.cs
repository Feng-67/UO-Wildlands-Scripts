/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class AlacrityScrollPicker : Item
    {
        [Constructable]
        public AlacrityScrollPicker() : base(0x9981) // Scroll of Alacrity Book Graphic
        {
            Name = "Alacrity Scroll Picker";
            Hue = 2601; // Unique Hue to distinguish from a normal book
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack.
                return;
            }

            // Check if player is already under an effect (Using your existing system check)
            PlayerMobile pm = from as PlayerMobile;
            if (pm != null && pm.AcceleratedSkill != SkillName.Alchemy && pm.AcceleratedSkill != (SkillName)0) 
            {
                // Note: Standard ServUO uses Alchemy as the 'null' check or a separate flag. 
                // We will check your specific Alacrity Table.
            }

            from.CloseGump(typeof(AlacrityPickerGump));
            from.SendGump(new AlacrityPickerGump(from, this));
        }

        public AlacrityScrollPicker(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class AlacrityPickerGump : Gump
    {
        private AlacrityScrollPicker m_Picker;

        public AlacrityPickerGump(Mobile from, AlacrityScrollPicker picker) : base(50, 50)
        {
            m_Picker = picker;

            AddPage(0);
            AddBackground(0, 0, 300, 450, 9270);
            AddAlphaRegion(10, 10, 280, 430);
            AddHtml(10, 20, 280, 20, "<BASEFONT COLOR=#FFA500><CENTER>SELECT ALACRITY SKILL</CENTER></BASEFONT>", false, false);

            AddPage(1);
            int y = 50;
            int count = 0;
            int page = 1;

            for (int i = 0; i < SkillInfo.Table.Length; ++i)
            {
                if (count >= 10)
                {
                    AddButton(240, 410, 0xFA5, 0xFA7, 0, GumpButtonType.Page, page + 1);
                    page++;
                    AddPage(page);
                    AddButton(20, 410, 0xFAE, 0xFAF, 0, GumpButtonType.Page, page - 1);
                    count = 0;
                    y = 50;
                }

                SkillInfo info = SkillInfo.Table[i];
                AddButton(20, y, 0x4B9, 0x4BA, i + 1, GumpButtonType.Reply, 0);
                AddLabel(45, y - 2, 0x481, info.Name);

                y += 35;
                count++;
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            PlayerMobile pm = from as PlayerMobile;

            if (m_Picker == null || m_Picker.Deleted || info.ButtonID == 0 || pm == null)
                return;

            int skillIndex = info.ButtonID - 1;

            if (skillIndex >= 0 && skillIndex < SkillInfo.Table.Length)
            {
                SkillName selectedSkill = (SkillName)skillIndex;

                // --- START ACTIVE ALACRITY LOGIC (Mirrors your ScrollOfAlacrity.cs) ---
                
                // 1. End any existing alacrity first
                ScrollOfAlacrity.AlacrityEnd(from);

                // 2. Set the skill on the player
                pm.AcceleratedSkill = selectedSkill;

                // 3. Add the Buff Icon (Icon 0x41F is Arcane Empowerment, used by Alacrity)
                // Using clilocs 1078511 (Scroll of Alacrity) and 1078512 (Accelerated gain in...)
                BuffInfo.AddBuff(pm, new BuffInfo(BuffIcon.ArcaneEmpowerment, 1078511, 1078512, TimeSpan.FromMinutes(15), pm, selectedSkill.ToString()));

                // 4. Start the 15-minute timer
                Timer.DelayCall(TimeSpan.FromMinutes(15), new TimerStateCallback(Expire_Callback), from);

                // 5. Cleanup and Messaging
                from.PlaySound(0xF5); // Use your scroll sound
                from.SendMessage(0x35, "You have initiated accelerated skillgain for {0}!", selectedSkill.ToString());
                
                m_Picker.Delete(); 
            }
        }

        private void Expire_Callback(object state)
        {
            ScrollOfAlacrity.AlacrityEnd((Mobile)state);
        }
    }
}
