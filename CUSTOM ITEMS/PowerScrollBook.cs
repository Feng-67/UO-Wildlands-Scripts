/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    [Flipable(0x9A95, 0x9AA7)]
    public class PowerScrollBook : BaseSpecialScrollBook
    {
        // Added the list the Gump needs to function
        public List<Item> Entries { get; private set; }

        public override Type ScrollType { get { return typeof(PowerScroll); } }
        public override int LabelNumber { get { return 1155684; } }
        public override int BadDropMessage { get { return 1155691; } }
        public override int DropMessage { get { return 1155692; } }
        public override int RemoveMessage { get { return 1155690; } }
        public override int GumpTitle { get { return 1155689; } }

        [Constructable]
        public PowerScrollBook() : base(0x9A95)
        {
            Hue = 1153;
            Entries = new List<Item>();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, 1019045);
                return;
            }

            from.CloseGump(typeof(PowerScrollBookGump));
            from.SendGump(new PowerScrollBookGump(from, this));
        }

        // Logic to accept Power Scrolls and refresh the gump
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is PowerScroll)
            {
                Entries.Add(dropped);
                dropped.Internalize();
                from.SendLocalizedMessage(DropMessage);

                // Refresh the gump if it's open
                from.CloseGump(typeof(PowerScrollBookGump));
                from.SendGump(new PowerScrollBookGump(from, this));

                InvalidateProperties();
                return true;
            }

            from.SendLocalizedMessage(BadDropMessage);
            return false;
        }

        public PowerScrollBook(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(Entries, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Entries = (version >= 1) ? reader.ReadStrongItemList() : new List<Item>();
        }

        public override Dictionary<SkillCat, List<SkillName>> SkillInfo { get { return _SkillInfo; } }
        public override Dictionary<int, double> ValueInfo { get { return _ValueInfo; } }
        public static Dictionary<SkillCat, List<SkillName>> _SkillInfo;
        public static Dictionary<int, double> _ValueInfo;

        public static void Initialize()
        {
            _SkillInfo = new Dictionary<SkillCat, List<SkillName>>();

            _SkillInfo[SkillCat.Combat] = new List<SkillName>()
    {
        SkillName.Anatomy, SkillName.Archery, SkillName.Fencing, SkillName.Focus,
        SkillName.Healing, SkillName.Macing, SkillName.Parry, SkillName.Swords,
        SkillName.Tactics, SkillName.Throwing, SkillName.Wrestling, SkillName.Lumberjacking
    };

            _SkillInfo[SkillCat.TradeSkills] = new List<SkillName>()
    {
        SkillName.Blacksmith, SkillName.Tailoring, SkillName.Alchemy, SkillName.Carpentry,
        SkillName.Cooking, SkillName.Inscribe, SkillName.Tinkering, SkillName.Fletching
    };

            _SkillInfo[SkillCat.Magic] = new List<SkillName>()
    {
        SkillName.Bushido, SkillName.Chivalry, SkillName.EvalInt, SkillName.Imbuing,
        SkillName.Magery, SkillName.Meditation, SkillName.Mysticism, SkillName.Necromancy,
        SkillName.Ninjitsu, SkillName.MagicResist, SkillName.Spellweaving, SkillName.SpiritSpeak
    };

            _SkillInfo[SkillCat.Wilderness] = new List<SkillName>()
    {
        SkillName.AnimalLore, SkillName.AnimalTaming, SkillName.Fishing, SkillName.Veterinary,
        SkillName.Camping, SkillName.Forensics, SkillName.Herding, SkillName.Tracking
    };

            _SkillInfo[SkillCat.Thievery] = new List<SkillName>()
    {
        SkillName.Stealing, SkillName.Stealth, SkillName.DetectHidden, SkillName.Hiding,
        SkillName.Lockpicking, SkillName.Poisoning, SkillName.RemoveTrap, SkillName.Snooping
    };

            _SkillInfo[SkillCat.Bard] = new List<SkillName>()
    {
        SkillName.Discordance, SkillName.Musicianship, SkillName.Peacemaking, SkillName.Provocation
    };

            _SkillInfo[SkillCat.Miscellaneous] = new List<SkillName>()
    {
        SkillName.ArmsLore, SkillName.Begging, SkillName.Cartography, SkillName.ItemID, SkillName.TasteID
    };

            _ValueInfo = new Dictionary<int, double>();
            _ValueInfo[1155685] = 105;
            _ValueInfo[1155686] = 110;
            _ValueInfo[1155687] = 115;
            _ValueInfo[1155688] = 120;
        }
    }
}
