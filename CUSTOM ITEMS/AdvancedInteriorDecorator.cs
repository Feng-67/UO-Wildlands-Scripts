/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Linq;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public enum DecorateCommand
    {
        None,
        Turn,
        Up,
        Down,
        North,
	East,
	South,
	West,
        GetHue,
	LockDown,
        Secure,
        Release
    }

    public class AdvancedInteriorDecorator : Item
    {
        public override int LabelNumber { get { return 1041280; } } // an advanced interior decorator

        [Constructable]
        public AdvancedInteriorDecorator()
            : base(0xFC1)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public AdvancedInteriorDecorator(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DecorateCommand Command { get; set; }

        public static bool InHouse(Mobile from)
        {
            BaseHouse house = BaseHouse.FindHouseAt(from);

            return (house != null && house.IsCoOwner(from));
        }

        public static bool CheckUse(AdvancedInteriorDecorator tool, Mobile from)
        {
            if (!InHouse(from))
                from.SendLocalizedMessage(502092); // You must be in your house to do this.
            else
                return true;

            return false;
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!InHouse(from))
                Command = DecorateCommand.GetHue;

            if (from.FindGump(typeof(InternalGump)) == null)
                from.SendGump(new InternalGump(from, this));

            if (Command != DecorateCommand.None)
                from.Target = new InternalTarget(this);
        }

        private class InternalGump : Gump
        {
            private readonly AdvancedInteriorDecorator m_Decorator;

            public InternalGump(Mobile from, AdvancedInteriorDecorator decorator)
                : base(150, 50)
            {
                m_Decorator = decorator;

                AddBackground(0, 0, 170, 510, 2600);

                AddPage(0);

                if (!InHouse(from))
                {
                    AddButton(40, 36, (decorator.Command == DecorateCommand.GetHue ? 2154 : 2152), 2154, 8, GumpButtonType.Reply, 0);
                    AddHtmlLocalized(80, 330, 100, 20, 1158863, false, false); // Get Hue   
                }
                else
                {
                AddButton( 40, 45, 2152, 2154, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 50, 70, 40, 1018323, false, false );
				
				AddButton( 40, 85, 2152, 2154, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 90, 70, 40, 1018324, false, false );
				
				AddButton( 40, 125, 2152, 2154, 3, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 130, 70, 40, 1018325, false, false );
				
				AddButton( 40, 165, 2152, 2154, 4, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 170, 70, 40, 1075389, false, false );
				
				AddButton( 40, 205, 2152, 2154, 5, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 210, 70, 40, 1075387, false, false );
				
				AddButton( 40, 245, 2152, 2154, 6, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 250, 70, 40, 1075386, false, false );
				
				AddButton( 40, 285, 2152, 2154, 7, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 290, 70, 40, 1075390, false, false );

                AddButton(40, 325, (decorator.Command == DecorateCommand.GetHue ? 2154 : 2152), 2154, 8, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 330, 100, 20, 1158863, false, false); // Get Hue   

                    //AddButton(40, 36, (decorator.Command == DecorateCommand.Turn ? 2154 : 2152), 2154, 1, GumpButtonType.Reply, 0);
                    //AddHtmlLocalized(80, 41, 100, 20, 1018323, false, false); // Turn

                   //AddButton(40, 86, (decorator.Command == DecorateCommand.Up ? 2154 : 2152), 2154, 2, GumpButtonType.Reply, 0);
                    //AddHtmlLocalized(80, 91, 100, 20, 1018324, false, false); // Up

                    //AddButton(40, 136, (decorator.Command == DecorateCommand.Down ? 2154 : 2152), 2154, 3, GumpButtonType.Reply, 0);
                    //AddHtmlLocalized(80, 141, 100, 20, 1018325, false, false); // Down

                    //AddButton(40, 186, (decorator.Command == DecorateCommand.GetHue ? 2154 : 2152), 2154, 4, GumpButtonType.Reply, 0);
                    //AddHtmlLocalized(80, 330, 100, 20, 1158863, false, false); // Get Hue                    

AddButton(40, 365, 2152, 2154, 9, GumpButtonType.Reply, 0);
AddHtml(80, 370, 100, 20, "Lock Down", false, false);

AddButton(40, 405, 2152, 2154, 10, GumpButtonType.Reply, 0);
AddHtml(80, 410, 100, 20, "Secure", false, false);

AddButton(40, 445, 2152, 2154, 11, GumpButtonType.Reply, 0);
AddHtml(80, 450, 100, 20, "Release", false, false);

                }

                AddHtmlLocalized(0, 0, 0, 0, 4, false, false);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                DecorateCommand command = DecorateCommand.None;
                Mobile m = sender.Mobile;

                int cliloc = 0;

                switch (info.ButtonID)
                {
                    
                    case 1: 
                    cliloc = 1073404; // Select an object to turn.
                    command = DecorateCommand.Turn; 
							break;
					case 2: 
                    cliloc = 1073405; // Select an object to increase its height.
                    command = DecorateCommand.Up; 
							break;
					case 3: 
                    cliloc = 1073406; // Select an object to lower its height.
                    command = DecorateCommand.Down; 
                             break;
                    case 4: 
                    m.SendMessage("Select an object to move North.");
                    command = DecorateCommand.North; 
							break;
					case 5: 
                    m.SendMessage("Select an object to move East.");
                    command = DecorateCommand.East; 
							break;
					case 6: 
                    m.SendMessage("Select an object to move South.");
                    command = DecorateCommand.South; 
							break;
					case 7: 
                    m.SendMessage("Select an object to move West.");
                    command = DecorateCommand.West; 
							break;
                   case 8: 
                   cliloc = 1158864; // Select an object to get the hue.
                   command = DecorateCommand.GetHue;
                           break;

 case 9:
    m.SendMessage("Select an object to lock down.");
    command = DecorateCommand.LockDown;
    break;
case 10:
    m.SendMessage("Select an object to secure.");
    command = DecorateCommand.Secure;
    break;
case 11:
    m.SendMessage("Select an object to release.");
    command = DecorateCommand.Release;
    break;

                   
                   // case 1:
                     //   cliloc = 1073404; // Select an object to turn.
                      //  command = DecorateCommand.Turn;
                       // break; //
                    //case 2:
                    //    cliloc = 1073405; // Select an object to increase its height.
                    //    command = DecorateCommand.Up;
                    //    break;
                    //case 3:
                    //    cliloc = 1073406; // Select an object to lower its height.
                    //    command = DecorateCommand.Down;
                    //    break;
                    //case 4:
                    //    cliloc = 1158864; // Select an object to get the hue.
                    //    command = DecorateCommand.GetHue;
                    //    break;
                }

                if (command != DecorateCommand.None)
                {
                    m_Decorator.Command = command;
                    m.SendGump(new InternalGump(m, m_Decorator));

                    if (cliloc != 0)
                        m.SendLocalizedMessage(cliloc);

                    m.Target = new InternalTarget(m_Decorator);
                }
                else
                {
                    Target.Cancel(m);
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly AdvancedInteriorDecorator m_Decorator;

            public InternalTarget(AdvancedInteriorDecorator decorator)
                : base(-1, false, TargetFlags.None)
            {
                CheckLOS = false;

                m_Decorator = decorator;
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                OnTarget(from, targeted);
            }

            private static Type[] m_KingsCollectionTypes = new Type[]
            {
                typeof(BirdLamp),    typeof(DragonLantern),
                typeof(KoiLamp),   typeof(TallLamp)
            };

            private static bool IsKingsCollection(Item item)
            {
                return m_KingsCollectionTypes.Any(t => t == item.GetType());
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Decorator.Command == DecorateCommand.GetHue)
                {
                    int hue = 0;

                    if (targeted is Item)
                        hue = ((Item)targeted).Hue;
                    else if (targeted is Mobile)
                        hue = ((Mobile)targeted).Hue;
                    else
                    {
                        from.Target = new InternalTarget(m_Decorator);
                        return;
                    }

                    from.SendLocalizedMessage(1158862, String.Format("{0}", hue)); // That object is hue ~1_HUE~
                }
                else if (targeted is Item && CheckUse(m_Decorator, from))
                {
                    BaseHouse house = BaseHouse.FindHouseAt(from);
                    Item item = (Item)targeted;

                    bool isDecorableComponent = false;

                    if (m_Decorator.Command == DecorateCommand.Turn && IsKingsCollection(item))
                    {
                        isDecorableComponent = true;
                    }
                    else if (item is AddonComponent || item is AddonContainerComponent || item is BaseAddonContainer)
                    {
                        object addon = null;
                        int count = 0;

                        if (item is AddonComponent)
                        {
                            AddonComponent component = (AddonComponent)item;
                            count = component.Addon.Components.Count;
                            addon = component.Addon;
                        }
                        else if (item is AddonContainerComponent)
                        {
                            AddonContainerComponent component = (AddonContainerComponent)item;
                            count = component.Addon.Components.Count;
                            addon = component.Addon;
                        }
                        else if (item is BaseAddonContainer)
                        {
                            BaseAddonContainer container = (BaseAddonContainer)item;
                            count = container.Components.Count;
                            addon = container;
                        }

                        if (count == 1 && Core.SE)
                            isDecorableComponent = true;

                        if (item is EnormousVenusFlytrapAddon)
                            isDecorableComponent = true;

                        if (m_Decorator.Command == DecorateCommand.Turn)
                        {
                            FlipableAddonAttribute[] attributes = (FlipableAddonAttribute[])addon.GetType().GetCustomAttributes(typeof(FlipableAddonAttribute), false);

                            if (attributes.Length > 0)
                                isDecorableComponent = true;
                        }
                    }
                    else if (item is Banner && m_Decorator.Command != DecorateCommand.Turn)
                    {
                        isDecorableComponent = true;
                    }

                    if (house == null || !house.IsCoOwner(from))
                    {
                        from.SendLocalizedMessage(502092); // You must be in your house to do 
                    }
                    else if (item.Parent != null || !house.IsInside(item))
                    {
                        from.SendLocalizedMessage(1042270); // That is not in your house.
                    }
                  else if (!house.IsLockedDown(item) && !house.IsSecure(item) && !isDecorableComponent && 
                             m_Decorator.Command != DecorateCommand.LockDown && 
                             m_Decorator.Command != DecorateCommand.Secure)
                    {
    // ... logic for error messages follows ...
                        if (item is AddonComponent && m_Decorator.Command == DecorateCommand.Turn)
                            from.SendLocalizedMessage(1042273); // You cannot turn that.
                        else if (item is AddonComponent && m_Decorator.Command == DecorateCommand.Up)
                            from.SendLocalizedMessage(1042274); // You cannot raise it up any higher.
                        else if (item is AddonComponent && m_Decorator.Command == DecorateCommand.Down)
                            from.SendLocalizedMessage(1042275); // You cannot lower it down any further.
                        else
                            from.SendLocalizedMessage(1042271); // That is not locked down.
                    }
                    else if (item is VendorRentalContract)
                    {
                        from.SendLocalizedMessage(1062491); // You cannot use the house decorator on that object.
                    }
                    /*else if (item.TotalWeight + item.PileWeight > 100)
                    {
                        from.SendLocalizedMessage(1042272); // That is too heavy.
                    }*/
                    else
                    {
                        switch (m_Decorator.Command)
                        {
                            case DecorateCommand.Up:
                                Up(item, from);
                                break;
                            case DecorateCommand.Down:
                                Down(item, from);
                                break;
                            case DecorateCommand.Turn:
                                Turn(item, from);
                                break;
                            case DecorateCommand.North:	
								North( item, from );	
								break;
							case DecorateCommand.East:	
								East( item, from );	
								break;
							case DecorateCommand.South:	
								South( item, from );
								break;
							case DecorateCommand.West:	
								West( item, from );	
								break;

case DecorateCommand.LockDown:
    house.LockDown(from, item);
    break;

case DecorateCommand.Secure:
    house.AddSecure(from, item);
    break;

case DecorateCommand.Release:
    house.Release(from, item);
    break;                       
                        }
                    }
                }

                from.Target = new InternalTarget(m_Decorator);
            }
        
            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Canceled)
                    from.CloseGump(typeof(AdvancedInteriorDecorator.InternalGump));
            }

            private static void Turn(Item item, Mobile from)
            {
                if (item is IFlipable)
                {
                    ((IFlipable)item).OnFlip(from);
                    return;
                }

                if (item is AddonComponent || item is AddonContainerComponent || item is BaseAddonContainer)
                {
                    object addon = null;

                    if (item is AddonComponent)
                        addon = ((AddonComponent)item).Addon;
                    else if (item is AddonContainerComponent)
                        addon = ((AddonContainerComponent)item).Addon;
                    else if (item is BaseAddonContainer)
                        addon = (BaseAddonContainer)item;

                    FlipableAddonAttribute[] aAttributes = (FlipableAddonAttribute[])addon.GetType().GetCustomAttributes(typeof(FlipableAddonAttribute), false);

                    if (aAttributes.Length > 0)
                    {
                        aAttributes[0].Flip(from, (Item)addon);
                        return;
                    }
                }

                FlipableAttribute[] attributes = (FlipableAttribute[])item.GetType().GetCustomAttributes(typeof(FlipableAttribute), false);

                if (attributes.Length > 0)
                    attributes[0].Flip(item);
                else
                    from.SendLocalizedMessage(1042273); // You cannot turn that.
            }

            private static void Up(Item item, Mobile from)
            {
                int floorZ = GetFloorZ(item);

                if (floorZ > int.MinValue && item.Z < (floorZ + 40)) // Confirmed : no height checks here
                    item.Location = new Point3D(item.Location, item.Z + 1);
                else
                    from.SendLocalizedMessage(1042274); // You cannot raise it up any higher.
            }

            private static void Down(Item item, Mobile from)
            {
                int floorZ = GetFloorZ(item);

                if (floorZ > int.MinValue && item.Z > GetFloorZ(item))
                    item.Location = new Point3D(item.Location, item.Z - 1);
                else
                    from.SendLocalizedMessage(1042275); // You cannot lower it down any further.
            }
            private static void North( Item item, Mobile from )
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);
				
				BaseMulti multi = (BaseMulti)house;
				
				MultiComponentList mcl = multi.Components;
				
				Point3D p = new Point3D(item.X, item.Y-1, item.Z);
				
				if( !house.IsInside(p, item.Z) )
					from.SendMessage("You cannot move it any further North.");
				else
					item.Y = ( item.Y - 1 );
			}

			private static void East( Item item, Mobile from )
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);
				
				BaseMulti multi = (BaseMulti)house;
				
				MultiComponentList mcl = multi.Components;
				
				int max = (mcl.Width / 2);
				
				Point3D p = new Point3D(item.X+1, item.Y, item.Z);
				
				if( !house.IsInside(p, item.Z) )
					from.SendMessage("You cannot move it any further East.");
				else
				item.X = ( item.X + 1 );
			}

			private static void South( Item item, Mobile from )
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);
				
				BaseMulti multi = (BaseMulti)house;
				
				MultiComponentList mcl = multi.Components;
				
				int max = (mcl.Height / 2);
				
				Point3D p = new Point3D(item.X, item.Y+1, item.Z);
				
				if( !house.IsInside(p, item.Z) )
					from.SendMessage("You cannot move it any further South.");
				else
				item.Y = ( item.Y + 1 );
			}

			private static void West( Item item, Mobile from )
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);
				
				BaseMulti multi = (BaseMulti)house;
				
				MultiComponentList mcl = multi.Components;
				
				int max = (mcl.Width / 2);
				
				Point3D p = new Point3D(item.X-1, item.Y, item.Z);
				
				if( !house.IsInside(p, item.Z) )
					from.SendMessage("You cannot move it any further West.");
				else
				item.X = ( item.X - 1 );
			}

            private static int GetFloorZ(Item item)
            {
                Map map = item.Map;

                if (map == null)
                    return int.MinValue;

                StaticTile[] tiles = map.Tiles.GetStaticTiles(item.X, item.Y, true);

                int z = int.MinValue;

                for (int i = 0; i < tiles.Length; ++i)
                {
                    StaticTile tile = tiles[i];
                    ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

                    int top = tile.Z; // Confirmed : no height checks here

                    if (id.Surface && !id.Impassable && top > z && top <= item.Z)
                        z = top;
                }
				
				if ( z == int.MinValue )
					z = map.Tiles.GetLandTile( item.X, item.Y ).Z;

return z;
            }

        }
    }
}
