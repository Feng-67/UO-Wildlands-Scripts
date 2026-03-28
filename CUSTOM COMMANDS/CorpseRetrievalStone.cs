/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Rutibex)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;

namespace Server
{
	public static class ResurrectHelper
	{
		public static int ResurrectRange = 3;
				
		public static void Resurrect(Mobile m, Item item)
		{
			if (m.Alive)
			{
				return;
			}

			if (!m.InRange(item.GetWorldLocation(), ResurrectRange))
			{
				m.SendLocalizedMessage(500446); // That is too far away.
			}
			else if (m.Map != null && m.Map.CanFit(m.Location, 16, false, false))
			{
				m.CloseGump(typeof(ResurrectGump));
				
				m.SendGump(new ResurrectGump(m, ResurrectMessage.Generic));
			}
			else
			{
				m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
			}
		}

        public static void Initialize()
        {
            EventSink.PlayerDeath += OnPlayerDeath;
        }

        private static void OnPlayerDeath(PlayerDeathEventArgs e)
        {
            Mobile m = e.Mobile;
            if (m != null && !m.Deleted)
            {
                m.SendGump(new DeathTeleportGump(m));
            }
        }

        public static Item FindStone()
        {
            foreach (Item item in World.Items.Values)
            {
                if (item is CorpseRetrievalStoneWest || item is CorpseRetrievalStoneNorth)
                {
                    return item;
                }
            }
            return null;
        }

        public class ResurrectEntry : ContextMenuEntry
		{
			private readonly Mobile m_Mobile;
			
			private readonly Item m_Item;
			
			public ResurrectEntry(Mobile mobile, Item item) : base(6195, ResurrectRange)
			{
				m_Mobile = mobile;
				
				m_Item = item;

				Enabled = !m_Mobile.Alive;
			}

			public override void OnClick()
			{
				Resurrect(m_Mobile, m_Item);
			}
		}
	}
}

namespace Server.Items
{
	public class CorpseRetrievalStoneWest : Item
	{
		private double totalweight;
		
		private int totalCost;
		
		private List<Item> items;
		
		private InternalItem m_Item;
		
		[Constructable]
		public CorpseRetrievalStoneWest() : this(false)
		{
		}

		[Constructable]
		public CorpseRetrievalStoneWest(bool bloodied) : base(bloodied ? 0x1D98 : 0x3)
		{
			Name = "a corpse retrieval stone";
			
			Movable = false;
			
			m_Item = new InternalItem(bloodied, this);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			if( !from.Alive )
			{
				list.Add(new ResurrectHelper.ResurrectEntry(from, this));
			}
		}

		public override void OnMovement(Mobile m, Point3D oldLocation) //increase range to 3
		{
			if (Parent == null && Utility.InRange(Location, m.Location, ResurrectHelper.ResurrectRange)
				&& !Utility.InRange(Location, oldLocation, ResurrectHelper.ResurrectRange))
			{
				ResurrectHelper.Resurrect(m, this);
			}
		}

		public override void OnSingleClick(Mobile m)
		{
			ResurrectHelper.Resurrect(m, this);
		}

		public override void OnDoubleClickDead(Mobile m)
		{
			ResurrectHelper.Resurrect(m, this);
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.CloseGump(typeof(CorpseRetrievalStoneGump));

			if (Utility.InRange(Location, from.Location, ResurrectHelper.ResurrectRange))
			{
				if (GetItems(from))
				{
					from.SendGump(new CorpseRetrievalStoneGump(from, items, totalCost));
				}
				else
				{
	   				from.SendLocalizedMessage(1080107); // I'm sorry, I have nothing for you at this time.
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}
		
		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
			{
				m_Item.Location = new Point3D(X, Y + 1, Z);
			}
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
			{
				m_Item.Map = Map;
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
			{
				m_Item.Delete();
			}
		}

		private bool GetItems(Mobile from)
		{
			List<Item> worldItems = new List<Item>(World.Items.Values);
			
			List<Corpse> corpses = new List<Corpse>();
			
			foreach (Item i in worldItems)
			{
				if (i is Corpse && ((Corpse)i).Owner == from)
				{
					corpses.Add(((Corpse)i));
				}
			}
			
			items = new List<Item>();
			
			totalweight = 0.0;
	
			if (corpses.Count > 0)
			{
				foreach (Corpse corpse in corpses)
				{
                    foreach (Item item in corpse.Items)
                    {
                        if (item != null && !(item.Deleted))
                        {
                            // Destroy gold instead of retrieving it
                            if (item is Gold)
                            {
                                item.Delete();
                            }
                            else
                            {
                                items.Add(item);
                                totalweight += item.Weight;
                            }
                        }
                    }
                }
            }

            totalCost = 15000;

            return items.Count > 0;
		}
		
		public CorpseRetrievalStoneWest(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
		}

		private class InternalItem : Item
		{
			private CorpseRetrievalStoneWest m_Item;
			
			public InternalItem(bool bloodied, CorpseRetrievalStoneWest item) : base(bloodied ? 0x1D97 : 0x2)
			{
				Name = "a corpse retrieval stone";
				
				Movable = false;
				
				m_Item = item;
			}

			public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
			{
				m_Item.GetContextMenuEntries(from, list);
			}
	
			public override bool HandlesOnMovement => true; // Tell the core that we implement OnMovement

			public override void OnMovement(Mobile m, Point3D oldLocation)
			{
				m_Item.OnMovement(m, oldLocation);
			}

			public override void OnSingleClick(Mobile m)
			{
				m_Item.OnSingleClick(m);
			}
	
			public override void OnDoubleClickDead(Mobile m)
			{
				m_Item.OnDoubleClickDead(m);
			}

			public override void OnDoubleClick(Mobile from)
			{
				m_Item.OnDoubleClick(from);
			}
				
			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
				{
					m_Item.Location = new Point3D(X, Y - 1, Z);
				}
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
				{
					m_Item.Map = Map;
				}
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
				{
					m_Item.Delete();
				}
			}

			public InternalItem(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as CorpseRetrievalStoneWest;
			}
		}
	}

