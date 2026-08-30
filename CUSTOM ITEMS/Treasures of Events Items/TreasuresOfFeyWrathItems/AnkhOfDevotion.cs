using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using System.Collections.Generic;
using Server.Network;
using Server.ContextMenus;
using Server.Multis;
using Server.Spells;

namespace Server.Items
{
    public class AnkhOfDevotion : Item, ISecurable
    {
        public static List<TeleportEntry> Entries;

        public static void Initialize()
        {
            Entries = new List<TeleportEntry>();
            //Trammel
            Entries.Add(new TeleportEntry(new Point3D(1857, 866, 0), Map.Trammel, "Shrine of Compassion"));     // Shrine of Compassion
            Entries.Add(new TeleportEntry(new Point3D(4216, 563, 36), Map.Trammel, "Shrine of Honesty"));    // Shrine of Honesty
            Entries.Add(new TeleportEntry(new Point3D(1729, 3527, 3), Map.Trammel, "Shrine of Honor"));    // Shrine of Honor
            Entries.Add(new TeleportEntry(new Point3D(4274, 3702, 0), Map.Trammel, "Shrine of Humility"));    // Shrine of Humility
            Entries.Add(new TeleportEntry(new Point3D(1300, 640, 16), Map.Trammel, "Shrine of Justice"));    // Shrine of Justice
            Entries.Add(new TeleportEntry(new Point3D(3354, 297, 9), Map.Trammel, "Shrine of Sacrifice"));     // Shrine of Sacrifice
            Entries.Add(new TeleportEntry(new Point3D(1606, 2489, 8), Map.Trammel, "Shrine of Spirituality"));    // Shrine of Spirituality
            Entries.Add(new TeleportEntry(new Point3D(2495, 3930, 0), Map.Trammel, "Shrine of Valor"));    // Shrine of Valor
            Entries.Add(new TeleportEntry(new Point3D(1460, 843, 0), Map.Trammel, "Shrine of Chaos"));     // Shrine of Chaos
            //Felucca
            Entries.Add(new TeleportEntry(new Point3D(1857, 866, 0), Map.Felucca, "Shrine of Compassion"));   // Shrine of Compassion
            Entries.Add(new TeleportEntry(new Point3D(4216, 563, 36), Map.Felucca, "Shrine of Honesty"));  // Shrine of Honesty
            Entries.Add(new TeleportEntry(new Point3D(1729, 3527, 3), Map.Felucca, "Shrine of Honor"));  // Shrine of Honor
            Entries.Add(new TeleportEntry(new Point3D(4274, 3702, 0), Map.Felucca, "Shrine of Humility"));  // Shrine of Humility
            Entries.Add(new TeleportEntry(new Point3D(1300, 640, 16), Map.Felucca, "Shrine of Justice"));  // Shrine of Justice
            Entries.Add(new TeleportEntry(new Point3D(3354, 297, 9), Map.Felucca, "Shrine of Sacrifice"));   // Shrine of Sacrifice
            Entries.Add(new TeleportEntry(new Point3D(1606, 2489, 8), Map.Felucca, "Shrine of Spirituality"));  // Shrine of Spirituality
            Entries.Add(new TeleportEntry(new Point3D(2495, 3930, 0), Map.Felucca, "Shrine of Valor"));  // Shrine of Valor
            Entries.Add(new TeleportEntry(new Point3D(1460, 843, 0), Map.Felucca, "Shrine of Chaos"));   // Shrine of Chaos
            //Tokuno Islands
            Entries.Add(new TeleportEntry(new Point3D(284, 711, 54), Map.Tokuno, "Shrine of Homare"));   // Shrine of Homare
            Entries.Add(new TeleportEntry(new Point3D(1044, 517, 15), Map.Tokuno, "Shrine of Isamu"));  // Shrine of Isamu
            Entries.Add(new TeleportEntry(new Point3D(718, 1162, 25), Map.Tokuno, "Shrine of Makoto"));  // Shrine of Makoto
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level { get; set; }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
        }

        [Constructable]
        public AnkhOfDevotion()
            : base(0x99C7)
        {
            Name = "Ankh of Devotion";
            Weight = 10.0;
            Hue = 2752;
        }

        public override bool ForceShowProperties { get { return true; } }

