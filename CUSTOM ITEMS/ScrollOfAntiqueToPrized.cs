/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
    public class ScrollOfAntiqueToPrized : Item
    {
        [Constructable]
        public ScrollOfAntiqueToPrized() : base(0xA1E4) // Star Map East Graphic
        {
            Name = "Scroll of Antique To Prized";
            Weight = 1.0;
            //Hue = 1638; // Red Hue
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your backpack for you to use it.
                return;
            }

            from.SendMessage("Target the antique item you wish to make prized.");
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ScrollOfAntiqueToPrized _scroll;

            public InternalTarget(ScrollOfAntiqueToPrized scroll) : base(1, false, TargetFlags.None)
            {
                _scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_scroll.Deleted || !_scroll.IsChildOf(from.Backpack))
                    return;

                if (targeted is Item item)
                {
                    if (!item.IsChildOf(from.Backpack) && item.Parent != from)
                    {
                        from.SendMessage("The item must be in your backpack or equipped.");
                        return;
                    }

                    // We need to find the NegativeAttributes on the specific item type
                    NegativeAttributes attrs = null;

                    if (item is BaseWeapon) attrs = ((BaseWeapon)item).NegativeAttributes;
                    else if (item is BaseArmor) attrs = ((BaseArmor)item).NegativeAttributes;
                    else if (item is BaseJewel) attrs = ((BaseJewel)item).NegativeAttributes;
                    else if (item is BaseClothing) attrs = ((BaseClothing)item).NegativeAttributes;
                    else if (item is BaseTalisman) attrs = ((BaseTalisman)item).NegativeAttributes;

                    if (attrs != null && attrs.Antique != 0)
                    {
                        attrs.Antique = 0;
                        attrs.Prized = 1;

                        // Chivalry Remove Curse Sound & Red Sparkle Visual
                        from.PlaySound(0x5AA); 
                        from.FixedParticles(0x375A, 1, 15, 5001, 1638, 4, EffectLayer.Waist);
                        
                        item.InvalidateProperties(); 
                        from.SendMessage("The item is no longer antique; it is now a prized possession.");
                        _scroll.Delete(); 
                    }
                    else
                    {
                        from.SendMessage("That item is not an antique or cannot be modified.");
                    }
                }
                else
                {
                    from.SendMessage("You can only use this on items.");
                }
            }
        }

        public ScrollOfAntiqueToPrized(Serial serial) : base(serial) { }

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
