/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.BulkOrders;

namespace Server.Items
{
    public class LargeBODBox : Container
    {
        // List to store Serials of processed LargeBODs
        private List<Serial> m_ProcessedDeeds;

        public override int DefaultGumpID { get { return 0x43; } }
        public override int DefaultDropSound { get { return 0x42; } }

        public override Rectangle2D Bounds => new Rectangle2D(16, 51, 168, 73);

        [Constructable]
        public LargeBODBox() : base(0x9AA)
        {
            Name = "Large BOD Deed Box";
            Weight = 2.0;
            m_ProcessedDeeds = new List<Serial>();
        }

        public LargeBODBox(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            // 1. Always open the container gump first
            base.OnDoubleClick(from);

            // 2. Location Check
            if (!IsChildOf(from.Backpack) && !IsChildOf(from.BankBox) && !IsSecure)
            {
                from.SendMessage("This must be in your backpack, bank box, or secured in your house to use.");
                return;
            }

            // 3. Find the Large BOD
            LargeBOD largeBOD = null;
            foreach (Item item in Items)
            {
                if (item is LargeBOD)
                {
                    largeBOD = (LargeBOD)item;
                    break;
                }
            }

            // 4. If no BOD is found, stop here (the box is already open from step 1)
            if (largeBOD == null)
            {
                return;
            }

            // 5. Anti-Exploit Check (Serial Tracking)
            if (m_ProcessedDeeds.Contains(largeBOD.Serial))
            {
                from.SendMessage("This specific Large BOD has already been split.");
                return;
            }

            // 6. Process
            if (GenerateSmallBODs(from, largeBOD))
            {
                m_ProcessedDeeds.Add(largeBOD.Serial);
                from.SendMessage("The Small BOD components have been generated.");
            }
        }

        private bool GenerateSmallBODs(Mobile from, LargeBOD largeBOD)
        {
            Type smallType = GetSmallBODType(largeBOD.BODType);

            if (smallType == null)
            {
                from.SendMessage("Unsupported BOD type.");
                return false;
            }

            foreach (var entry in largeBOD.Entries)
            {
                SmallBOD small = (SmallBOD)Activator.CreateInstance(smallType);
                small.AmountMax = largeBOD.AmountMax;
                small.RequireExceptional = largeBOD.RequireExceptional;
                small.Material = largeBOD.Material;

                if (entry.Details != null)
                {
                    small.Type = entry.Details.Type;
                    small.Number = entry.Details.Number;
                }

                this.DropItem(small);
            }
            return true;
        }

        private Type GetSmallBODType(BODType type)
        {
            switch (type)
            {
                case BODType.Smith: return typeof(SmallSmithBOD);
                case BODType.Tailor: return typeof(SmallTailorBOD);
                case BODType.Alchemy: return typeof(SmallAlchemyBOD);
                case BODType.Inscription: return typeof(SmallInscriptionBOD);
                case BODType.Tinkering: return typeof(SmallTinkerBOD);
                case BODType.Cooking: return typeof(SmallCookingBOD);
                case BODType.Fletching: return typeof(SmallFletchingBOD);
                case BODType.Carpentry: return typeof(SmallCarpentryBOD);
                default: return null;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // Version

            writer.Write(m_ProcessedDeeds.Count);
            foreach (Serial s in m_ProcessedDeeds)
                writer.Write(s);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_ProcessedDeeds = new List<Serial>();
            if (version >= 1)
            {
                int count = reader.ReadInt();
                for (int i = 0; i < count; i++)
                    m_ProcessedDeeds.Add(reader.ReadInt());
            }
        }
    }
}
