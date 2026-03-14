/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using Server.Multis;

namespace Server.Items
{
    public abstract class VirtualStorageChest : Item
    {
        // The Virtual List: Stores Item Type and Quantity
        public Dictionary<Type, int> Content = new Dictionary<Type, int>();

        public abstract Type[] AllowedTypes { get; }
        public abstract string ChestTitle { get; }

        public VirtualStorageChest(int itemID) : base(itemID)
        {
            Movable = true;
            Weight = 10.0; 
        }

        public VirtualStorageChest(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500446); // Too far away.
                return;
            }

            if (!IsLockedDown && !IsSecure)
            {
                from.SendMessage("This storage chest must be secured in a house to function.");
                return;
            }

            from.SendGump(new VirtualStorageGump(from, this));
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (!IsLockedDown && !IsSecure)
            {
                from.SendMessage("This storage chest must be secured in a house to function.");
                return false;
            }

            // Using the new int return type
            if (TryAdd(dropped) > 0)
            {
                from.PlaySound(0x42);
                from.SendGump(new VirtualStorageGump(from, this));
                return true;
            }

            from.SendMessage("That item does not belong in this stockpile.");
            return false;
        }

        // Returns the amount added, or 0 if failed
        public int TryAdd(Item item)
        {
            Type t = item.GetType();
            bool isAllowed = false;

            foreach (Type a in AllowedTypes) 
            { 
                if (t == a || t.IsSubclassOf(a)) 
                { 
                    isAllowed = true; 
                    break; 
                } 
            }

            if (isAllowed)
            {
                int amountToAdd = item.Amount;
                if (Content.ContainsKey(t)) Content[t] += amountToAdd;
                else Content[t] = amountToAdd;

                item.Delete();
                return amountToAdd;
            }
            return 0;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(Content.Count);
            foreach (var kvp in Content) { writer.Write(kvp.Key.FullName); writer.Write(kvp.Value); }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                string typeName = reader.ReadString();
                int amt = reader.ReadInt();
                Type t = Type.GetType(typeName);
                if (t != null) Content[t] = amt;
            }
        }
    }
}
