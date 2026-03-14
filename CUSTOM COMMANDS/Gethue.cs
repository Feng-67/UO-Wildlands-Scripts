/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Milva)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System; 
using Server.Network; 
using Server.Mobiles; 
using Server.Targeting;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items; 
using System.Collections; 

namespace Server.Scripts.Commands { 
	public class GetHueCommand : BaseCommand { 

		public static void Initialize() { 
			TargetCommands.Register( new GetHueCommand() ); 
		} 

		public GetHueCommand() {
			AccessLevel = AccessLevel.Player;
			Supports = CommandSupport.Single;
			Commands = new string[]{ "GetHue" };
			ObjectTypes = ObjectTypes.All;
			Usage = "GetHue";
			Description = "Gets the hue of an object.";
		}

		public override void Execute( CommandEventArgs e, object obj ) {
			if (obj is Item)
			{
				Item tar = (Item)obj;
				e.Mobile.SendMessage( "Hue: " + tar.Hue ); 
			}
			
			else if (obj is Mobile)
			{
				Mobile targ = (Mobile)obj;
				e.Mobile.SendMessage( "Hue: " + targ.Hue ); 
			}

			else if (obj is PlayerMobile)
			{
				PlayerMobile targ = (PlayerMobile)obj;
				e.Mobile.SendMessage( "Hue: " + targ.Hue ); 
			}
		}
	} 
} 
