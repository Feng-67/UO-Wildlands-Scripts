using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    public class BaseHire : BaseCreature
    {
        // ── Configurable (override per subclass if desired) ─────────────
        public virtual int HireFee => 1000000;

        public virtual int RegenHitsBonus => 20;
        public virtual int RegenStamBonus => 20;
        public virtual int RegenManaBonus => 20;

        public virtual TimeSpan RegenInterval    => TimeSpan.FromSeconds(2.0);
        public virtual TimeSpan RecoveryDuration => TimeSpan.FromSeconds(60.0);

        // ── Serialized state ────────────────────────────────────────────
        private bool m_IsHired;
        private bool m_AutoPickupAmmo;
        private bool m_BeQuiet;
        private DateTime m_RecoveryStartedUtc = DateTime.MinValue;

        // ── Runtime state (not serialized) ──────────────────────────────
        private DateTime m_NextAmmoPickup = DateTime.UtcNow;
        private Timer m_RegenTimer;
        private Timer m_KiteTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsHired
        {
            get { return m_IsHired; }
            set
            {
                if (m_IsHired == value) return;
                m_IsHired = value;
                Delta(MobileDelta.Noto);
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AutoPickupAmmo
        {
            get { return m_AutoPickupAmmo; }
            set { m_AutoPickupAmmo = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BeQuiet
        {
            get { return m_BeQuiet; }
            set { m_BeQuiet = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime RecoveryStartedUtc
        {
            get { return m_RecoveryStartedUtc; }
            set { m_RecoveryStartedUtc = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRecovering { get { return m_RecoveryStartedUtc != DateTime.MinValue; } }

        public override bool IsBondable        { get { return true; } }
        public override bool CanAutoStable     { get { return true; } }
        public override bool CanDetectHidden   { get { return false; } }
        public override bool KeepsItemsOnDeath { get { return true; } }

        public override int GetMaxResistance(ResistanceType type) { return 70; }

        // ── Constructors ────────────────────────────────────────────────
        public BaseHire(AIType AI)
            : base(AI, FightMode.Aggressor, 10, 1, 0.1, 4.0)
        {
            ControlSlots = 4;
            EnableDefaultSwitches();
            StartRegen();
            StartKiting();
        }

        public BaseHire()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.1, 4.0)
        {
            ControlSlots = 4;
            EnableDefaultSwitches();
            StartRegen();
            StartKiting();
        }

        public BaseHire(Serial serial) : base(serial) { }

        private void EnableDefaultSwitches()
        {
            m_AutoPickupAmmo = true;
        }

        // ── Serialization ───────────────────────────────────────────────
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(m_IsHired);
            writer.Write(m_AutoPickupAmmo);
            writer.Write(m_BeQuiet);
            writer.WriteDeltaTime(m_RecoveryStartedUtc);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    m_IsHired           = reader.ReadBool();
                    m_AutoPickupAmmo    = reader.ReadBool();
                    m_BeQuiet           = reader.ReadBool();
                    m_RecoveryStartedUtc = reader.ReadDeltaTime();
                    break;
                }
                case 0:
                {
                    // Legacy ServUO BaseHire v0: bool m_IsHired, int m_HoldGold.
                    // m_HoldGold is deprecated (one-time fee system now).
                    m_IsHired = reader.ReadBool();
                    reader.ReadInt(); // discard old m_HoldGold
                    m_AutoPickupAmmo     = true;
                    m_BeQuiet            = false;
                    m_RecoveryStartedUtc = DateTime.MinValue;
                    break;
                }
            }

            StartRegen();
            StartKiting();

            // Restart-safe recovery (replaces ModernUO's [AfterDeserialization]).
            if (m_RecoveryStartedUtc != DateTime.MinValue)
            {
                var remaining = RecoveryDuration - (DateTime.UtcNow - m_RecoveryStartedUtc);

                if (remaining <= TimeSpan.Zero)
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), ReturnFromRecovery);
                else
                    Timer.DelayCall(remaining, ReturnFromRecovery);
            }
        }

        // ── Passive regen ───────────────────────────────────────────────
        private void StartRegen()
        {
            if (m_RegenTimer != null) m_RegenTimer.Stop();
            m_RegenTimer = Timer.DelayCall(RegenInterval, RegenInterval, ApplyRegenTick);
        }

        private void ApplyRegenTick()
        {
            if (Deleted || !Alive || IsDeadPet || !Controlled || IsRecovering) return;

            if (Hits < HitsMax)
                Hits = Math.Min(HitsMax, Hits + RegenHitsBonus);

            if (Stam < StamMax)
                Stam = Math.Min(StamMax, Stam + RegenStamBonus);

            if (Mana < ManaMax)
                Mana = Math.Min(ManaMax, Mana + RegenManaBonus);
        }

        // ── Kiting ──────────────────────────────────────────────────────
        private void StartKiting()
        {
            if (m_KiteTimer != null) m_KiteTimer.Stop();
            m_KiteTimer = Timer.DelayCall(TimeSpan.FromSeconds(0.5),
                                          TimeSpan.FromSeconds(0.5),
                                          KiteTick);
        }

        private void KiteTick()
        {
            if (Deleted || !Alive || IsDeadPet || !Controlled || IsRecovering) return;

            var combatant = Combatant;
            if (combatant == null || combatant.Deleted || !combatant.Alive) return;

            bool ranged =
                FindItemOnLayer(Layer.TwoHanded) is BaseRanged ||
                FindItemOnLayer(Layer.OneHanded) is BaseRanged ||
                AIObject is MageAI;

            if (!ranged) return;

            if (InRange(combatant, 2))
            {
                Direction dir  = GetDirectionTo(combatant);
                Direction flee = (Direction)(((int)dir + 2) & 0x7);
                Move(flee);
            }
        }

        public override void OnDelete()
        {
            if (m_RegenTimer != null) { m_RegenTimer.Stop(); m_RegenTimer = null; }
            if (m_KiteTimer  != null) { m_KiteTimer.Stop();  m_KiteTimer  = null; }
            base.OnDelete();
        }

        // ── Death → Healer recovery ─────────────────────────────────────
        public override bool OnBeforeDeath()
        {
            // Cancel death entirely — no corpse, no ghost, no loot.
            BeginRecovery();
            return false;
        }

        private void BeginRecovery()
        {
            if (IsRecovering) return;

            m_RecoveryStartedUtc = DateTime.UtcNow;
            InvalidateProperties();

            Combatant = null;
            Warmode   = false;

            Poison = null;
            Hits   = HitsMax;
            Stam   = StamMax;
            Mana   = ManaMax;

            int seconds = (int)RecoveryDuration.TotalSeconds;

            PublicOverheadMessage(
                MessageType.Regular,
                0x22,
                false,
                string.Format(
                    "I'M BADLY WOUNDED — I'LL FIND A HEALER AND BE BACK IN {0} SECONDS!",
                    seconds));

            if (ControlMaster != null)
                ControlMaster.SendMessage(
                    0x22,
                    string.Format(
                        "{0} IS BADLY WOUNDED AND HEADING TO THE HEALER. RETURNING IN {1} SECONDS.",
                        Name.ToUpperInvariant(),
                        seconds));

            PlaySound(0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, 2023);

            MoveToWorld(Point3D.Zero, Map.Internal);

            Timer.DelayCall(RecoveryDuration, ReturnFromRecovery);
        }

        private void ReturnFromRecovery()
        {
            if (Deleted)
            {
                m_RecoveryStartedUtc = DateTime.MinValue;
                return;
            }

            Mobile owner = ControlMaster;

            if (owner == null || owner.Deleted)
            {
                m_RecoveryStartedUtc = DateTime.MinValue;
                Delete();
                return;
            }

            if (owner.Map == null || owner.Map == Map.Internal)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), ReturnFromRecovery);
                return;
            }

            m_RecoveryStartedUtc = DateTime.MinValue;
            InvalidateProperties();

            MoveToWorld(owner.Location, owner.Map);

            PublicOverheadMessage(
                MessageType.Regular,
                0x22,
                false,
                "I'VE BEEN PATCHED UP — READY FOR BATTLE!");

            owner.SendMessage(
                0x22,
                string.Format("{0} HAS RETURNED FROM THE HEALER!", Name.ToUpperInvariant()));

            PlaySound(0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, 5023);
        }

        // ── Hiring ──────────────────────────────────────────────────────
        public virtual Mobile GetOwner()
        {
            if (!Controlled) return null;

            Mobile owner = ControlMaster;
            m_IsHired = true;

            if (owner == null) return null;

            if (owner.Deleted)
            {
                Say(1005653); // Hmmm. I seem to have lost my master.
                SetControlMaster(null);
                return null;
            }

            return owner;
        }

        public virtual bool AddHire(Mobile m)
        {
            Mobile owner = GetOwner();

            if (owner != null)
            {
                m.SendLocalizedMessage(1043283, owner.Name); // I am following ~1_NAME~.
                return false;
            }

            if (SetControlMaster(m))
            {
                m_IsHired  = true;
                IsBonded   = true; // bond on hire
                InvalidateProperties();
                return true;
            }

            return false;
        }

        public virtual void TryHireFromBank(Mobile from)
        {
            if (from == null || from.Deleted || Controlled) return;

            int bank = Banker.GetBalance(from);

            if (bank < HireFee)
            {
                SayHireCost();
                from.SendMessage(string.Format(
                    "You have {0:N0} gold in your bank; the fee is {1:N0}.", bank, HireFee));
                return;
            }

            if (from.Followers + ControlSlots > from.FollowersMax)
            {
                SayTo(from, 500896); // I see you already have an escort.
                return;
            }

            if (!Banker.Withdraw(from, HireFee))
            {
                SayTo(from, "Your bank refuses the draft. Try again later.");
                return;
            }

            if (!AddHire(from))
            {
                if (from.BankBox != null)
                    from.BankBox.DropItem(new Gold(HireFee));
                return;
            }

            SayTo(from, string.Format(
                "A pleasure doing business, {0}. {1:N0} gold received.", from.Name, HireFee));
        }

        internal void SayHireCost()
        {
            Say(string.Format("I am available for hire — {0:N0} gold, one time.", HireFee));
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.Mobile.InRange(this, 6) &&
                (e.HasKeyword(0x003B) || e.HasKeyword(0x0162) || e.HasKeyword(0x000C)))
            {
                if (Controlled)
                {
                    Say(1042495); // I have already been hired.
                }
                else
                {
                    e.Handled = true;
                    SayHireCost();
                }
            }

            base.OnSpeech(e);
        }

        // ── Access security ─────────────────────────────────────────────
        public static bool CheckAccess(BaseCreature merc, Mobile from)
        {
            if (from == merc || from.AccessLevel >= AccessLevel.GameMaster) return true;
            if (from.Alive && merc.Controlled && !merc.IsDeadPet && from == merc.ControlMaster) return true;
            return false;
        }

        // ── Automations ─────────────────────────────────────────────────
        public override void OnThink()
        {
            base.OnThink();

            if (!Controlled || Deleted || IsDeadPet || IsRecovering) return;

            if (m_AutoPickupAmmo && DateTime.UtcNow >= m_NextAmmoPickup && HasRangedEquipped())
            {
                TryPickupAmmo();
            }
        }

        private bool HasRangedEquipped()
        {
            return FindItemOnLayer(Layer.TwoHanded) is BaseRanged ||
                   FindItemOnLayer(Layer.OneHanded) is BaseRanged;
        }

        private void TryPickupAmmo()
        {
            if (Backpack == null) return;

            var items = new List<Item>();

            foreach (var item in GetItemsInRange(2))
            {
                if (item.Movable && (item is Arrow || item is Bolt))
                    items.Add(item);
            }

            if (items.Count == 0) return;

            foreach (var item in items)
            {
                if (item.Deleted) continue;
                if (!Backpack.CheckHold(this, item, false, true)) continue;

                Backpack.DropItem(item);
            }

            m_NextAmmoPickup = DateTime.UtcNow + TimeSpan.FromSeconds(5);
        }

        // ── AI switching on equip ───────────────────────────────────────
        public override void OnItemAdded(Item item)
        {
            base.OnItemAdded(item);
            if (item is BaseRanged) ChangeAIType(AIType.AI_Archer);
        }

        public override void OnItemRemoved(Item item)
        {
            base.OnItemRemoved(item);
            if (item is BaseRanged) ChangeAIType(AIType.AI_Melee);
        }

        // ── Context menu ────────────────────────────────────────────────
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (Deleted) return;

            if (!Controlled)
            {
                list.Add(new HireEntry(from, this));
            }
            else
            {
                base.GetContextMenuEntries(from, list);
            }
        }

        public class HireEntry : ContextMenuEntry
        {
            private readonly Mobile   m_Mobile;
            private readonly BaseHire m_Hire;

            public HireEntry(Mobile from, BaseHire hire)
                : base(6120, 3)
            {
                m_Mobile = from;
                m_Hire   = hire;
            }

            public override void OnClick()
            {
                if (m_Hire != null && !m_Hire.Deleted)
                    m_Hire.TryHireFromBank(m_Mobile);
            }
        }
    }
}
