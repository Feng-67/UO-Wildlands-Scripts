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
using Server.Mobiles;

namespace Server.Items
{
    #region Asian Carpet
    public class AsianCarpetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new AsianCarpetDeed(); } }
        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public AsianCarpetAddon(int hue)
        {
            AddComponent(new AddonComponent(2769), 0, 0, 0);
            AddComponent(new AddonComponent(2769), 0, 1, 0);
            AddComponent(new AddonComponent(2769), 0, -1, 0);
            AddComponent(new AddonComponent(2769), 1, 0, 0);
            AddComponent(new AddonComponent(2769), -1, 0, 0);
            AddComponent(new AddonComponent(2769), 1, 1, 0);
            AddComponent(new AddonComponent(2769), -1, -1, 0);
            AddComponent(new AddonComponent(2769), 1, -1, 0);
            AddComponent(new AddonComponent(2769), -1, 1, 0);
            AddComponent(new AddonComponent(2775), 0, -2, 0);
            AddComponent(new AddonComponent(2775), -1, -2, 0);
            AddComponent(new AddonComponent(2775), 1, -2, 0);
            AddComponent(new AddonComponent(2777), 0, 2, 0);
            AddComponent(new AddonComponent(2777), -1, 2, 0);
            AddComponent(new AddonComponent(2777), 1, 2, 0);
            AddComponent(new AddonComponent(2774), -2, 0, 0);
            AddComponent(new AddonComponent(2774), -2, 1, 0);
            AddComponent(new AddonComponent(2774), -2, -1, 0);
            AddComponent(new AddonComponent(2776), 2, 0, 0);
            AddComponent(new AddonComponent(2776), 2, 1, 0);
            AddComponent(new AddonComponent(2776), 2, -1, 0);
            AddComponent(new AddonComponent(2771), -2, -2, 0);
            AddComponent(new AddonComponent(2772), -2, 2, 0);
            AddComponent(new AddonComponent(2773), 2, -2, 0);
            AddComponent(new AddonComponent(2770), 2, 2, 0);
            Hue = hue;
        }

        public AsianCarpetAddon(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class AsianCarpetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new AsianCarpetAddon(this.Hue); } }
        [Constructable]
        public AsianCarpetDeed() { Name = "an asian rug deed"; }
        public AsianCarpetDeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
    #endregion

    #region Blue Carpet
    public class BlueCarpetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new BlueCarpetDeed(); } }
        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public BlueCarpetAddon(int hue)
        {
            AddComponent(new AddonComponent(2749), 0, 0, 0);
            AddComponent(new AddonComponent(2749), 0, 1, 0);
            AddComponent(new AddonComponent(2749), 0, -1, 0);
            AddComponent(new AddonComponent(2749), 1, 0, 0);
            AddComponent(new AddonComponent(2749), -1, 0, 0);
            AddComponent(new AddonComponent(2749), 1, 1, 0);
            AddComponent(new AddonComponent(2749), -1, -1, 0);
            AddComponent(new AddonComponent(2749), 1, -1, 0);
            AddComponent(new AddonComponent(2749), -1, 1, 0);
            AddComponent(new AddonComponent(2807), 0, -2, 0);
            AddComponent(new AddonComponent(2807), -1, -2, 0);
            AddComponent(new AddonComponent(2807), 1, -2, 0);
            AddComponent(new AddonComponent(2805), 0, 2, 0);
            AddComponent(new AddonComponent(2805), -1, 2, 0);
            AddComponent(new AddonComponent(2805), 1, 2, 0);
            AddComponent(new AddonComponent(2806), -2, 0, 0);
            AddComponent(new AddonComponent(2806), -2, 1, 0);
            AddComponent(new AddonComponent(2806), -2, -1, 0);
            AddComponent(new AddonComponent(2808), 2, 0, 0);
            AddComponent(new AddonComponent(2808), 2, 1, 0);
            AddComponent(new AddonComponent(2808), 2, -1, 0);
            AddComponent(new AddonComponent(2755), -2, -2, 0);
            AddComponent(new AddonComponent(2756), -2, 2, 0);
            AddComponent(new AddonComponent(2757), 2, -2, 0);
            AddComponent(new AddonComponent(2754), 2, 2, 0);
            Hue = hue;
        }

        public BlueCarpetAddon(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class BlueCarpetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new BlueCarpetAddon(this.Hue); } }
        [Constructable]
        public BlueCarpetDeed() { Name = "a blue rug deed"; }
        public BlueCarpetDeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
    #endregion

    #region Fancy Blue Carpet
    public class FancyBlueCarpetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new FancyBlueCarpetDeed(); } }
        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public FancyBlueCarpetAddon(int hue)
        {
            AddComponent(new AddonComponent(2810), 0, 0, 0);
            AddComponent(new AddonComponent(2810), 0, 1, 0);
            AddComponent(new AddonComponent(2810), 0, -1, 0);
            AddComponent(new AddonComponent(2810), 1, 0, 0);
            AddComponent(new AddonComponent(2810), -1, 0, 0);
            AddComponent(new AddonComponent(2810), 1, 1, 0);
            AddComponent(new AddonComponent(2810), -1, -1, 0);
            AddComponent(new AddonComponent(2810), 1, -1, 0);
            AddComponent(new AddonComponent(2810), -1, 1, 0);
            AddComponent(new AddonComponent(2807), 0, -2, 0);
            AddComponent(new AddonComponent(2807), -1, -2, 0);
            AddComponent(new AddonComponent(2807), 1, -2, 0);
            AddComponent(new AddonComponent(2805), 0, 2, 0);
            AddComponent(new AddonComponent(2805), -1, 2, 0);
            AddComponent(new AddonComponent(2805), 1, 2, 0);
            AddComponent(new AddonComponent(2806), -2, 0, 0);
            AddComponent(new AddonComponent(2806), -2, 1, 0);
            AddComponent(new AddonComponent(2806), -2, -1, 0);
            AddComponent(new AddonComponent(2808), 2, 0, 0);
            AddComponent(new AddonComponent(2808), 2, 1, 0);
            AddComponent(new AddonComponent(2808), 2, -1, 0);
            AddComponent(new AddonComponent(2755), -2, -2, 0);
            AddComponent(new AddonComponent(2756), -2, 2, 0);
            AddComponent(new AddonComponent(2757), 2, -2, 0);
            AddComponent(new AddonComponent(2754), 2, 2, 0);
            Hue = hue;
        }

        public FancyBlueCarpetAddon(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class FancyBlueCarpetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new FancyBlueCarpetAddon(this.Hue); } }
        [Constructable]
        public FancyBlueCarpetDeed() { Name = "a fancy blue rug deed"; }
        public FancyBlueCarpetDeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
    #endregion

    #region Fancy Carpet
    public class FancyCarpetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new FancyCarpetDeed(); } }
        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public FancyCarpetAddon(int hue)
        {
            AddComponent(new AddonComponent(2796), 0, 0, 0);
            AddComponent(new AddonComponent(2796), 0, 1, 0);
            AddComponent(new AddonComponent(2796), 0, -1, 0);
            AddComponent(new AddonComponent(2796), 1, 0, 0);
            AddComponent(new AddonComponent(2796), -1, 0, 0);
            AddComponent(new AddonComponent(2797), 1, 1, 0);
            AddComponent(new AddonComponent(2797), -1, -1, 0);
            AddComponent(new AddonComponent(2797), 1, -1, 0);
            AddComponent(new AddonComponent(2797), -1, 1, 0);
            AddComponent(new AddonComponent(2803), 0, -2, 0);
            AddComponent(new AddonComponent(2803), -1, -2, 0);
            AddComponent(new AddonComponent(2803), 1, -2, 0);
            AddComponent(new AddonComponent(2801), 0, 2, 0);
            AddComponent(new AddonComponent(2801), -1, 2, 0);
            AddComponent(new AddonComponent(2801), 1, 2, 0);
            AddComponent(new AddonComponent(2802), -2, 0, 0);
            AddComponent(new AddonComponent(2802), -2, 1, 0);
            AddComponent(new AddonComponent(2802), -2, -1, 0);
            AddComponent(new AddonComponent(2804), 2, 0, 0);
            AddComponent(new AddonComponent(2804), 2, 1, 0);
            AddComponent(new AddonComponent(2804), 2, -1, 0);
            AddComponent(new AddonComponent(2799), -2, -2, 0);
            AddComponent(new AddonComponent(2800), -2, 2, 0);
            AddComponent(new AddonComponent(2801), 2, -2, 0);
            AddComponent(new AddonComponent(2798), 2, 2, 0);
            Hue = hue;
        }

        public FancyCarpetAddon(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class FancyCarpetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new FancyCarpetAddon(this.Hue); } }
        [Constructable]
        public FancyCarpetDeed() { Name = "a fancy rug deed"; }
        public FancyCarpetDeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
    #endregion

    #region Fancy Red Carpet
    public class FancyRedCarpetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new FancyRedCarpetDeed(); } }
        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public FancyRedCarpetAddon(int hue)
        {
            AddComponent(new AddonComponent(2758), 0, 0, 0);
            AddComponent(new AddonComponent(2758), 0, 1, 0);
            AddComponent(new AddonComponent(2758), 0, -1, 0);
            AddComponent(new AddonComponent(2758), 1, 0, 0);
            AddComponent(new AddonComponent(2758), -1, 0, 0);
            AddComponent(new AddonComponent(2758), 1, 1, 0);
            AddComponent(new AddonComponent(2758), -1, -1, 0);
            AddComponent(new AddonComponent(2758), 1, -1, 0);
            AddComponent(new AddonComponent(2758), -1, 1, 0);
            AddComponent(new AddonComponent(2766), 0, -2, 0);
            AddComponent(new AddonComponent(2766), -1, -2, 0);
            AddComponent(new AddonComponent(2766), 1, -2, 0);
            AddComponent(new AddonComponent(2764), 0, 2, 0);
            AddComponent(new AddonComponent(2764), -1, 2, 0);
            AddComponent(new AddonComponent(2764), 1, 2, 0);
            AddComponent(new AddonComponent(2765), -2, 0, 0);
            AddComponent(new AddonComponent(2765), -2, 1, 0);
            AddComponent(new AddonComponent(2765), -2, -1, 0);
            AddComponent(new AddonComponent(2767), 2, 0, 0);
            AddComponent(new AddonComponent(2767), 2, 1, 0);
            AddComponent(new AddonComponent(2767), 2, -1, 0);
            AddComponent(new AddonComponent(2762), -2, -2, 0);
            AddComponent(new AddonComponent(2763), -2, 2, 0);
            AddComponent(new AddonComponent(2764), 2, -2, 0);
            AddComponent(new AddonComponent(2761), 2, 2, 0);
            Hue = hue;
        }

        public FancyRedCarpetAddon(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class FancyRedCarpetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new FancyRedCarpetAddon(this.Hue); } }
        [Constructable]
        public FancyRedCarpetDeed() { Name = "a fancy red rug deed"; }
        public FancyRedCarpetDeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
    #endregion

    #region Plain Blue Carpet
    public class PlainBlueCarpetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new PlainBlueCarpetDeed(); } }
        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public PlainBlueCarpetAddon(int hue)
        {
            AddComponent(new AddonComponent(2750), 0, 0, 0);
            AddComponent(new AddonComponent(2750), 0, 1, 0);
            AddComponent(new AddonComponent(2750), 0, -1, 0);
            AddComponent(new AddonComponent(2750), 1, 0, 0);
            AddComponent(new AddonComponent(2750), -1, 0, 0);
            AddComponent(new AddonComponent(2750), 1, 1, 0);
            AddComponent(new AddonComponent(2750), -1, -1, 0);
            AddComponent(new AddonComponent(2750), 1, -1, 0);
            AddComponent(new AddonComponent(2750), -1, 1, 0);
            AddComponent(new AddonComponent(2807), 0, -2, 0);
            AddComponent(new AddonComponent(2807), -1, -2, 0);
            AddComponent(new AddonComponent(2807), 1, -2, 0);
            AddComponent(new AddonComponent(2805), 0, 2, 0);
            AddComponent(new AddonComponent(2805), -1, 2, 0);
            AddComponent(new AddonComponent(2805), 1, 2, 0);
            AddComponent(new AddonComponent(2806), -2, 0, 0);
            AddComponent(new AddonComponent(2806), -2, 1, 0);
            AddComponent(new AddonComponent(2806), -2, -1, 0);
            AddComponent(new AddonComponent(2808), 2, 0, 0);
            AddComponent(new AddonComponent(2808), 2, 1, 0);
            AddComponent(new AddonComponent(2808), 2, -1, 0);
            AddComponent(new AddonComponent(2755), -2, -2, 0);
            AddComponent(new AddonComponent(2756), -2, 2, 0);
            AddComponent(new AddonComponent(2757), 2, -2, 0);
            AddComponent(new AddonComponent(2754), 2, 2, 0);
            Hue = hue;
        }

        public PlainBlueCarpetAddon(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class PlainBlueCarpetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new PlainBlueCarpetAddon(this.Hue); } }
        [Constructable]
        public PlainBlueCarpetDeed() { Name = "a plain blue rug deed"; }
        public PlainBlueCarpetDeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
    #endregion

    #region Red Carpet
    public class RedCarpetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new RedCarpetDeed(); } }
        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public RedCarpetAddon(int hue)
        {
            AddComponent(new AddonComponent(2760), 0, 0, 0);
            AddComponent(new AddonComponent(2760), 0, 1, 0);
            AddComponent(new AddonComponent(2760), 0, -1, 0);
            AddComponent(new AddonComponent(2760), 1, 0, 0);
            AddComponent(new AddonComponent(2760), -1, 0, 0);
            AddComponent(new AddonComponent(2760), 1, 1, 0);
            AddComponent(new AddonComponent(2760), -1, -1, 0);
            AddComponent(new AddonComponent(2760), 1, -1, 0);
            AddComponent(new AddonComponent(2760), -1, 1, 0);
            AddComponent(new AddonComponent(2766), 0, -2, 0);
            AddComponent(new AddonComponent(2766), -1, -2, 0);
            AddComponent(new AddonComponent(2766), 1, -2, 0);
            AddComponent(new AddonComponent(2764), 0, 2, 0);
            AddComponent(new AddonComponent(2764), -1, 2, 0);
            AddComponent(new AddonComponent(2764), 1, 2, 0);
            AddComponent(new AddonComponent(2765), -2, 0, 0);
            AddComponent(new AddonComponent(2765), -2, 1, 0);
            AddComponent(new AddonComponent(2765), -2, -1, 0);
            AddComponent(new AddonComponent(2767), 2, 0, 0);
            AddComponent(new AddonComponent(2767), 2, 1, 0);
            AddComponent(new AddonComponent(2767), 2, -1, 0);
            AddComponent(new AddonComponent(2762), -2, -2, 0);
            AddComponent(new AddonComponent(2763), -2, 2, 0);
            AddComponent(new AddonComponent(2764), 2, -2, 0);
            AddComponent(new AddonComponent(2761), 2, 2, 0);
            Hue = hue;
        }

        public RedCarpetAddon(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class RedCarpetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new RedCarpetAddon(this.Hue); } }
        [Constructable]
        public RedCarpetDeed() { Name = "a red rug deed"; }
        public RedCarpetDeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
    #endregion
}

namespace Server.Mobiles
{
    public class RugMerchant : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override NpcGuild NpcGuild { get { return NpcGuild.TinkersGuild; } }

        [Constructable]
        public RugMerchant() : base("the rug maker")
        {
            SetSkill(SkillName.Carpentry, 85.0, 100.0);
            SetSkill(SkillName.Lumberjacking, 60.0, 83.0);
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRugMerchant());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();
            AddItem(new Server.Items.HalfApron());
        }

        public RugMerchant(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    public class SBRugMerchant : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBRugMerchant() { }

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo("a persian rug deed", typeof(FancyCarpetDeed), 50000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("an asian rug deed", typeof(AsianCarpetDeed), 50000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("a fancy blue rug deed", typeof(FancyBlueCarpetDeed), 25000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("a blue rug deed", typeof(BlueCarpetDeed), 20000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("a plain blue rug deed", typeof(PlainBlueCarpetDeed), 10000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("a fancy red rug deed", typeof(FancyRedCarpetDeed), 25000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("a red rug deed", typeof(RedCarpetDeed), 20000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
