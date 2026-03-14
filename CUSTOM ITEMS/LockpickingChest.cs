/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
    public class LockpickingChest : LockableContainer
    {
       // private bool m_Locked;

        [Constructable]
        public LockpickingChest(): base(0x9AA)
        {
			Name = "Lockpicking Chest";
            Locked = true;
            LockLevel = 1;
			MaxLockLevel = 100; // edit here to go as high as you want for your shard. 95 is osi style, 100 is max, higher with power scrolls or however you set it up in your shard.
            RequiredSkill = 1;
			Movable = true;  // set to true if you make available to players
            Weight = 4.0;

        }
// --- PLACE IT HERE ---
        public override void OnDoubleClick(Mobile from)
        {
            double currentSkill = from.Skills[SkillName.Lockpicking].Value;
            this.RequiredSkill = (int)currentSkill;
            this.LockLevel = (int)currentSkill;

            from.SendMessage("The chest's tumblers shift to match your expertise.");

            base.OnDoubleClick(from);
        }
// ---------------------

        public override void LockPick(Mobile from)
        {
            this.Locked = true;
            from.SendMessage("The container magically relocks it self.");
        }
        public LockpickingChest(Serial serial)
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
