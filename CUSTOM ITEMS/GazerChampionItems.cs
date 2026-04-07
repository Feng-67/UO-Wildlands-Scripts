/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;

namespace Server.Items
{
    // 1. Riftwalker’s Pendant
    public class RiftwalkersPendant : BaseNecklace
    {
        [Constructable]
        public RiftwalkersPendant() : base(0x1089) // Necklace ItemID
        {
            Name = "Riftwalker's Pendant";
            Hue = 1154;
            Attributes.BonusHits = 8;
            Attributes.BonusStam = 2;
            Attributes.BonusMana = 2;
        }

        public RiftwalkersPendant(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 2. Void Scarred Charm
    public class VoidScarredCharm : BaseNecklace
    {
        [Constructable]
        public VoidScarredCharm() : base(0x1089)
        {
            Name = "Void Scarred Charm";
            Hue = 1107;
            Attributes.BonusHits = 3;
            Attributes.BonusStam = 4;
            Attributes.BonusMana = 8;
        }

        public VoidScarredCharm(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 3. Void Shield of Invulnerability
    public class VoidShieldOfInvulnerability : OrderShield
    {
        [Constructable]
        public VoidShieldOfInvulnerability()
        {
            Name = "Void Shield of Invulnerability";
            Hue = 1107;
            Attributes.DefendChance = 15;
            StrRequirement = 50;
            MaxHitPoints = 200;
            HitPoints = 200;
        }

        public VoidShieldOfInvulnerability(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 4. Void Quiver
    public class VoidQuiver : BaseQuiver
    {
        [Constructable]
        public VoidQuiver() : base(0x2FB7)
        {
            Name = "Void Quiver";
            LootType = LootType.Blessed;
            Hue = 1154;
            WeightReduction = 30;
            Attributes.BonusHits = 7;
            Attributes.BonusStam = 1;
            Attributes.BonusMana = 4;

            // FIX: Changed DamageModifier to DamageIncrease to match your BaseQuiver.cs
            DamageIncrease = 10;
        }

        public VoidQuiver(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }

    // 5. Void Essence Vials
    public class VoidEssenceVials : Item
    {
        [Constructable]
        public VoidEssenceVials() : base(0xE24)
        {
            Name = "Vials of Void Essence";
            Hue = 1107;
        }

        public VoidEssenceVials(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
