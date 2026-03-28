/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class MarkRuneStone : Item
    {
        [Constructable]
        public MarkRuneStone() : base(0x1F14) // Item ID for a runestone
        {
            Name = "Mark Rune";
            Hue = 0x481; // White hue
            LootType = LootType.Blessed;
            Weight = 1.0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("double click to mark a rune"); // Tooltip subtitle
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            // Check if player has 25 GP in bank
            if (Banker.Withdraw(from, 25))
            {
                RecallRune rune = new RecallRune();

                rune.Marked = true;
                rune.Target = from.Location;
                rune.TargetMap = from.Map;
                rune.Description = "a marked rune";

                if (from.AddToBackpack(rune))
                {
                    from.PlaySound(0x1FA); // Mark spell sound effect
                    from.SendMessage("25 gold has been withdrawn from your bank, and a marked rune has been placed in your backpack.");
                }
                else
                {
                    rune.Delete();
                    Banker.Deposit(from, 25); // Refund if backpack is full
                    from.SendMessage("Your backpack is full. The gold has been returned to your bank.");
                }
            }
            else
            {
                from.SendMessage("You do not have the 25 gold required in your bank to mark a rune.");
            }
        }

        public MarkRuneStone(Serial serial) : base(serial)
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