	[TypeAlias("Server.Items.CorpseRetrievalStoneEast")]
	public class CorpseRetrievalStoneNorth : Item
	{
		private double totalweight;
		
		private int totalCost;
		
		private List<Item> items;
		
		private InternalItem m_Item;
		
		[Constructable]
		public CorpseRetrievalStoneNorth() : this(false)
		{
		}

		[Constructable]
		public CorpseRetrievalStoneNorth(bool bloodied) : base(bloodied ? 0x1E5D : 0x4)
		{
			Name = "a corpse retrieval stone";
			
			Movable = false;
			
			m_Item = new InternalItem(bloodied, this);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			if( !from.Alive )
			{
				list.Add(new ResurrectHelper.ResurrectEntry(from, this));
			}
		}

		public override void OnMovement(Mobile m, Point3D oldLocation) //increase range to 3
		{
			if (Parent == null && Utility.InRange(Location, m.Location, ResurrectHelper.ResurrectRange)
				&& !Utility.InRange(Location, oldLocation, ResurrectHelper.ResurrectRange))
			{
				ResurrectHelper.Resurrect(m, this);
			}
		}

		public override void OnSingleClick(Mobile m)
		{
			ResurrectHelper.Resurrect(m, this);
		}

		public override void OnDoubleClickDead(Mobile m)
		{
			ResurrectHelper.Resurrect(m, this);
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.CloseGump(typeof(CorpseRetrievalStoneGump));

			if (Utility.InRange(Location, from.Location, ResurrectHelper.ResurrectRange))
			{
				if (GetItems(from))
				{
					from.SendGump(new CorpseRetrievalStoneGump(from, items, totalCost));
				}
				else
				{
	   				from.SendLocalizedMessage(1080107); // I'm sorry, I have nothing for you at this time.
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}
		
		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
			{
				m_Item.Location = new Point3D(X + 1, Y, Z);
			}
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
			{
				m_Item.Map = Map;
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
			{
				m_Item.Delete();
			}
		}

		private bool GetItems(Mobile from)
		{
			List<Item> worldItems = new List<Item>(World.Items.Values);
			
			List<Corpse> corpses = new List<Corpse>();
			
			foreach (Item i in worldItems)
			{
				if (i is Corpse && ((Corpse)i).Owner == from)
				{
					corpses.Add(((Corpse)i));
				}
			}
			
			items = new List<Item>();
			
			totalweight = 0.0;
	
			if (corpses.Count > 0)
			{
				foreach (Corpse corpse in corpses)
				{
                    foreach (Item item in corpse.Items)
                    {
                        if (item != null && !(item.Deleted))
                        {
                            // Destroy gold instead of retrieving it
                            if (item is Gold)
                            {
                                item.Delete();
                            }
                            else
                            {
                                items.Add(item);
                                totalweight += item.Weight;
                            }
                        }
                    }
                }
            }

            totalCost = 15000;

            return items.Count > 0;
		}
		
		public CorpseRetrievalStoneNorth(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
		}

		[TypeAlias("Server.Items.CorpseRetrievalStoneNorth+InternalItem")]
		private class InternalItem : Item
		{
			private CorpseRetrievalStoneNorth m_Item;
			
			public InternalItem(bool bloodied, CorpseRetrievalStoneNorth item) : base(bloodied ? 0x1E5C : 0x5)
			{
				Name = "a corpse retrieval stone";
				
				Movable = false;
				
				m_Item = item;
			}
			
			public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
			{
				m_Item.GetContextMenuEntries(from, list);
			}
	
			public override bool HandlesOnMovement => true; // Tell the core that we implement OnMovement

			public override void OnMovement(Mobile m, Point3D oldLocation)
			{
				m_Item.OnMovement(m, oldLocation);
			}

			public override void OnSingleClick(Mobile m)
			{
				m_Item.OnSingleClick(m);
			}

			public override void OnDoubleClickDead(Mobile m)
			{
				m_Item.OnDoubleClickDead(m);
			}

			public override void OnDoubleClick(Mobile from)
			{
				m_Item.OnDoubleClick(from);
			}
				
			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
				{
					m_Item.Location = new Point3D(X - 1, Y, Z);
				}
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
				{
					m_Item.Map = Map;
				}
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
				{
					m_Item.Delete();
				}
			}

			public InternalItem(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as CorpseRetrievalStoneNorth;
			}
		}
	}
}
	
namespace Server.Gumps
{
	public class CorpseRetrievalStoneGump : Gump
	{
		private Mobile m_From;
		
