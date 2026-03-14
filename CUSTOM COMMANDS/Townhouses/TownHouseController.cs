/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Voxpire)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.TownHouses
{
    public class TownHouseController : Item
    {
        // Identity / region
        public string RegionName { get; private set; }
        public int RegionPriority { get; private set; }

        // Bounds
        public Rectangle2D Rect { get; private set; }
        public int MinZ { get; private set; }
        public int MaxZ { get; private set; }

        public bool HasBounds { get { return TownHouseUtil.HasBounds(Rect); } }

        public Rectangle2D[] Bounds
        {
            get
            {
                if (!HasBounds)
                    return new Rectangle2D[0];

                return new Rectangle2D[] { Rect };
            }
        }

        // State / economy
        public TownHouseState State { get; private set; }
        public TownHouseWipePolicy WipePolicy { get; private set; }

        public int PurchasePrice { get; private set; }

        // Ownership & access
        public Mobile Owner { get; private set; }
        public List<Mobile> CoOwners { get; private set; }
        public List<Mobile> Friends { get; private set; }
        public List<Mobile> Bans { get; private set; }

        // Owner sale (player-driven)
        public bool OwnerSaleActive { get; private set; }
        public int OwnerSalePrice { get; private set; }

        public bool IsForSale
        {
            get { return Owner != null && !Owner.Deleted && OwnerSaleActive && OwnerSalePrice > 0; }
        }

        // Locks & secures
        public int LockdownLimit { get; private set; }
        public List<Item> Lockdowns { get; private set; }
        public List<Container> Secures { get; private set; }

        // Region instance
        private TownHouseRegion m_Region;

        [Constructable]
        public TownHouseController() : base(0xBD2)
        {
            Name = "town house sign";
            Hue = 1141;
            Movable = true;

            RegionName = "TownHouse";
            RegionPriority = 50;

            Rect = new Rectangle2D(0, 0, 0, 0);
            MinZ = -128;
            MaxZ = 128;

            State = TownHouseState.Purchasable;
            WipePolicy = TownHouseWipePolicy.LeaveAsIs;

            PurchasePrice = 250000;

            Owner = null;
            CoOwners = new List<Mobile>();
            Friends = new List<Mobile>();
            Bans = new List<Mobile>();

            LockdownLimit = 1500;
            Lockdowns = new List<Item>();
            Secures = new List<Container>();

            OwnerSaleActive = false;
            OwnerSalePrice = 0;
            TownHouseSystem.Register(this);
        }

        public TownHouseController(Serial serial) : base(serial) { }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();
            TownHouseSystem.Register(this);
            EnsureRegion();
        }

        public override void OnMapChange()
        {
            base.OnMapChange();
            RemoveRegion();
            EnsureRegion();
        }

        public override void OnDelete()
        {
            RemoveRegion();
            TownHouseSystem.Unregister(this);
            base.OnDelete();
        }

        // ----------------------------------------------------------------------
        // TOOLTIPS & CLICKING
        // ----------------------------------------------------------------------

        public override void OnSingleClick(Mobile from)
        {
            // Do not call base.OnSingleClick to avoid default item label if we want custom ones
            // But usually calling base is fine if we just want to send a message.
            // Here we want the overhead name to be specific.
            
            if (from == null) return;

            // Send the standard item name first (or custom overhead)
            // LabelTo(from, Name); 

            if (Owner == null)
            {
                // Server Sale
                if (State == TownHouseState.Purchasable)
                    LabelTo(from, string.Format("For Sale: {0:N0} gp", PurchasePrice));
                else
                    LabelTo(from, "Unclaimed Property");
            }
            else
            {
                // Player Sale
                if (IsForSale)
                {
                    LabelTo(from, string.Format("Owned by {0} (For Sale: {1:N0} gp)", Owner.Name, OwnerSalePrice));
                }
                else
                {
                    LabelTo(from, string.Format("The Home of {0}", Owner.Name));
                }
            }
        }

 public override void GetProperties(ObjectPropertyList list)
{
    base.GetProperties(list);

    if (Owner == null)
    {
        // Status: Unclaimed
        list.Add(1060659, "Status\tUnclaimed");
    }
    else
    {
        if (IsForSale)
        {
            // Status: FOR SALE BY OWNER
            list.Add(1060659, "Status\tFOR SALE BY OWNER");
            
            // This displays as: "100,000 GP"
            // We use 1070722 because it is a generic "~1_val~" cliloc with no built-in labels or colons.
            list.Add(1070722, string.Format("{0:N0} GP", OwnerSalePrice));
        }
        else
        {
            // Status: Private Residence
            list.Add(1060659, "Status\tPrivate Residence");
        }
    }
}

        public override void OnDoubleClick(Mobile from)
        {
            bool inRegion = (from.Region is TownHouseRegion && ((TownHouseRegion)from.Region).Controller == this);

            if (inRegion || from.InRange(Location, 5) || from.AccessLevel >= AccessLevel.GameMaster)
            {
                from.CloseGump(typeof(TownHouseSignGump));
                from.SendGump(new TownHouseSignGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }

        // ----------------------------------------------------------------------
        // ACCESS & HELPERS
        // ----------------------------------------------------------------------

        private bool SameAccount(Mobile a, Mobile b)
        {
            if (!TownHouseConfig.UseAccountOwnership || a == null || b == null)
                return false;

            return (a.Account != null && b.Account != null && a.Account == b.Account);
        }

        public bool IsOwner(Mobile m) => (m != null && (Owner == m || SameAccount(Owner, m)));

        public bool IsCoOwner(Mobile m)
        {
            if (m == null) return false;
            // CoOwner list check first, then Account check
            return CoOwners.Contains(m) || SameAccount(Owner, m);
        }

        public bool IsFriend(Mobile m)
        {
            if (m == null) return false;
            return IsOwner(m) || IsCoOwner(m) || Friends.Contains(m);
        }

        public bool IsBanned(Mobile m)
        {
            if (m == null) return false;
            return Bans.Contains(m);
        }

        public bool IsInside(Point3D p, Map map)
        {
            if (map == null || map == Map.Internal || map != Map) return false;
            if (!HasBounds) return false;
            return TownHouseUtil.In3DRange(p, Rect, MinZ, MaxZ);
        }

        public void EnsureRegion()
        {
            if (Map == null || Map == Map.Internal || !HasBounds) return;
            if (m_Region != null) return;

            m_Region = new TownHouseRegion(this);
            m_Region.Register();
        }

        public void RemoveRegion()
        {
            if (m_Region != null)
            {
                m_Region.Unregister();
                m_Region = null;
            }
        }

        // GM setters
        public void SetBounds(Rectangle2D rect, int minZ, int maxZ, string regionName)
        {
            Rect = rect;
            MinZ = minZ;
            MaxZ = maxZ;

            if (!string.IsNullOrEmpty(regionName))
                RegionName = regionName;

            RemoveRegion();
            EnsureRegion();
            InvalidateProperties();
        }

        public void ApplySettings(int purchasePrice, int lockdownLimit, int minZ, int maxZ)
        {
            PurchasePrice = Math.Max(0, purchasePrice);
            LockdownLimit = Math.Max(0, lockdownLimit);

            MinZ = minZ;
            MaxZ = maxZ;

            RemoveRegion();
            EnsureRegion();
            InvalidateProperties();
        }

        public void CycleState()
{
    // If we are currently Purchasable, skip Rentable and go straight to Transferable
    if (this.State == TownHouseState.Purchasable)
    {
        this.State = TownHouseState.Transferable;
    }
    else
    {
        // If we are Transferable (or somehow stuck in Rentable), go back to Purchasable
        this.State = TownHouseState.Purchasable;
    }

    InvalidateProperties();
}

        public void ToggleWipePolicy()
        {
            WipePolicy = (WipePolicy == TownHouseWipePolicy.LeaveAsIs)
                ? TownHouseWipePolicy.WipeItemsInBounds
                : TownHouseWipePolicy.LeaveAsIs;

            InvalidateProperties();
        }

        // ----------------------------------------------------------------------
        // TRANSACTIONS
        // ----------------------------------------------------------------------

        public bool TryPurchase(Mobile buyer)
        {
            if (buyer == null) return false;

            if (!HasBounds)
            {
                buyer.SendMessage("This town house is not configured yet.");
                return false;
            }

            if (Owner != null)
            {
                buyer.SendMessage("This town house is already owned.");
                return false;
            }

            if (State != TownHouseState.Purchasable)
            {
                buyer.SendMessage("This town house is not for purchase.");
                return false;
            }

            if (!Banker.Withdraw(buyer, PurchasePrice))
            {
                buyer.SendMessage("You do not have enough gold in the bank.");
                return false;
            }

            AssignOwner(buyer);
            buyer.SendMessage("You have purchased the town house.");
            TownHouseSystem.OnPurchased?.Invoke(this, buyer);

            return true;
        }

        // Deprecated method stub for safety
        public bool TryRent(Mobile renter) { return false; }

        private void AssignOwner(Mobile m)
        {
            Owner = m;

            State = TownHouseState.Transferable;
            OwnerSaleActive = false;
            OwnerSalePrice = 0;

            CoOwners.Clear();
            Friends.Clear();
            Bans.Clear();

            if (WipePolicy == TownHouseWipePolicy.WipeItemsInBounds)
                WipeItemsInside();

            EnsureRegion();
            InvalidateProperties();
        }

        public void EvictOwner()
        {
            Mobile oldOwner = Owner;

            Owner = null;
            CoOwners.Clear();
            Friends.Clear();
            Bans.Clear();

            OwnerSaleActive = false;
            OwnerSalePrice = 0;

            MoveAllMovablesToCrate();

            Lockdowns.Clear();
            Secures.Clear();

            if (WipePolicy == TownHouseWipePolicy.WipeItemsInBounds)
                WipeItemsInside();

            if (oldOwner != null)
                oldOwner.SendMessage("Your town house has been reclaimed.");

            TownHouseSystem.OnEvicted?.Invoke(this);

            InvalidateProperties();
        }

        private void WipeItemsInside()
        {
            if (Map == null || Map == Map.Internal || !HasBounds) return;

            TownHouseUtil.ForEachItemInBounds(Map, Rect, MinZ, MaxZ, delegate(Item item)
            {
                if (item == null || item.Deleted || item == this) return;
                if (item is TownHouseHighlightTile || item is TownHouseMovingCrate) return;
                item.Delete();
            });
        }

        private void MoveAllMovablesToCrate()
        {
            if (Map == null || Map == Map.Internal || !HasBounds) return;

            TownHouseMovingCrate crate = GetOrCreateCrate();

            TownHouseUtil.ForEachItemInBounds(Map, Rect, MinZ, MaxZ, delegate(Item item)
            {
                if (item == null || item.Deleted || item == this) return;
                if (item is TownHouseHighlightTile || item is TownHouseMovingCrate) return;

                bool isLocked = Lockdowns.Contains(item);
                bool isSecure = false;
                Container c = item as Container;
                if (c != null) isSecure = Secures.Contains(c);

                if (!item.Movable && !isLocked && !isSecure) return;

                if (crate != null && !crate.Deleted)
                    crate.DropItem(item);
                else
                    item.Delete();
            });
        }

        private TownHouseMovingCrate GetOrCreateCrate()
        {
            TownHouseMovingCrate crate = null;
            IPooledEnumerable e = GetItemsInRange(0);
            try
            {
                foreach (Item item in e)
                {
                    TownHouseMovingCrate mc = item as TownHouseMovingCrate;
                    if (mc != null && !mc.Deleted)
                    {
                        crate = mc;
                        break;
                    }
                }
            }
            finally { e.Free(); }

            if (crate == null)
            {
                crate = new TownHouseMovingCrate();
                crate.MoveToWorld(Location, Map);
            }

            return crate;
        }

        // ----------------------------------------------------------------------
        // ITEMS / SECURES
        // ----------------------------------------------------------------------

        public bool CanLockdown(Mobile m)
        {
            return IsFriend(m) && Lockdowns.Count < LockdownLimit;
        }

        public bool TryLockdown(Mobile m, Item item)
        {
            if (m == null || item == null || item.Deleted) return false;

            if (!IsInside(item.Location, item.Map)) {
                m.SendMessage("That is not inside this town house.");
                return false;
            }

            if (!CanLockdown(m)) {
                m.SendMessage("You cannot lock down more items here.");
                return false;
            }

            if (Lockdowns.Contains(item)) {
                m.SendMessage("That is already locked down.");
                return false;
            }

            item.Movable = false;
            Lockdowns.Add(item);
            m.SendMessage("Item locked down.");
            return true;
        }

        public bool TrySecure(Mobile m, Container c)
        {
            if (m == null || c == null || c.Deleted) return false;

            if (!IsInside(c.Location, c.Map)) {
                m.SendMessage("That is not inside this town house.");
                return false;
            }

            if (!IsFriend(m)) {
                m.SendMessage("You do not have access to do that.");
                return false;
            }

            if (Secures.Contains(c)) {
                m.SendMessage("That is already secured.");
                return false;
            }

            c.Movable = false;
            Secures.Add(c);
            m.SendMessage("Container secured.");
            return true;
        }

        public bool TryRelease(Mobile m, Item item)
        {
            if (m == null || item == null || item.Deleted) return false;

            // STRICT ACCESS CHECK:
            // - Owners and CoOwners can release anything.
            // - Friends can only release items they personally locked down? 
            //   (Current simplified logic: Friends cannot release at all to be safe, 
            //   or you can track who locked it down. Standard UO: Friends cannot release.)
            
            if (!IsCoOwner(m) && !IsOwner(m) && m.AccessLevel < AccessLevel.GameMaster)
            {
                m.SendMessage("You must be at least a Co-Owner to release items.");
                return false;
            }

            if (Lockdowns.Remove(item)) {
                item.Movable = true;
                m.SendMessage("Item released.");
                return true;
            }

            Container c = item as Container;
            if (c != null && Secures.Remove(c)) {
                c.Movable = true;
                m.SendMessage("Secure released.");
                return true;
            }

            m.SendMessage("That is not locked down or secured.");
            return false;
        }

        // ----------------------------------------------------------------------
        // ACCESS LISTS
        // ----------------------------------------------------------------------

        public bool TryAddFriend(Mobile from, Mobile target)
        {
            if (!IsOwner(from) && !IsCoOwner(from) && from.AccessLevel < AccessLevel.GameMaster) return false;
            
            if (target == null || target.Deleted || !target.Player) return false;
            if (Friends.Contains(target) || IsOwner(target) || IsCoOwner(target)) return false;
            Friends.Add(target);
            return true;
        }

        public bool TryAddCoOwner(Mobile from, Mobile target)
        {
            // STRICT CHECK: Only Owner can add CoOwners
            if (!IsOwner(from) && from.AccessLevel < AccessLevel.GameMaster) 
            {
                from.SendMessage("Only the owner can add Co-Owners.");
                return false;
            }

            if (target == null || target.Deleted || !target.Player) return false;
            if (CoOwners.Contains(target) || IsOwner(target)) return false;
            
            CoOwners.Add(target);
            Friends.Remove(target);
            Bans.Remove(target);
            return true;
        }

        public bool TryBan(Mobile from, Mobile target)
        {
            if (!IsOwner(from) && !IsCoOwner(from) && from.AccessLevel < AccessLevel.GameMaster) return false;
            
            if (target == null || target.Deleted || !target.Player) return false;
            if (IsOwner(target)) return false; // Cannot ban owner
            
            if (!Bans.Contains(target)) Bans.Add(target);
            
            Friends.Remove(target);
            CoOwners.Remove(target);
            // Optionally eject them now
            return true;
        }

        public bool TryRemoveFriend(Mobile from, Mobile target)
        {
            if (!IsOwner(from) && !IsCoOwner(from) && from.AccessLevel < AccessLevel.GameMaster) return false;
            return Friends.Remove(target);
        }

        public bool TryRemoveCoOwner(Mobile from, Mobile target)
        {
            // STRICT CHECK: Only Owner can remove CoOwners
            if (!IsOwner(from) && from.AccessLevel < AccessLevel.GameMaster)
            {
                from.SendMessage("Only the owner can remove Co-Owners.");
                return false;
            }
            return CoOwners.Remove(target);
        }

        public bool TryUnban(Mobile from, Mobile target)
        {
            if (!IsOwner(from) && !IsCoOwner(from) && from.AccessLevel < AccessLevel.GameMaster) return false;
            return Bans.Remove(target);
        }

        public bool TryTransfer(Mobile from, Mobile to)
        {
            if (from == null || to == null) return false;
            if (State != TownHouseState.Transferable) {
                from.SendMessage("This town house is not transferable.");
                return false;
            }
            if (!IsOwner(from)) {
                from.SendMessage("You are not the owner.");
                return false;
            }
            if (to.Deleted || !to.Player) {
                from.SendMessage("That is not a valid player.");
                return false;
            }

            Owner = to;
            CoOwners.Clear();
            Friends.Clear();
            Bans.Clear();

            from.SendMessage("You have transferred the town house.");
            to.SendMessage("You are now the owner of this town house.");
            InvalidateProperties();
            return true;
        }

        // ----------------------------------------------------------------------
        // PLAYER SALES
        // ----------------------------------------------------------------------

        public bool TryBeginSale(Mobile from, int price)
        {
            if (from == null) return false;
            if (Owner == null || Owner.Deleted) return false;
            if (!IsOwner(from)) {
                from.SendMessage("You are not the owner.");
                return false;
            }
            if (price <= 0) {
                from.SendMessage("Sale price must be greater than zero.");
                return false;
            }

            OwnerSaleActive = true;
            OwnerSalePrice = price;

            from.SendMessage("This town house is now for sale.");
            InvalidateProperties(); // Updates Tooltip!
            return true;
        }

        public bool TryCancelSale(Mobile from)
        {
            if (from == null) return false;
            if (!IsOwner(from) && from.AccessLevel < AccessLevel.GameMaster) return false;

            OwnerSaleActive = false;
            OwnerSalePrice = 0;

            from.SendMessage("Sale cancelled.");
            InvalidateProperties(); // Updates Tooltip!
            return true;
        }

        public bool TryAbandon(Mobile from)
        {
            if (from == null) return false;
            if (!IsOwner(from) && from.AccessLevel < AccessLevel.GameMaster) return false;

            EvictOwner(); 
            from.SendMessage("You have abandoned the town house.");
            return true;
        }

        public bool TryPurchaseFromOwner(Mobile buyer)
        {
            if (buyer == null) return false;

            if (!IsForSale) {
                buyer.SendMessage("This town house is not for sale.");
                return false;
            }

            if (Owner == null || Owner.Deleted) {
                buyer.SendMessage("This town house has no owner.");
                return false;
            }

            if (IsOwner(buyer)) {
                buyer.SendMessage("You already own this town house.");
                return false;
            }

            int price = OwnerSalePrice;

            if (!Banker.Withdraw(buyer, price)) {
                buyer.SendMessage("You do not have enough gold in your bank.");
                return false;
            }

            Mobile oldOwner = Owner;

            if (!GiveSaleProceeds(oldOwner, price))
            {
                GiveSaleProceeds(buyer, price); // Refund
                buyer.SendMessage("Sale failed: the seller could not receive payment.");
                return false;
            }

            Owner = buyer;
            CoOwners.Clear();
            Friends.Clear();
            Bans.Clear();
            OwnerSaleActive = false;
            OwnerSalePrice = 0;

            buyer.SendMessage("You have purchased the town house for " + price.ToString("N0") + " gp.");
            if (oldOwner != null && !oldOwner.Deleted)
                oldOwner.SendMessage("Your town house has been sold for " + price.ToString("N0") + " gp.");

            // FORCE REFRESH OF TOOLTIP
            InvalidateProperties(); 
            
            // OPTIONAL: Resend gump if open, though usually the gump handles its own refresh.
            // This ensures the sign Item visually updates for anyone looking at it.
            return true;
        }

        private static bool GiveSaleProceeds(Mobile m, int amount)
        {
            if (m == null || m.Deleted || amount <= 0) return false;

            try
            {
                if (Banker.Deposit(m, amount)) return true;

                Item check = null;
                try { check = new BankCheck(amount); } catch { check = null; }

                Container bank = m.BankBox;
                if (bank != null && !bank.Deleted && check != null && bank.TryDropItem(m, check, false)) return true;

                Container pack = m.Backpack;
                if (pack != null && !pack.Deleted)
                {
                    if (check != null) {
                        if (pack.TryDropItem(m, check, false)) return true;
                    } else {
                        Item gold = new Gold(amount);
                        if (pack.TryDropItem(m, gold, false)) return true;
                    }
                }

                if (check != null) {
                    check.MoveToWorld(m.Location, m.Map);
                    return true;
                }

                Item g2 = new Gold(amount);
                g2.MoveToWorld(m.Location, m.Map);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ----------------------------------------------------------------------
        // MISC
        // ----------------------------------------------------------------------

        public void ShowHighlight(Mobile gm, TimeSpan duration)
        {
            if (gm == null || Map == null || Map == Map.Internal || !HasBounds) return;

            int step = Math.Max(1, TownHouseConfig.HighlightStep);
            for (int x = Rect.X; x < Rect.X + Rect.Width; x += step)
            {
                for (int y = Rect.Y; y < Rect.Y + Rect.Height; y += step)
                {
                    int z = Map.GetAverageZ(x, y);
                    Point3D p = new Point3D(x, y, z);
                    if (z < MinZ || z > MaxZ) continue;
                    TownHouseHighlightTile t = new TownHouseHighlightTile();
                    t.MoveToWorld(p, Map);
                    t.Start(duration);
                }
            }
            gm.SendMessage("Highlight placed.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(2); // version

            writer.Write(RegionName);
            writer.Write(RegionPriority);

            writer.Write(Rect);
            writer.Write(MinZ);
            writer.Write(MaxZ);

            writer.Write((int)State);
            writer.Write((int)WipePolicy);

            writer.Write(PurchasePrice);
            // Legacy Rent fields for compatibility with older saves
            writer.Write(0); // RentPrice
            writer.Write(0); // RentDays

            writer.Write(Owner);

            writer.Write(CoOwners.Count);
            for (int i = 0; i < CoOwners.Count; i++) writer.Write(CoOwners[i]);

            writer.Write(Friends.Count);
            for (int i = 0; i < Friends.Count; i++) writer.Write(Friends[i]);

            writer.Write(Bans.Count);
            for (int i = 0; i < Bans.Count; i++) writer.Write(Bans[i]);

            writer.Write(OwnerSaleActive);
            writer.Write(OwnerSalePrice);

            writer.Write(LockdownLimit);

            writer.Write(Lockdowns.Count);
            for (int i = 0; i < Lockdowns.Count; i++) writer.Write(Lockdowns[i]);

            writer.Write(Secures.Count);
            for (int i = 0; i < Secures.Count; i++) writer.Write(Secures[i]);

            // Legacy RentPaidUntil
            writer.Write(DateTime.MinValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                RegionName = reader.ReadString();
                RegionPriority = reader.ReadInt();

                Rect = reader.ReadRect2D();
                MinZ = reader.ReadInt();
                MaxZ = reader.ReadInt();

                State = (TownHouseState)reader.ReadInt();
                WipePolicy = (TownHouseWipePolicy)reader.ReadInt();

                PurchasePrice = reader.ReadInt();
                int dummyRentPrice = reader.ReadInt();
                int dummyRentDays = reader.ReadInt();

                Owner = reader.ReadMobile();

                int co = reader.ReadInt();
                CoOwners = new List<Mobile>(co);
                for (int i = 0; i < co; i++) {
                    Mobile m = reader.ReadMobile();
                    if (m != null) CoOwners.Add(m);
                }

                int fr = reader.ReadInt();
                Friends = new List<Mobile>(fr);
                for (int i = 0; i < fr; i++) {
                    Mobile m = reader.ReadMobile();
                    if (m != null) Friends.Add(m);
                }

                int bn = reader.ReadInt();
                Bans = new List<Mobile>(bn);
                for (int i = 0; i < bn; i++) {
                    Mobile m = reader.ReadMobile();
                    if (m != null) Bans.Add(m);
                }

                if (version >= 2) {
                    OwnerSaleActive = reader.ReadBool();
                    OwnerSalePrice = reader.ReadInt();
                } else {
                    OwnerSaleActive = false;
                    OwnerSalePrice = 0;
                }

                LockdownLimit = reader.ReadInt();

                int ld = reader.ReadInt();
                Lockdowns = new List<Item>(ld);
                for (int i = 0; i < ld; i++) {
                    Item item = reader.ReadItem();
                    if (item != null) Lockdowns.Add(item);
                }

                int sc = reader.ReadInt();
                Secures = new List<Container>(sc);
                for (int i = 0; i < sc; i++) {
                    Container c = reader.ReadItem() as Container;
                    if (c != null) Secures.Add(c);
                }

                DateTime dummyRentDate = reader.ReadDateTime();
            }
            else
            {
                // fallback init
                RegionName = "TownHouse";
                RegionPriority = 50;
                Rect = new Rectangle2D(0, 0, 0, 0);
                MinZ = -128;
                MaxZ = 128;
                State = TownHouseState.Purchasable;
                WipePolicy = TownHouseWipePolicy.LeaveAsIs;
                PurchasePrice = 250000;
                Owner = null;
                CoOwners = new List<Mobile>();
                Friends = new List<Mobile>();
                Bans = new List<Mobile>();
                LockdownLimit = 125;
                Lockdowns = new List<Item>();
                Secures = new List<Container>();
                OwnerSaleActive = false;
                OwnerSalePrice = 0;
            }

            TownHouseSystem.Register(this);
            EnsureRegion();
        }
    }

    public class TownHouseMovingCrate : WoodenBox
    {
        [Constructable]
        public TownHouseMovingCrate()
        {
            Name = "town house moving crate";
            Hue = 1109;
            Movable = false;
        }

        public TownHouseMovingCrate(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
