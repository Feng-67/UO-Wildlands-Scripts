/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class InstrumentCase : WoodenBox
    {
        [Constructable]
        public InstrumentCase() : base()
        {
            Name = "Instrument Case";
            LootType = LootType.Blessed;
            // Original DecorativeBox hue and sounds are used by default
        }

        // Adds the subtext to the tooltip in a normal font color
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Weight Reduction: 50%");
        }

        // Prevents non-instrument items from being dropped in
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is BaseInstrument)
            {
                return base.OnDragDrop(from, dropped);
            }
            else
            {
                from.SendMessage("This case is specially padded only for musical instruments.");
                return false;
            }
        }

        // Ensures only instruments can be held (via targeting or other methods)
        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (item is BaseInstrument)
            {
                return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
            }
            
            if (message)
                m.SendMessage("This case can only hold musical instruments.");
                
            return false;
        }

        // Logic for the 50% Weight Reduction
        public override void UpdateTotal(Item abrownItem, TotalType type, int delta)
        {
            if (type == TotalType.Weight)
                delta = (int)(delta * 0.5);

            base.UpdateTotal(abrownItem, type, delta);
        }

        public InstrumentCase(Serial serial) : base(serial)
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
