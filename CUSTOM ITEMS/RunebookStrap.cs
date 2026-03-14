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
    public class RunebookStrap : BaseContainer
    {
        [Constructable]
        public RunebookStrap() : base(0xA721)
        {
            Name = "Runebook Strap";
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Weight Reduction: 100%");
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Runebook || dropped is RunicAtlas)
                return base.OnDragDrop(from, dropped);

            from.SendMessage("This strap can only hold runebooks or runic atlases.");
            return false;
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (item is Runebook || item is RunicAtlas)
                return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
            
            if (message)
                m.SendMessage("This strap can only hold runebooks or runic atlases.");
                
            return false;
        }

        // FORCE CALCULATED WEIGHT TO 0
        public override int GetTotal(TotalType type)
        {
            if (type == TotalType.Weight)
            {
                // Ignores content weight entirely
                return 0;
            }
            return base.GetTotal(type);
        }

        // Backup method
        public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            if (type == TotalType.Weight)
                delta = 0;

            base.UpdateTotal(sender, type, delta);
        }

        public RunebookStrap(Serial serial) : base(serial)
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
        }
    }
}