        public override void OnDoubleClick(Mobile from)
        {
            if ((IsLockedDown || IsSecure) && from.InRange(GetWorldLocation(), 2))
            {
                from.SendGump(new InternalGump(from as PlayerMobile, this));
            }
            else if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500295); // You are too far away to do that.
            }
            else
            {
                from.SendLocalizedMessage(502692); // This must be in a house and be locked down to work.
            }
        }

        private class InternalGump : Gump
        {
            public Item ankhOfDevotion { get; set; }
            public PlayerMobile User { get; set; }

            public InternalGump(PlayerMobile pm, Item ankh)
                : base(500, 500)
            {
                ankhOfDevotion = ankh;
                User = pm;

                AddGumpLayout();
            }

            private string Color(string color, string str)
        {
            return String.Format("<basefont color={0}>{1}</basefont>", color, str);
        }

            public void AddGumpLayout()
            {
                int width = 420;
                int height = 370;
                AddBackground(0, 0, width, height, 1755);

                //Title
                AddHtmlLocalized(10, 10, width-20, 18, 1114513, "#1156704", 0x56BA, false, false); // Select your destination
                
                int col1x = 30;
                int col2x = 225;                

                //Add Map Labels
                AddHtml(col1x, 40, 150, 20, Color("#ACACD5","Trammel"), false, false);
                AddHtml(col2x, 40, 150, 20, Color("#ACACD5","Felucca"), false, false);
                AddHtml(col1x, 260, 150, 20, Color("#ACACD5","Tokuno Islands"), false, false);

                //Add shrine buttons/labels
                for (int i = 0; i < AnkhOfDevotion.Entries.Count; i++)
                {
                    TeleportEntry entry = AnkhOfDevotion.Entries[i];
                    int button = i+1;
                    int index = i;
                    string hue = "#FFFFFF";
                    if (entry.DestMap == Map.Trammel)
                        hue = "#AC8BEE";
                    else if (entry.DestMap == Map.Felucca)
                        hue = "#00B494";
                    else if (entry.DestMap == Map.Tokuno)
                        hue = "#296229";

                    
                    int x = 0;
                    //Set the correct X
                    if (0 <= index && index <= 8)
                        x = col1x;
                    else if (9 <= index && index <= 17)
                        x = col2x;
                    else if (18 <= index && index <= 20)
                        x = col1x;

                    int y = 65; 
                    int row2offset = 220;            
                    //Check if resetting index for second column Y
                    if (9 <= index && index <= 17)
                        index -= 9; //subtract 9 to get the right y-offsets for the 2nd column
                    if (index >= 18)
                    {
                        y += row2offset;
                        index -= 18;
                    }
                    
                    //Add the button and label to the gump
                    AddButton(x, y + (index * 20) + 1, 1209, 1210, button, GumpButtonType.Reply, 0);
                    AddHtml(x + 40, y + (index * 20), 150, 20, Color(hue,entry.Description), false, false);
                }
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                if (info.ButtonID > 0)
                {
                    int id = info.ButtonID;

                    if (id-1 < AnkhOfDevotion.Entries.Count)
                    {
                        Point3D p = AnkhOfDevotion.Entries[id-1].Location;
                        Map map = AnkhOfDevotion.Entries[id-1].DestMap;
                        
                        if (CheckTravel(p))
                        {
                            BaseCreature.TeleportPets(User, p, map);
                            User.Combatant = null;
                            User.Warmode = false;
                            User.Hidden = true;

                            User.MoveToWorld(p, map);

                            Effects.PlaySound(p, map, 0x1FE);
                        }
                    }
                }
            }

            private bool CheckTravel(Point3D p)
            {
                if (!User.InRange(ankhOfDevotion.GetWorldLocation(), 2) || User.Map != ankhOfDevotion.Map)
                {
                    User.SendLocalizedMessage(500295); // You are too far away to do that.
                }
                else if (SpellHelper.RestrictRedTravel && User.Murderer)
                {
                    User.SendLocalizedMessage(1019004); // You are not allowed to travel there.
                }
                else if (Factions.Sigil.ExistsOn(User))
                {
                    User.SendLocalizedMessage(1019004); // You are not allowed to travel there.
                }
                else if (User.Criminal)
                {
                    User.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
                }
                else if (SpellHelper.CheckCombat(User))
                {
                    User.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                }
                else if (User.Spell != null)
                {
                    User.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
                }
                else if (User.Map == Map.Ilshenar && User.InRange(p, 1))
                {
                    User.SendLocalizedMessage(1019003); // You are already there.
                }
                else
                    return true;

                return false;
            }
        }

        public AnkhOfDevotion(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write((int)Level);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Level = (SecureLevel)reader.ReadInt();
        }
    }

    public class TeleportEntry
    {
        public Point3D Location;
        public Map DestMap;
        public String Description;

        public TeleportEntry(Point3D loc, Map dest, string desc)
        {
            Location = loc;
            DestMap = dest;
            Description = desc;
        }
    }
}
