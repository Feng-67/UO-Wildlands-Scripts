/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 *
 * Hostile variant of the Rising Colossus for use in wave spawners.
 * Stats are fixed at the high end of the summon scaling range.
 */
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class OrcHoundStatue : Item
    {
        [Constructable]
        public OrcHoundStatue() : base(0x211C) // Dog statue UO graphic
        {
            Name = "Orc Hound Statue (Double Click To Claim)";
            Weight = 1.0;
            Hue = 1934; // Matching the Orc theme hue
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (from.Skills[SkillName.AnimalTaming].Base < 100.0)
            {
                from.SendMessage(38, "You must be a Grandmaster Tamer to claim this ferocious beast.");
                return;
            }

            if ((from.Followers + 1) > from.FollowersMax) // Orc Hounds take 1 slot
            {
                from.SendLocalizedMessage(1049611); // You have too many followers to summon that creature.
                return;
            }

            OrcHound pet = new OrcHound();
            pet.Controlled = true;
            pet.ControlMaster = from;
            pet.ControlOrder = OrderType.Follow;
            pet.ControlTarget = from;
            pet.MoveToWorld(from.Location, from.Map);

            from.SendMessage(68, "You successfully tame the Orc Hound!");
            this.Delete();
        }

        public OrcHoundStatue(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
