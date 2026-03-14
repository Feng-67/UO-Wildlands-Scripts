/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Multis;

namespace Server.Items
{
    public class DavyJonesPoker : Item // Inherit from Item to hide weapon stats
    {
        [Constructable]
        public DavyJonesPoker() : base(0xF62) // Standard Spear ItemID
        {
            Name = "Davy Jones' Iron Poker";
            Hue = 1315;
            LootType = LootType.Blessed;
            Weight = 7.0;
            Layer = Layer.OneHanded; // Allows it to be held/displayed without being a 'weapon'
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            from.SendMessage("Target a corpse in the water to sink it.");
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DavyJonesPoker m_Poker;

            public InternalTarget(DavyJonesPoker poker) : base(10, false, TargetFlags.None)
            {
                m_Poker = poker;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Poker.Deleted || !from.InRange(m_Poker.GetWorldLocation(), 2))
                    return;

                if (targeted is Corpse)
                {
                    Corpse c = (Corpse)targeted;

                    // 1. Boat Check
                    BaseBoat boat = BaseBoat.FindBoatAt(from);
                    if (boat == null)
                    {
                        from.SendMessage("You must be on a boat to use this.");
                        return;
                    }

                    // 2. Water Check
                    if (IsWater(c.Map, c.X, c.Y))
                    {
                        Effects.SendLocationEffect(c.Location, c.Map, 0x352E, 50); // Splash
                        from.PlaySound(0x026); // Splash sound

                        c.Delete();
                        from.SendMessage("You poke the remains into the depths.");
                    }
                    else
                    {
                        from.SendMessage("That corpse is not in the water.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a corpse.");
                }
            }

            private bool IsWater(Map map, int x, int y)
            {
                if (map == null) return false;

                LandTile landTile = map.Tiles.GetLandTile(x, y);
                int id = landTile.ID;

                if (id >= 168 && id <= 171) return true; // Ocean
                if (id >= 310 && id <= 311) return true; // Shallow water

                StaticTile[] statics = map.Tiles.GetStaticTiles(x, y);
                foreach (StaticTile t in statics)
                {
                    if (t.ID >= 6038 && t.ID <= 6066) return true;
                }

                return false;
            }
        }

        public DavyJonesPoker(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