		private List<Item> m_Items;
		
		private int m_TotalCost;

		public CorpseRetrievalStoneGump(Mobile from, List<Item> items, int totalCost) : base(0, 0)
		{
			m_From = from;
			
			m_Items = items;
			
			m_TotalCost = totalCost;
			
			AddPage(0);

			AddBackground(0, 0, 400, 350, 2600);

			AddHtml(0, 20, 400, 35, @"<center><strong>Corpse Retrival System</strong></center>", false, false);
			
			AddHtml(50, 55, 300, 140, String.Format(
					"It is possible for your corpse items to be returned to you.<br>" +
					"It will cost you {0} Gold. <br>" +
					"CONTINUE - Bring my items now. The gold I carried will be destroyed!<br>" +
					"CANCEL - I will go get my own stuff", m_TotalCost.ToString()), true, true);
 
			AddButton(200, 227, 4005, 4007, 0, GumpButtonType.Reply, 0);
			
			AddHtmlLocalized(235, 230, 110, 35, 1011012, false, false); // CANCEL

			AddButton(65, 227, 4005, 4007, 1, GumpButtonType.Reply, 0);
			
			AddHtmlLocalized(100, 230, 110, 35, 1011011, false, false); // CONTINUE
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 0)
			{
				return;
			}

			if (info.ButtonID == 1)
			{
				if (MakePayment(m_From, m_TotalCost))
				{
					foreach (Item i in m_Items)
					{
						m_From.Backpack.AddItem(i);
					}
		
					m_From.SendLocalizedMessage(1062471); // You quickly gather all of your belongings.
				}
			}
		}

		public static bool MakePayment(Mobile buyer, int totalCost, BaseVendor from = null)
		{
			Container cont;
			
			bool bought = false;
			
			bool fromBank = false;

			cont = buyer.Backpack;
			
			bought = buyer.AccessLevel >= AccessLevel.GameMaster;
			
			if (!bought && cont != null && BaseVendor.ConsumeGold(cont, totalCost))
			{
				bought = true;
			}
			
			if (!bought)
			{
				if (totalCost <= Int32.MaxValue)
				{
					if (Banker.Withdraw(buyer, totalCost))
					{
						bought = true;
						
						fromBank = true;
					}
				}
				else if (buyer.Account != null && AccountGold.Enabled)
				{
					if (buyer.Account.WithdrawCurrency(totalCost / AccountGold.CurrencyThreshold))
					{
						bought = true;
					}
				}
			}

			if (!bought)
			{
				cont = buyer.FindBankNoCreate();

				if (cont != null && BaseVendor.ConsumeGold(cont, totalCost))
				{
					bought = true;
					
					fromBank = true;
				}
			}

			if (from != null)
			{
				if (bought)
				{
					buyer.PlaySound(0x32); //coins

					if (buyer.AccessLevel >= AccessLevel.GameMaster)
					{
						from.SayTo(buyer, true, "I would not presume to charge thee anything.  Here is the repair you requested.");
					}
					else if (fromBank)
					{
						// The total of your purchase is ~1_val~ gold, which has been drawn from your bank account.  My thanks for the patronage.
						from.SayTo (buyer, 1151638, totalCost.ToString());
					}
					else
					{
						// The total of your purchase is ~1_val~ gold.  My thanks for the patronage.
						from.SayTo (buyer, 1151639, totalCost.ToString());
					}
					
					return true;
				}

				// ? Begging thy pardon, but thy bank account lacks these funds.
				// : Begging thy pardon, but thou casnt afford that.
				buyer.SayTo(buyer, totalCost >= 2000 ? 500191 : 500192, 0x3B2);
			}
			else
			{
				if (bought)
				{
					buyer.PlaySound(0x32); //coins

					if (buyer.AccessLevel >= AccessLevel.GameMaster)
					{
						buyer.SendMessage("You have not been charged for this service.");
					}
					else if (fromBank)
					{
						buyer.SendMessage(String.Format("{0} gold has been drawn from your bank account.", totalCost));
					}
					else
					{
						buyer.SendMessage(String.Format("{0} gold has been taken for the service.", totalCost));
					}

					return true;
				}
			
				buyer.SendMessage("You lack sufficient funds for this service.");
			}
			   
			return false;
		}
	}
}
