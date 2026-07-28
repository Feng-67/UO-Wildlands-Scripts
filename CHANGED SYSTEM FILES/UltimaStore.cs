using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server.Engines.Quests.Hag;
using Server.Commands;
using Server.Mobiles;
using Server.Items;
using ShrinkSystem;
using Server.Engines.VendorSearching;
using Server.Gumps;
using Server.Network;
using Server.Engines.Points;
using Server.Multis;

namespace Server.Engines.UOStore
{
    public enum StoreCategory
    {
        None,
        Featured,
        Character,
        Equipment,
        Decorations,
        Mounts,
        Dyes,
        PetDyes,
        Cart
    }

    public enum SortBy
    {
        Name,
        PriceLower,
        PriceHigher,
        Newest,
        Oldest
    }

    public static class UltimaStore
    {
        public static readonly string FilePath = Path.Combine("Saves/Misc", "UltimaStore.bin");

        public static bool Enabled { get { return Configuration.Enabled; } set { Configuration.Enabled = value; } }

        public static List<StoreEntry> Entries { get; private set; }
        public static Dictionary<Mobile, List<Item>> PendingItems { get; private set; }

        private static UltimaStoreContainer _UltimaStoreContainer;

        public static UltimaStoreContainer UltimaStoreContainer
        {
            get
            {
                if (_UltimaStoreContainer != null && _UltimaStoreContainer.Deleted)
                {
                    _UltimaStoreContainer = null;
                }

                return _UltimaStoreContainer ?? (_UltimaStoreContainer = new UltimaStoreContainer());
            }
        }

        static UltimaStore()
        {
            Entries = new List<StoreEntry>();
            PendingItems = new Dictionary<Mobile, List<Item>>();
            PlayerProfiles = new Dictionary<Mobile, PlayerProfile>();
        }

        public static void Configure()
        {
            PacketHandlers.Register(0xFA, 1, true, UOStoreRequest);

            CommandSystem.Register("Store", AccessLevel.Player, e => OpenStore(e.Mobile as PlayerMobile));

            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        public static void Initialize()
        {
            // Featured
            StoreCategory cat = StoreCategory.Featured;
           //Register<VirtueShield>(1109616, 1158384, 0x7818, 0, 0, 1500, cat);
           //Register<SoulstoneToken>(1158404, 1158405, 0x2A93, 0, 2598, 1000, cat, ConstructSoulstone);
           //Register<DeluxeStarterPackToken>(1158368, 1158369, 0, 0x9CCB, 0, 2000, cat);
           //Register<GreenGoblinStatuette>(1125133, 1158015, 0xA095, 0, 0, 600, cat);
           //Register<TotemOfChromaticFortune>(1157606, 1157604, 0, 0x9CC9, 0, 300, cat);
           //Register<MythicCharacterToken>(new TextDefinition[] { 1156614, 1156615 }, 1156679, 0x2AAA, 0, 0, 2500, cat);

            // Character
            cat = StoreCategory.Character;

            // Format: Type, NameCliloc, DescCliloc, ItemID, Hue, TooltipCliloc, Price, Category

            // Category: Power Scrolls / Miscellaneous
            Register<ChampionPowerScroll>("Champion's Power Scroll", 1041088, 0x2258, 1021108, 0, 5000, cat);
            Register<MythicCharacterToken>(new TextDefinition[] { 1156614, 1156615 }, 1156679, 0x2AAA, 0, 0, 2500, cat);
            Register<CharacterReincarnationToken>(new TextDefinition[] { 1156612, 1156615 }, 1156677, 0x2AAA, 0, 0, 2000, cat);
            Register<GenderChangeToken>(new TextDefinition[] { 1156609, 1156615 }, 1156642, 0x2AAA, 0, 0, 1000, cat);
            Register<NameChangeToken>(new TextDefinition[] { 1156608, 1156615 }, 1156641, 0x2AAA, 0, 0, 1000, cat);
            Register<RaceChangeToken>("Race Change Token", 0, 0x2AAA, 0, 0, 1000, cat);
            Register<HABPromotionalToken>(new TextDefinition[] { 1158741, 1156615 }, 1158740, 0x2AAA, 0, 0, 600, cat);
            Register<StableSlotIncreaseToken>(1157608, 1157609, 0x2AAA, 0, 0, 500, cat);
            Register<MysticalPolymorphTotem>(1158780, 1158781, 0xA276, 0, 0, 600, cat);
            //Register<DeluxeStarterPackToken>(1158368, 1158369, 0, 0x9CCB, 0, 2000, cat);
            Register<GreenGoblinStatuette>(1125133, 1158015, 0xA095, 0, 0, 600, cat);
            Register<GreyGoblinStatuette>(1125135, 1158015, 0xA097, 0, 0, 600, cat);

            // Equipment
            cat = StoreCategory.Equipment;

            // Format: Type, NameCliloc, DescCliloc, ItemID, Hue, TooltipCliloc, Price, Category

            Register<TransmogPotion>(1159501, 1159501, 0x0EFF, 0, 1150, 100, cat);
            Register<PetBondingPotion>(1152921, 1156678, 0, 0x9CBC, 0, 175, cat);
            Register<ElixirOfRebirth>(1112762, 1112762, 0x24E2, 0x48E, 1112762, 10, cat);
            Register<PetLeash>("Pet Leash", 0, 0x1374, 1153, 1153, 200, cat);
            Register(typeof(ScrollOfDecurse), "Scroll of Decurse", 0, 0xA1E4, 1266, 0, 500, cat);
            Register(typeof(ScrollOfAntiqueToPrized), "Scroll of Antique to Prized", 0, 0xA1E4, 1638, 0, 750, cat);

            Register<FullMagerySpellbook>("Magery Spellbook", 0, 0xEFA, 0, 0, 10, cat);
            Register<FullNecroSpellbook>("Necromany Spellbook", 0, 0x2253, 0, 0, 10, cat);
            Register<FullMysticBook>("Mysticism Spellbook", 0, 0x2D9D, 0, 0, 10, cat);
            Register<MasteriesBookFull>("Masteries Spellbook", 0, 0x2252, 0, 0, 20, cat);
            Register<CitiesOfBritannia>("Cites of Britannia", 0, 0x9C16, 1154, 0, 50, cat);
            Register<RunicAtlasExceptional>("Runic Atlas", 0, 0x9C16, 0, 0, 20, cat);
            Register<SpellweavingBookFull>("Spellweaving Spellbook", 0, 0x2D50, 0, 0, 40, cat);
            Register<PowerScrollBook>("Power Scroll Book", 1155684, 0x9A95, 1153, 1153, 500, cat);
            Register<MarkRuneStone>("Mark Rune", 0, 0x1F14, 0x481, 0, 100, cat);
            Register<LargeBODBox>("Large BOD Deed Box", 0, 0x9AA, 0, 0, 200, cat);

            Register<ReagentStorageChest>("Reagent Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<ScrollStorageChest>("Scroll Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<LogStorageChest>("Log Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<MinerStorageChest>("Miner Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<TailorStorageChest>("Tailor Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<FoodStorageChest>("Food Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<MondainStorageChest>("Mondain's Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<StygianAbyssStorageChest>("Stygian Abyss Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<HighSeasStorageChest>("High Seas Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<DyeStorageChest>("Dye Storage Chest", 0, 0xE7C, 0, 0, 250, cat);
            Register<RefinementCabinet>("Refinement Cabinet", 0, 0xB2E7, 0, 0, 250, cat);

            Register<DavyJonesPoker>("Davy Jones' Iron Poker", 1041088, 0xF62, 2122, 1315, 50, cat);
            Register<BreakParalysisPotion>("Break Paralysis Potion", 0, 0xF09, 2543, 0, 5, cat);
            Register<HangoverCure>("Hag's Hangover Cure", 0, 0xE2B, 0x2D, 0, 100, cat);
            Register<PowderOfTemperament>("Powder of Fortification", 0, 4102, 2419, 0, 35, cat);
            Register<ClothingBlessDeed>("Clothing Bless Deed", 0, 0x14F0, 0, 0, 500, cat);
            Register<MannequinDeed>("Mannequin Deed", 0, 0x14F0, 0, 0, 500, cat);
            
            Register<InstrumentCase>("Instrument Case", 0, 0xE7D, 0, 0, 150, cat);
            Register<RunebookStrap>("Runebook Strap", 0, 0xA721, 0, 0, 200, cat);
            Register<SpellbookStrap>("Spellbook Strap", 0, 0xA71F, 0, 0, 250, cat);
            Register<TrashBarrelPortable>("Portable Trash Barrel", 0, 0xE77, 0x386, 0, 25, cat);
            Register<HooksShield>("Hook's Shield", 0, 0xA64A, 0, 0, 1000, cat);
            Register<LordMorphiusEpaulettes>("Lord Morphius' Epaulettes", 0, 0x9985, 0, 0, 10000, cat);
            Register<ChampionMonolith>("Champion Monolith", 0, 41066, 0, 0, 1000, cat);
            Register<VaseOfVirtue>("Vase of Virtue", 0, 0xB189, 0, 0, 1000, cat);
            Register<LockpickingChest>("Lockpicking Chest", 0, 0x9AA, 0, 0, 50, cat);
                        
            Register<GypsyHeaddress>("Gypsy Headdress", 1073254, 0x1544, 0x453, 0x453, 250, cat);
            Register<NystulsWizardsHat>("Nystul's Wizard Hat", 1073255, 0x1718, 0x453, 0x453, 250, cat);
            Register<JesterHatOfChuckles>("Jester Hat of Chuckles", 1073256, 0x171C, 0x453, 0x453, 250, cat);
            Register<KeeoneansChainMail>("Keeonean's Chain Mail", 1073264, 0x13BF, 0x84E, 0x84E, 250, cat);
            Register<ClaininsSpellbook>("Clainin's Spellbook", 1073262, 0xEFA, 0x84D, 0x84D, 250, cat);
            Register<VesperOrderShield>("Vesper Order Shield", 1073258, 0x1BC4, 0x835, 0x835, 250, cat);
            Register<VesperChaosShield>("Vesper Chaos Shield", 1073259, 0x1BC3, 0xFA, 0xFA, 250, cat);
            Register<BlackthornsKryss>("Blackthorn's Kryss", 1073260, 0x1401, 0x5E5, 0x5E5, 250, cat);
            Register<SwordOfJustice>("Sword of Justice", 1073261, 0x13B9, 0x47E, 0x47E, 250, cat);
            Register<GeoffreysAxe>("Geoffrey's Axe", 1073263, 0xF45, 0x21, 0x21, 250, cat);

            Register<DiscoKilt>("Discordance Kilt", 0, 0x1537, 1284, 0, 250, cat);
            Register<PeaceSash>("Peace Sash", 0, 0x1541, 1284, 0, 250, cat);
            Register<ProvoCloak>("Provocation Cloak", 0, 0x1515, 1284, 0, 250, cat);
            Register<BardDragonElementalSlayer>("Dragon Elemental Slayer", 0, 0x0EB3, 0x851, 0, 250, cat);
            Register<BardRepondFeySlayer>("Repond Fey Slayer", 0, 0x0EB3, 0x978, 0, 250, cat);
            Register<BardUndeadDemonSlayer>("Undead Demon Slayer", 0, 0x0EB3, 0x59D, 0, 250, cat);
                        
            Register<RoseOfStrength>("Rose of Strength", 1156960, 0x0EB0, 0, 1645, 100, cat);
            Register<RoseOfIntelligence>("Rose of Intelligence", 1156960, 0x0EB0, 0, 2498, 100, cat);
            Register<RoseOfDexterity>("Rose of Dexterity", 1156960, 0x0EB0, 0, 33, 100, cat);

            Register<SoulstoneToken>(1158404, 1158405, 0x2A93, 0, 2598, 1000, cat, ConstructSoulstone);
            Register<RedSoulstone>(1078836, 0, 0x32F6, 0, 0, 1000, cat);
            Register<SoulstoneToken>(1078835, 1158405, 0x2ADC, 0, 0, 1000, cat, ConstructSoulstone);
            Register<SoulstoneToken>(1078834, 1158405, 0x2A93, 0, 0, 1000, cat, ConstructSoulstone);
            Register<CommodityDeedBox>(1080523, 0, 0x9AA, 0, 0x4AB, 500, cat);
            Register<CrystalPortal>(1113945, 0, 0x468B, 0, 0, 1500, cat);
            Register<CorruptedCrystalPortal>(1150074, 0, 0x468B, 0, 0, 1500, cat);

            Register<WeaponEngravingTool>(1076158, 0, 0x1028, 0, 0, 750, cat);
            Register<EtherealRetouchingTool>(1113814, 0, 0x1028, 0, 0x481, 750, cat);
            Register<PetBrandingIron>(1157314, 1157372, 0, 0x9CC3, 0, 600, cat);
            Register<ImprovedRockHammer>(1157177, 1157306, 0, 0x9CBB, 0, 1000, cat);
            Register<ForgedMetalOfArtifacts>(new TextDefinition[] { 1149868, 1156686 }, 1156674, 0, 0x9C65, 0, 1000, cat, ConstructForgedMetal);
            Register<ForgedMetalOfArtifacts>(new TextDefinition[] { 1149868, 1156687 }, 1156675, 0, 0x9C65, 0, 600, cat, ConstructForgedMetal);
            Register<PenOfWisdom>(1115358, 1156669, 0, 0x9C62, 0, 600, cat);
            Register<BritannianShipDeed>(1150100, 1156673, 0, 0x9C6A, 0, 1200, cat);
            Register<VirtueShield>(1109616, 1158384, 0x7818, 0, 0, 1500, cat);
            Register<SmugglersEdge>(1071499, 1156664, 0, 0x9C63, 0, 400, cat);
            Register<UndertakersStaff>(1071498, 1156663, 0x13F8, 0, 0, 1000, cat);
            Register<ReptalonFormTalisman>(new TextDefinition[] { 1157010, 1075202 }, 1156967, 0x2F59, 0, 0, 100, cat);
            Register<QuiverOfInfinity>(1075201, 1156971, 0x2B02, 0, 0, 100, cat);
            Register<CuSidheFormTalisman>(new TextDefinition[] { 1157010, 1031670 }, 1156970, 0x2F59, 0, 0, 100, cat);
            Register<FerretFormTalisman>(new TextDefinition[] { 1157010, 1031672 }, 1156969, 0x2F59, 0, 0, 100, cat);

            Register<LeggingsOfEmbers>(1062911, 1156956, 0x1411, 0, 0x2C, 100, cat);
            Register<ShaminoCrossbow>(1062915, 1156957, 0x26C3, 0, 0x504, 100, cat);
            Register<SamuraiHelm>(1062923, 1156959, 0x236C, 0, 0, 100, cat);
            Register<HolySword>(1062921, 1156962, 0xF61, 0, 0x482, 100, cat);
            Register<DupresShield>(1075196, 1156963, 0x2B01, 0, 0, 100, cat);
            Register<OssianGrimoire>(1078148, 1156965, 0x2253, 0, 0, 100, cat);
            Register<SquirrelFormTalisman>(new TextDefinition[] { 1157010, 1031671 }, 1156966, 0x2F59, 0, 0, 100, cat);
            Register<BagOfBulkOrderCovers>(1071116, 1157603, 0, 0x9CC6, 0, 200, cat, ConstructBOBCoverOne);
            Register<MerchantsTrinket>(new TextDefinition[] { 1156827, 1156681 }, 1156666, 0, 0x9C67, 0, 300, cat, ConstructMerchantsTrinket);
            Register<MerchantsTrinket>(new TextDefinition[] { 1156828, 1156682 }, 1156667, 0, 0x9C67, 0, 500, cat, ConstructMerchantsTrinket);
            Register<ArmorEngravingToolToken>(1080547, 1156652, 0, 0x9C65, 0, 200, cat);
            Register<BagOfBulkOrderCovers>(1071116, 1156654, 0, 0x9CC6, 0, 200, cat, ConstructBOBCoverTwo);
                        
            Register<StoreValoriteCloak>("Valorite Cloak", 1041296, 0x1515, 2210, 2210, 250, cat);
            Register<StoreValoriteRobe>("Valorite Robe", 1041297, 0x1F03, 2210, 2210, 250, cat);
            Register<StoreValoriteDress>("Valorite Dress", 1080371, 0x1F01, 2210, 2210, 250, cat);
            Register<StoreGargishValoriteRobe>("Gargish Valorite Robe", 1113885, 0x46AA, 2210, 2210, 250, cat);
            Register<StoreGargishValoriteFancyRobe>("Gargish Valorite Fancy Robe", 1113884, 0x46AB, 2210, 2210, 250, cat);

            Register<EarringsOfProtection>(new TextDefinition[] { 1156821, 1156822 }, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Physcial
            Register<EarringsOfProtection>(1071092, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Fire
            Register<EarringsOfProtection>(1071093, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Cold
            Register<EarringsOfProtection>(1071094, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Poison
            Register<EarringsOfProtection>(1071095, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Energy

            Register<HoodedShroudOfShadows>(1079727, 1156643, 0x2684, 0, 0x455, 1000, cat);
            Register<HoodedBritanniaRobe>(1125155, 1158016, 0xA0AB, 0, 0, 1500, cat, ConstructRobe);
            Register<HoodedBritanniaRobe>(1125155, 1158016, 0xA0AC, 0, 0, 1500, cat, ConstructRobe);
            Register<HoodedBritanniaRobe>(1125155, 1158016, 0xA0AD, 0, 0, 1500, cat, ConstructRobe);
            Register<HoodedBritanniaRobe>(1125155, 1158016, 0xA0AE, 0, 0, 1500, cat, ConstructRobe);
            Register<HoodedBritanniaRobe>(1125155, 1158016, 0xA0AF, 0, 0, 1500, cat, ConstructRobe);
                      
                                 
            // decorations
            cat = StoreCategory.Decorations;

            // Format: Type, NameCliloc, DescCliloc, ItemID, Hue, TooltipCliloc, Price, Category

            Register<BannerDeed>(1006048, 0, 0x14F0, 0, 0, 100, cat);
            Register<FlamingHeadDeed>(1006049, 0, 0x14F0, 0, 0, 100, cat);
            Register<MinotaurStatueDeed>(1080409, 0, 0x14F0, 0, 0, 100, cat);
            Register<PottedCactusDeed>(1080407, 0, 0x14F0, 0, 0, 100, cat);
            Register<DecorativeShieldDeed>(1049737, 0, 0x14F0, 0, 0, 100, cat);
            Register<HangingSkeletonDeed>(1049738, 0, 0x14F0, 0, 0, 300, cat);
            Register<StoneAnkhDeed>(1049739, 0, 0x14F0, 0, 0, 500, cat);
            Register<BloodyPentagramDeed>(1080384, 0, 0x14F0, 0, 0, 500, cat);
            Register<GardenShedDeed>(1153491, 0, 0x14F0, 0, 0, 1000, cat);
            Register<DecorativeKitchenSet>(1158970, 1158971, 0, 0x9CE8, 0, 1200, cat);
            Register<SquirrelMailbox>(1158859, 1158857, 0xA207, 0, 0, 400, cat);
            Register<BarrelMailbox>(1158859, 1158857, 0xA1F7, 0, 0, 400, cat);

            Register<DecorativeBlackwidowDeed>(1157897, 1157898, 0, 0x9CD7, 0, 600, cat);
            Register<HildebrandtDragonRugDeed>(1157889, 1157890, 0, 0x9CD8, 0, 700, cat);
            Register<SmallWorldTreeRugAddonDeed>(1157206, 1157898, 0, 0x9CBA, 0, 300, cat);
            Register<LargeWorldTreeRugAddonDeed>(1157207, 1157898, 0, 0x9CBA, 0, 500, cat);
            Register<MountedPixieWhiteDeed>(new TextDefinition[] { 1074482, 1156915 }, 1156974, 0x2A79, 0, 0, 100, cat);
            Register<MountedPixieLimeDeed>(new TextDefinition[] { 1074482, 1156914 }, 1156974, 0x2A77, 0, 0, 100, cat);
            Register<MountedPixieBlueDeed>(new TextDefinition[] { 1074482, 1156913 }, 1156974, 0x2A75, 0, 0, 100, cat);
            Register<MountedPixieOrangeDeed>(new TextDefinition[] { 1074482, 1156912 }, 1156974, 0x2A73, 0, 0, 100, cat);
            Register<MountedPixieGreenDeed>(new TextDefinition[] { 1074482, 1156911 }, 1156974, 0x2A71, 0, 0, 100, cat);
            Register<UnsettlingPortraitDeed>(1074480, 1156973, 0x2A65, 0, 0, 100, cat);
            Register<CreepyPortraitDeed>(1074481, 1156972, 0x2A69, 0, 0, 100, cat);
            Register<DisturbingPortraitDeed>(1074479, 1156955, 0x2A5D, 0, 0, 100, cat);
            Register<DawnsMusicBox>(1075198, 1156968, 0x2AF9, 0, 0, 100, cat);
            Register<BedOfNailsDeed>(1074801, 1156975, 0, 0x9C8D, 0, 100, cat);
            Register<BrokenCoveredChairDeed>(1076257, 1156950, 0xC17, 0, 0, 100, cat);
            Register<BoilingCauldronDeed>(1076267, 1156949, 0, 0x9CB9, 0, 100, cat);
            Register<SuitOfGoldArmorDeed>(1076265, 1156943, 0x3DAA, 0, 0, 100, cat);
            Register<BrokenBedDeed>(1076263, 1156945, 0, 0x9C8F, 0, 100, cat);
            Register<BrokenArmoireDeed>(1076262, 1156946, 0xC12, 0, 0, 100, cat);
            Register<BrokenVanityDeed>(1076260, 1156947, 0, 0x9C90, 0, 100, cat);
            Register<BrokenBookcaseDeed>(1076258, 1156948, 0xC14, 0, 0, 100, cat);
            Register<SacrificialAltarDeed>(1074818, 1156954, 0, 0x9C8E, 0, 100, cat);
            Register<HauntedMirrorDeed>(1074800, 1156953, 0x2A7B, 0, 0, 100, cat);
            Register<BrokenChestOfDrawersDeed>(1076261, 1156951, 0xC24, 0, 0, 100, cat);
            Register<StandingBrokenChairDeed>(1076259, 1156952, 0xC1B, 0, 0, 100, cat);
            Register<FountainOfLifeDeed>(1075197, 1156964, 0x2AC0, 0, 0, 100, cat);
            Register<TapestryOfSosaria>(1062917, 1156961, 0x234E, 0, 0, 100, cat);
            Register<HearthOfHomeFireDeed>(1062919, 1156958, 0, 0x9C97, 0, 100, cat);
            Register<CommemorativeRobe>(1157009, 1156908, 0x4B9D, 0, 0, 500, cat);
            Register<StoreSingingBall>(1041245, 1156907, 0, 0x9CB8, 0, 200, cat);
            Register<SecretChest>(1151583, 1156909, 0x9706, 0, 0, 500, cat);
            Register<HangingSwordsDeed>(1076272, 1156936, 0, 0x9C96, 0, 100, cat);
            Register<UnmadeBedDeed>(1076279, 1156935, 0, 0x9C9B, 0, 100, cat);
            Register<CurtainsDeed>(1076280, 1156934, 0, 0x9C93, 0, 100, cat);
            Register<TableWithOrangeClothDeed>(new TextDefinition[] { 1157012, 1157013 }, 1156933, 0x118E, 0, 0, 100, cat);
            Register<TableWithBlueClothDeed>(1076276, 1156932, 0x118C, 0, 0, 100, cat);
            Register<CherryBlossomTreeDeed>(1076268, 1156940, 0, 0x9C91, 0, 100, cat);
            Register<IronMaidenDeed>(1076288, 1156924, 0x1249, 0, 0, 100, cat);
            Register<SmallFishingNetDeed>(1076286, 1156923, 0x1EA3, 0, 0, 100, cat);
            Register<StoneStatueDeed>(1076284, 1156922, 0, 0x9C9A, 0, 100, cat);
            Register<WallTorchDeed>(1076282, 1156921, 0x3D98, 0, 0, 100, cat);
            Register<HouseLadderDeed>(1076287, 1156920, 0x2FDE, 0, 0, 100, cat);
            Register<LargeFishingNetDeed>(1076285, 1156919, 0x3D8E, 0, 0, 100, cat);
            Register<FountainDeed>(1076283, 1156918, 0, 0x9C94, 0, 100, cat);
            Register<ScarecrowDeed>(1076608, 1156917, 0x1E34, 0, 0, 100, cat);
            Register<HangingAxesDeed>(1076271, 1156937, 0, 0x9C95, 0, 100, cat);
            Register<AppleTreeDeed>(1076269, 1156938, 0, 0x9C8C, 0, 100, cat);
            Register<GuillotineDeed>(1024656, 1156941, 0x125E, 0, 0, 100, cat);
            Register<SuitOfSilverArmorDeed>(1076266, 1156942, 0x3D86, 0, 0, 100, cat);
            Register<PeachTreeDeed>(1076270, 1156939, 0, 0x9C98, 0, 100, cat);
            Register<CherryBlossomTrunkDeed>(1076784, 1156925, 0x26EE, 0, 0, 100, cat);
            Register<PeachTrunkDeed>(1076786, 1156926, 0xD9C, 0, 0, 100, cat);
            Register<BrokenFallenChairDeed>(1076264, 1156944, 0xC19, 0, 0, 100, cat);
            Register<TableWithRedClothDeed>(1076277, 1156930, 0x118E, 0, 0, 100, cat);
            Register<VanityDeed>(1074027, 1156931, 0, 0x9C9C, 0, 100, cat);
            Register<AppleTrunkDeed>(1076785, 1156927, 0xD98, 0, 0, 100, cat);
            Register<TableWithPurpleClothDeed>(new TextDefinition[] { 1157011, 1157013 }, 1156929, 0x118B, 0, 0, 100, cat);
            Register<WoodenCoffinDeed>(1076274, 1156928 , 0, 0x9C92, 0, 100, cat);
            Register<RaisedGardenDeed>(new TextDefinition[] { 1150359, 1156688 }, 1156680, 0, 0x9C8B, 0, 2000, cat, ConstructRaisedGarden);
            Register<HouseTeleporterTileBag>(new TextDefinition[] { 1156683, 1156826 }, 1156668, 0x40B9, 0, 1201, 1000, cat);
            Register<WoodworkersBenchDeed>(1026641, 1156670, 0x14F0, 0, 0, 600, cat);
            Register<LargeGlowingLadyBug>(1071400, 1156660, 0x2CFD, 0, 0, 200, cat);
            Register<FreshGreenLadyBug>(1071401, 1156661, 0x2D01, 0, 0, 200, cat);
            Register<WillowTreeDeed>(1071105, 1156658, 0x224A, 0, 0, 200, cat);
            Register<FallenLogDeed>(1071088, 1156649, 0, 0x9C88, 0, 200, cat);
            Register<LampPost2>(1071089, 1156650, 0xB22, 0, 0, 200, cat, ConstructLampPost);
            Register<HitchingPost>(1071090, 1156651, 0x14E7, 0, 0, 200, cat, ConstructHitchingPost);
            Register<AncestralGravestone>(1071096, 1156653, 0x1174, 0, 0, 200, cat);
            Register<WoodenBookcase>(1071102, 1156655, 0x0A9D, 0, 0, 200, cat);
            Register<SnowTreeDeed>(1071103, 1156656, 0, 0x9C8A, 0, 200, cat);
            Register<MapleTreeDeed>(1071104, 1156657, 0, 0x9C87, 0, 200, cat);

            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1157015 }, 1156916, 0, 0x9CB5, 0, 50, cat, ConstructMiniHouseDeed); // two story wood & plaster
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011317 }, 1156916, 0x22F5, 0, 0, 50, cat, ConstructMiniHouseDeed); // small stone tower
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011307 }, 1156916, 0x22E0, 0, 0, 50, cat, ConstructMiniHouseDeed); // wood and plaster house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011308 }, 1156916, 0x22E1, 0, 0, 50, cat, ConstructMiniHouseDeed); // thathed-roof cottage
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011312 }, 1156916, 0, 0x9CB2, 0, 50, cat, ConstructMiniHouseDeed); // Tower
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011313 }, 1156916, 0, 0x9CB1, 0, 50, cat, ConstructMiniHouseDeed); // Small stone keep
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011314 }, 1156916, 0, 0x9CB0, 0, 50, cat, ConstructMiniHouseDeed); // Castle
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011320 }, 1156916, 0x22F3, 0, 0, 50, cat, ConstructMiniHouseDeed); // sanstone house with patio
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011316 }, 1156916, 0, 0x9CB3, 0, 50, cat, ConstructMiniHouseDeed); // marble house with patio
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011319 }, 1156916, 0x2300, 0, 0, 50, cat, ConstructMiniHouseDeed); // two story villa
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1157014 }, 1156916, 0, 0x9CB6, 0, 50, cat, ConstructMiniHouseDeed); // two story stone & plaster
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011315 }, 1156916, 0, 0x9CB4, 0, 50, cat, ConstructMiniHouseDeed); // Large house with patio
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011309 }, 1156916, 0, 0x9CB7, 0, 50, cat, ConstructMiniHouseDeed); // brick house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011304 }, 1156916, 0x22C9, 0, 0, 50, cat, ConstructMiniHouseDeed); // field stone house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011306 }, 1156916, 0x22DF, 0, 0, 50, cat, ConstructMiniHouseDeed); // wooden house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011305 }, 1156916, 0x22DE, 0, 0, 50, cat, ConstructMiniHouseDeed); // small brick house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011303 }, 1156916, 0x22E1, 0, 0, 50, cat, ConstructMiniHouseDeed); // stone and plaster house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011318 }, 1156916, 0x22FB, 0, 0, 50, cat, ConstructMiniHouseDeed); // two-story log cabin
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011321 }, 1156916, 0x22F6, 0, 0, 50, cat, ConstructMiniHouseDeed); // small stone workshop
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011322 }, 1156916, 0x22F4, 0, 0, 50, cat, ConstructMiniHouseDeed); // small marble workshop

            // mounts
            cat = StoreCategory.Mounts;

            // Format: Type, NameCliloc, DescCliloc, ItemID, Hue, TooltipCliloc, Price, Category

            Register<EtherealHorse>(1006019, 0, 0x20DD, 0, 0, 100, cat);
            Register<EtherealLlama>(1006051, 0, 0x20F6, 0, 0, 100, cat);
            Register<EtherealOstard>(1006050, 0, 0x2135, 0, 0, 100, cat);
            Register<EtherealKirin>(1049746, 0, 0x25A0, 0, 0, 100, cat);
            Register<EtherealUnicorn>(1049745, 0, 0x25CE, 0, 0, 100, cat);
            Register<EtherealRidgeback>(1049747, 0, 0x2615, 0, 0, 100, cat);
            Register<EtherealBeetle>(1049748, 0, 0x260F, 0, 0, 100, cat);
            Register<EtherealSwampDragon>(1049749, 0, 0x2619, 0, 0, 100, cat);
            Register<RideablePolarBear>(1076159, 0, 0x20E1, 0, 0, 200, cat);
            Register<EtherealCuSidhe>(1080386, 0, 0x2D96, 0, 0, 200, cat);
            Register<EtherealReptalon>(1113908, 0, 0x2D95, 0, 0, 200, cat);
            Register<EtherealHiryu>(1113813, 0, 0x276A, 0, 0, 200, cat);
            Register<EtherealTiger>(1154589, 0, 0x9844, 0, 0, 200, cat);
            Register<EtherealSerpentineDragon>(1157995, 0, 0xA010, 0, 0, 200, cat);
            Register<EtherealWarBoar>(1159423, 0, 0xA554, 0, 0, 200, cat);
            Register<EtherealAncientHellHound>(1155723, 0, 0x3FFD, 0, 0, 200, cat);
            //Register<SkeletalCatStatue>(1158462, 1158738, 0xA138, 0, 0, 200, cat);
            //Register<EowmuStatue>(1158082, 1158433, 0xA0C0, 0, 0, 1000, cat);
            //Register<WindrunnerStatue>(1124685, 1157373, 0x9ED5, 0, 0, 1000, cat);
            
            Register<AlaskanMalamuteStatue>("Alaskan Malmute", 0, 0xA76C, 0, 0, 20000, cat);
            Register<GreatDaneStatue>("Great Dane", 0, 0xA76D, 1117, 0, 20000, cat);
            Register<NewfoundlandStatue>("Newfoundland", 0, 0xA76B, 0, 0, 20000, cat);
            Register<RottweilerStatue>("Rottweiler", 0, 0xA770, 0, 0, 20000, cat);
            Register<RussianTerrierStatue>("Russian Terrier", 0, 0xA76F, 0, 0, 20000, cat);
            Register<SaintBernardStatue>("Saint Bernard", 0, 0xA76E, 0, 0, 20000, cat);


            // dyes
            cat = StoreCategory.Dyes;

            // Format: Type, NameCliloc, DescCliloc, ItemID, Hue, TooltipCliloc, Price, Category

            Register<RewardBlackDyeTub>(1006008, 0, 0xFAB, 0, 0x001, 250, cat);
            Register<FurnitureDyeTub>(1006013, 0, 0xFAB, 0, 0, 250, cat);
            Register<SpecialDyeTub>(1006047, 0, 0xFAB, 0, 0, 250, cat);
            Register<LeatherDyeTub>(1006052, 0, 0xFAB, 0, 0, 500, cat);
            Register<RunebookDyeTub>(1049740, 0, 0xFAB, 0, 0, 500, cat);
            Register<SpellbookDyeTub>("Spellbook Dye Tub", 1024009, 0xFAB, 0, 0, 1000, cat);

            // Cub Store Dyes - 1000 SOV (5 uses each)
            Register<CubPhoenixRed>(1151651, 0, 0xEFF, 0, 1964, 1000, cat);
            Register<CubAuraOfAmber>(1152308, 0, 0xEFF, 0, 1967, 1000, cat);
            Register<CubDeepViolet>(1151912, 0, 0xEFF, 0, 1929, 1000, cat);
            Register<CubPolishedBronze>(1151909, 0, 0xEFF, 0, 1944, 1000, cat);
            Register<CubVibrantCrimson>(1153386, 0, 0xEFF, 0, 1964, 1000, cat);
            Register<CubLavender>(1151650, 0, 0xEFF, 0, 1951, 1000, cat);
            Register<CubGleamingFuchsia>(1152311, 0, 0xEFF, 0, 1930, 1000, cat);
            Register<CubDeepBlue>(1152348, 0, 0xEFF, 0, 1939, 1000, cat);
            Register<CubGlossyFuchsia>(1152347, 0, 0xEFF, 0, 1919, 1000, cat);
            Register<CubDarkVoid>(1154214, 0, 0xEFF, 0, 2068, 1000, cat);
            Register<CubMurkySeagreen>(1152309, 0, 0xEFF, 0, 1992, 1000, cat);
            Register<CubReflectiveShadow>(1153387, 0, 0xEFF, 0, 1910, 1000, cat);
            Register<CubLiquidSunshine>(1154213, 0, 0xEFF, 0, 1923, 1000, cat);
            Register<CubShadowyBlue>(1152310, 0, 0xEFF, 0, 1960, 1000, cat);
            Register<CubBlackAndGreen>(1151911, 0, 0xEFF, 0, 1979, 1000, cat);
            Register<CubGlossyBlue>(1151910, 0, 0xEFF, 0, 1916, 1000, cat);
            Register<CubHunterGreen>(1151649, 0, 0xEFF, 0, 1936, 1000, cat);
            Register<CubSlateBlue>(1151653, 0, 0xEFF, 0, 1983, 1000, cat);
            Register<CubMotherOfPearl>(1154120, 0, 0xEFF, 0, 2720, 1000, cat);
            Register<CubStarBlue>(1154121, 0, 0xEFF, 0, 2723, 1000, cat);
            Register<CubMurkyAmber>(1152350, 0, 0xEFF, 0, 1989, 1000, cat);
            Register<CubVibranSeagreen>(1152349, 0, 0xEFF, 0, 1970, 1000, cat);
            Register<CubVibrantOcher>(1154736, 0, 0xEFF, 0, 2725, 1000, cat);
            Register<CubMossyGreen>(1154731, 0, 0xEFF, 0, 2684, 1000, cat);
            Register<CubOliveGreen>(1154733, 0, 0xEFF, 0, 2709, 1000, cat);
            Register<CubMottledSunsetBlue>(1154734, 0, 0xEFF, 0, 2714, 1000, cat);
            Register<CubTyrianPurple>(1154735, 0, 0xEFF, 0, 2716, 1000, cat);
            Register<CubIntenseTeal>(1154732, 0, 0xEFF, 0, 2691, 1000, cat);

            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1157275 }, 1156671, 0, 0x9CBF, 0, 400, cat, ConstructHaochisPigment); // Heartwood Sienna
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1157274 }, 1156671, 0, 0x9CBD, 0, 400, cat, ConstructHaochisPigment); // Campion White
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1157273 }, 1156671, 0, 0x9CC2, 0, 400, cat, ConstructHaochisPigment); // Yewish Pine
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1157272 }, 1156671, 0, 0x9CC0, 0, 400, cat, ConstructHaochisPigment); // Minocian Fire
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1157269 }, 1156671, 0, 0x9CC1, 0, 400, cat, ConstructHaochisPigment); // Celtic Lime
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1071246 }, 1156671, 0, 0x9CAF, 0, 400, cat, ConstructHaochisPigment); // Ninja Black
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1018352 }, 1156671, 0, 0x9C83, 0, 400, cat, ConstructHaochisPigment); // Olive
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1071247 }, 1156671, 0, 0x9C7D, 0, 400, cat, ConstructHaochisPigment); // Dark Reddish Brown
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1071245 }, 1156671, 0, 0x9C85, 0, 400, cat, ConstructHaochisPigment); // Yellow
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1071244 }, 1156671, 0, 0x9C80, 0, 400, cat, ConstructHaochisPigment); // Pretty Pink
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1071248 }, 1156671, 0, 0x9C81, 0, 400, cat, ConstructHaochisPigment); // Midnight Blue
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1023856 }, 1156671, 0, 0x9C7F, 0, 400, cat, ConstructHaochisPigment); // Emerald
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1115467 }, 1156671, 0, 0x9C82, 0, 400, cat, ConstructHaochisPigment); // Smoky Gold
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1115468 }, 1156671, 0, 0x9C7E, 0, 400, cat, ConstructHaochisPigment); // Ghost's Grey
            Register<HaochisPigment>(new TextDefinition[] { 1071249, 1115471 }, 1156671, 0, 0x9C84, 0, 400, cat, ConstructHaochisPigment); // Ocean Blue 

            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070994 }, 1156906, 0, 0x9CA8, 0, 400, cat, ConstructPigments); // Nox Green
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079584 }, 1156906, 0, 0x9CAF, 0, 400, cat, ConstructPigments); // Midnight Coal
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070995 }, 1156906, 0, 0x9CA5, 0, 400, cat, ConstructPigments); // Rum Red
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079580 }, 1156906, 0, 0x9CA4, 0, 400, cat, ConstructPigments); // Coal
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079582 }, 1156906, 0, 0x9CA3, 0, 400, cat, ConstructPigments); // Storm Bronze
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079581 }, 1156906, 0, 0x9CA2, 0, 400, cat, ConstructPigments); // Faded Gold
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070988 }, 1156906, 0, 0x9CA1, 0, 400, cat, ConstructPigments); // Violet Courage Purple
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079585 }, 1156906, 0, 0x9CA2, 0, 400, cat, ConstructPigments); // Faded Bronze
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070996 }, 1156906, 0, 0x9C9F, 0, 400, cat, ConstructPigments); // Fire Orange
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079586 }, 1156906, 0, 0x9C9E, 0, 400, cat, ConstructPigments); // Faded Rose
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079583 }, 1156906, 0, 0x9CA7, 0, 400, cat, ConstructPigments); // Rose
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079587 }, 1156906, 0, 0x9CA9, 0, 400, cat, ConstructPigments); // Deep Rose
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070990 }, 1156906, 0, 0x9CAA, 0, 400, cat, ConstructPigments); // Luna White
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070992 }, 1156906, 0, 0x9CAF, 0, 400, cat, ConstructPigments); // Shadow Dancer Black
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070989 }, 1156906, 0, 0x9CAE, 0, 400, cat, ConstructPigments); // Invulnerability Blue
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070991 }, 1156906, 0, 0x9CAD, 0, 400, cat, ConstructPigments); // Dryad Green
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070993 }, 1156906, 0, 0x9CAC, 0, 400, cat, ConstructPigments); // Berserker Red
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079579 }, 1156906, 0, 0x9CAB, 0, 400, cat, ConstructPigments); // Faded Coal
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070987 }, 1156906, 0, 0x9C9D, 0, 400, cat, ConstructPigments); // Paragon Gold
                
                        
            Register<AbyssalHairDye>(1149822, 1156676, 0, 0x9C7A, 0, 400, cat);
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071439 }, 1156676, 0, 0x9C78, 0, 400, cat, ConstructHairDye); // Lemon Lime
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071470 }, 1156676, 0, 0x9C6D, 0, 400, cat, ConstructHairDye); // Yew Brown 
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071471 }, 1156676, 0, 0x9C6E, 0, 400, cat, ConstructHairDye); // Bloodwood Red
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071438 }, 1156676, 0, 0x9C6F, 0, 400, cat, ConstructHairDye); // Vivid Blue
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071469 }, 1156676, 0, 0x9C71, 0, 400, cat, ConstructHairDye); // Ash Blonde
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071472 }, 1156676, 0, 0x9C72, 0, 400, cat, ConstructHairDye); // Heartwood Green
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071472 }, 1156676, 0, 0x9C85, 0, 400, cat, ConstructHairDye); // Oak Blonde
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071474 }, 1156676, 0, 0x9C70, 0, 400, cat, ConstructHairDye); // Sacred White
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071473 }, 1156676, 0, 0x9C73, 0, 400, cat, ConstructHairDye); // Frostwood Ice Green
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071440 }, 1156676, 0, 0x9C76, 0, 400, cat, ConstructHairDye); // Fiery Blonde
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071437 }, 1156676, 0, 0x9C77, 0, 400, cat, ConstructHairDye); // Bitter Brown
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071442 }, 1156676, 0, 0x9C74, 0, 400, cat, ConstructHairDye); // Gnaw's Twisted Blue
            Register<SpecialHairDye>(new TextDefinition[] { 1071387, 1071441 }, 1156676, 0, 0x9C75, 0, 400, cat, ConstructHairDye); // Dusk Black

            // pet dyes
            cat = StoreCategory.PetDyes;
                                    
            // 3. Permanent Pet Dyes - 1000 SOV
            Register<PetCubIntenseTeal>(1154732, 1024009, 0xFAB, 0, 2691, 1000, cat);
            Register<PetCubTyrianPurple>(1154735, 1024009, 0xFAB, 0, 2716, 1000, cat);
            Register<PetCubMottledSunsetBlue>(1154734, 1024009, 0xFAB, 0, 2714, 1000, cat);
            Register<PetCubMossyGreen>(1154731, 1024009, 0xFAB, 0, 2684, 1000, cat);
            Register<PetCubVibrantOcher>(1154736, 1024009, 0xFAB, 0, 2725, 1000, cat);
            Register<PetCubOliveGreen>(1154733, 1024009, 0xFAB, 0, 2709, 1000, cat);
            Register<PetCubPolishedBronze>(1151909, 1024009, 0xFAB, 0, 1944, 1000, cat);
            Register<PetCubGlossyBlue>(1151910, 1024009, 0xFAB, 0, 1916, 1000, cat);
            Register<PetCubBlackAndGreen>(1151911, 1024009, 0xFAB, 0, 1979, 1000, cat);
            Register<PetCubDeepViolet>(1151912, 1024009, 0xFAB, 0, 1929, 1000, cat);
            Register<PetCubAuraOfAmber>(1152308, 1024009, 0xFAB, 0, 1967, 1000, cat);
            Register<PetCubMurkySeagreen>(1152309, 1024009, 0xFAB, 0, 1992, 1000, cat);
            Register<PetCubShadowyBlue>(1152310, 1024009, 0xFAB, 0, 1960, 1000, cat);
            Register<PetCubGleamingFuchsia>(1152311, 1024009, 0xFAB, 0, 1930, 1000, cat);
            Register<PetCubGlossyFuchsia>(1152347, 1024009, 0xFAB, 0, 1919, 1000, cat);
            Register<PetCubDeepBlue>(1152348, 1024009, 0xFAB, 0, 1939, 1000, cat);
            Register<PetCubVibranSeagreen>(1152349, 1024009, 0xFAB, 0, 1970, 1000, cat);
            Register<PetCubMurkyAmber>(1152350, 1024009, 0xFAB, 0, 1989, 1000, cat);
            Register<PetCubVibrantCrimson>(1153386, 1024009, 0xFAB, 0, 1964, 1000, cat);
            Register<PetCubReflectiveShadow>(1153387, 1024009, 0xFAB, 0, 1910, 1000, cat);
            Register<PetCubStarBlue>(1154121, 1024009, 0xFAB, 0, 2723, 1000, cat);
            Register<PetCubMotherOfPearl>(1154120, 1024009, 0xFAB, 0, 2720, 1000, cat);
            Register<PetCubLiquidSunshine>(1154213, 1024009, 0xFAB, 0, 1923, 1000, cat);
            Register<PetCubDarkVoid>(1154214, 1024009, 0xFAB, 0, 2068, 1000, cat);
            Register<PetCubPhoenixRed>(1151651, 1024009, 0xFAB, 0, 1964, 1000, cat);
            Register<PetCubLavender>(1151650, 1024009, 0xFAB, 0, 1951, 1000, cat);
            Register<PetCubHunterGreen>(1151649, 1024009, 0xFAB, 0, 1936, 1000, cat);
            Register<PetCubSlateBlue>(1151653, 1024009, 0xFAB, 0, 1983, 1000, cat);

            // Sample Pet Dyes - 5 SOV
            Register<SamplePetCubIntenseTeal>(1154732, 1024009, 0xEFB, 0, 2691, 5, cat);
            Register<SamplePetCubTyrianPurple>(1154735, 1024009, 0xEFB, 0, 2716, 5, cat);
            Register<SamplePetCubMottledSunsetBlue>(1154734, 1024009, 0xEFB, 0, 2714, 5, cat);
            Register<SamplePetCubMossyGreen>(1154731, 1024009, 0xEFB, 0, 2684, 5, cat);
            Register<SamplePetCubVibrantOcher>(1154736, 1024009, 0xEFB, 0, 2725, 5, cat);
            Register<SamplePetCubOliveGreen>(1154733, 1024009, 0xEFB, 0, 2709, 5, cat);
            Register<SamplePetCubPolishedBronze>(1151909, 1024009, 0xEFB, 0, 1944, 5, cat);
            Register<SamplePetCubGlossyBlue>(1151910, 1024009, 0xEFB, 0, 1916, 5, cat);
            Register<SamplePetCubBlackAndGreen>(1151911, 1024009, 0xEFB, 0, 1979, 5, cat);
            Register<SamplePetCubDeepViolet>(1151912, 1024009, 0xEFB, 0, 1929, 5, cat);
            Register<SamplePetCubAuraOfAmber>(1152308, 1024009, 0xEFB, 0, 1967, 5, cat);
            Register<SamplePetCubMurkySeagreen>(1152309, 1024009, 0xEFB, 0, 1992, 5, cat);
            Register<SamplePetCubShadowyBlue>(1152310, 1024009, 0xEFB, 0, 1960, 5, cat);
            Register<SamplePetCubGleamingFuchsia>(1152311, 1024009, 0xEFB, 0, 1930, 5, cat);
            Register<SamplePetCubGlossyFuchsia>(1152347, 1024009, 0xEFB, 0, 1919, 5, cat);
            Register<SamplePetCubDeepBlue>(1152348, 1024009, 0xEFB, 0, 1939, 5, cat);
            Register<SamplePetCubVibranSeagreen>(1152349, 1024009, 0xEFB, 0, 1970, 5, cat);
            Register<SamplePetCubMurkyAmber>(1152350, 1024009, 0xEFB, 0, 1989, 5, cat);
            Register<SamplePetCubVibrantCrimson>(1153386, 1024009, 0xEFB, 0, 1964, 5, cat);
            Register<SamplePetCubReflectiveShadow>(1153387, 1024009, 0xEFB, 0, 1910, 5, cat);
            Register<SamplePetCubStarBlue>(1154121, 1024009, 0xEFB, 0, 2723, 5, cat);
            Register<SamplePetCubMotherOfPearl>(1154120, 1024009, 0xEFB, 0, 2720, 5, cat);
            Register<SamplePetCubLiquidSunshine>(1154213, 1024009, 0xEFB, 0, 1923, 5, cat);
            Register<SamplePetCubDarkVoid>(1154214, 1024009, 0xEFB, 0, 2068, 5, cat);
            Register<SamplePetCubPhoenixRed>(1151651, 1024009, 0xEFB, 0, 1964, 5, cat);
            Register<SamplePetCubLavender>(1151650, 1024009, 0xEFB, 0, 1951, 5, cat);
            Register<SamplePetCubHunterGreen>(1151649, 1024009, 0xEFB, 0, 1936, 5, cat);
            Register<SamplePetCubSlateBlue>(1151653, 1024009, 0xEFB, 0, 1983, 5, cat);
        }

        public static void Register<T>(TextDefinition name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null) where T : Item
        {
            Register(typeof(T), name, tooltip, itemID, gumpID, hue, cost, cat, constructor);
        }

        public static void Register(Type itemType, TextDefinition name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null)
        {
            Register(new StoreEntry(itemType, name, tooltip, itemID, gumpID, hue, cost, cat, constructor));
        }

        public static void Register<T>(TextDefinition[] name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null) where T : Item
        {
            Register(typeof(T), name, tooltip, itemID, gumpID, hue, cost, cat, constructor);
        }

        public static void Register(Type itemType, TextDefinition[] name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null)
        {
            Register(new StoreEntry(itemType, name, tooltip, itemID, gumpID, hue, cost, cat, constructor));
        }

        public static void Register(StoreEntry entry)
        {
            Entries.Add(entry);
        }

        public static bool CanSearch(Mobile m)
        {
            return m != null && m.Region.GetLogoutDelay(m) <= TimeSpan.Zero;
        }

        public static void UOStoreRequest(NetState state, PacketReader pvSrc)
        {
            OpenStore(state.Mobile as PlayerMobile);
        }

        public static void OpenStore(PlayerMobile user)
        {
            if (user == null || user.NetState == null)
            {
                return;
            }

            if (!Enabled || (Configuration.Expansion != Expansion.None && Core.Expansion < Configuration.Expansion))
            {
                // The promo code redemption system is currently unavailable. Please try again later.
                user.SendLocalizedMessage(1062904);
                return;
            }

            if (Configuration.CurrencyImpl == CurrencyType.None)
            {
                // The promo code redemption system is currently unavailable. Please try again later.
                user.SendLocalizedMessage(1062904);
                return;
            }

            if (!user.NetState.UltimaStore)
            {
                user.SendMessage("You must update Ultima Online in order to use the in game store.");
                return;
            }

            if (user.AccessLevel < AccessLevel.Counselor && !CanSearch(user))
            {
                // Before using the in game store, you must be in a safe log-out location
                // such as an inn or a house which has you on its Owner, Co-owner, or Friends list.
                user.SendLocalizedMessage(1156586);
                return;
            }

            if (!user.HasGump(typeof(UltimaStoreGump)))
            {
                BaseGump.SendGump(new UltimaStoreGump(user));
            }
        }

        #region Constructors
        public static Item ConstructHairDye(Mobile m, StoreEntry entry)
        {
            var info = NaturalHairDye.Table.FirstOrDefault(x => x.Localization == entry.Name[1].Number);

            if(info != null)
            {
                return new NaturalHairDye(info.Type);
            }

            return null;
        }

        public static Item ConstructHaochisPigment(Mobile m, StoreEntry entry)
        {
            var info = HaochisPigment.Table.FirstOrDefault(x => x.Localization == entry.Name[1].Number);

            if (info != null)
            {
                return new HaochisPigment(info.Type, 50);
            }

            return null;
        }

        public static Item ConstructPigments(Mobile m, StoreEntry entry)
        {
            PigmentType type = PigmentType.None;

            for (int i = 0; i < PigmentsOfTokuno.Table.Length; i++)
            {
                if (PigmentsOfTokuno.Table[i][1] == entry.Name[1].Number)
                {
                    type = (PigmentType)i;
                    break;
                }
            }

            if (type != PigmentType.None)
            {
                return new PigmentsOfTokuno(type, 50);
            }

            return null;
        }

        public static Item ConstructEarrings(Mobile m, StoreEntry entry)
        {
            AosElementAttribute ele = AosElementAttribute.Physical;

            switch (entry.Name[0].Number)
            {
                case 1071092: ele = AosElementAttribute.Fire; break;
                case 1071093: ele = AosElementAttribute.Cold; break;
                case 1071094: ele = AosElementAttribute.Poison; break;
                case 1071095: ele = AosElementAttribute.Energy; break;
            }

            return new EarringsOfProtection(ele);
        }

        public static Item ConstructRobe(Mobile m, StoreEntry entry)
        {
            return new HoodedBritanniaRobe(entry.ItemID);
        }

        public static Item ConstructMiniHouseDeed(Mobile m, StoreEntry entry)
        {
            int label = entry.Name[1].Number;

            switch (label)
            {
                default:
                    for (int i = 0; i < MiniHouseInfo.Info.Length; i++)
                    {
                        if (MiniHouseInfo.Info[i].LabelNumber == entry.Name[1].Number)
                        {
                            var type = (MiniHouseType)i;

                            return new MiniHouseDeed(type);
                        }
                    }
                    return null;
                case 1157015: return new MiniHouseDeed(MiniHouseType.TwoStoryWoodAndPlaster);
                case 1157014: return new MiniHouseDeed(MiniHouseType.TwoStoryStoneAndPlaster);
            }
        }

        public static Item ConstructRaisedGarden(Mobile m, StoreEntry entry)
        {
            var bag = new Bag();

            bag.DropItem(new RaisedGardenDeed());
            bag.DropItem(new RaisedGardenDeed());
            bag.DropItem(new RaisedGardenDeed());

            return bag;
        }

        public static Item ConstructLampPost(Mobile m, StoreEntry entry)
        {
            var item = new LampPost2
            {
                Movable = true,
                LootType = LootType.Blessed
            };

            return item;
        }

        public static Item ConstructForgedMetal(Mobile m, StoreEntry entry)
        {
            switch (entry.Name[1].Number)
            {
                case 1156686: return new ForgedMetalOfArtifacts(10);
                case 1156687: return new ForgedMetalOfArtifacts(5);
            }

            return null;
        }

        public static Item ConstructSoulstone(Mobile m, StoreEntry entry)
        {
            switch (entry.Name[0].Number)
            {
                case 1078835: return new SoulstoneToken(SoulstoneType.Blue);
                case 1078834: return new SoulstoneToken(SoulstoneType.Green);
                case 1158404: return new SoulstoneToken(SoulstoneType.Violet);
            }

            return null;
        }

        public static Item ConstructMerchantsTrinket(Mobile m, StoreEntry entry)
        {
            switch(entry.Name[0].Number)
            {
                case 1156827: return new MerchantsTrinket(false);
                case 1156828: return new MerchantsTrinket(true);
            }

            return null;
        }

        public static Item ConstructBOBCoverOne(Mobile m, StoreEntry entry)
        {
            return new BagOfBulkOrderCovers(12, 25);
        }

        public static Item ConstructBOBCoverTwo(Mobile m, StoreEntry entry)
        {
            return new BagOfBulkOrderCovers(1, 11);
        }

        public static Item ConstructHitchingPost(Mobile m, StoreEntry entry)
        {
            return new HitchingPost(false);
        }
        #endregion

        public static void AddPendingItem(Mobile m, Item item)
        {
            if (!PendingItems.TryGetValue(m, out List<Item> list))
            {
                PendingItems[m] = list = new List<Item>();
            }

            if (!list.Contains(item))
            {
                list.Add(item);
            }

            UltimaStoreContainer.DropItem(item);
        }

        public static bool HasPendingItem(PlayerMobile pm)
        {
            return PendingItems.ContainsKey(pm);
        }

        public static void CheckPendingItem(Mobile m)
        {
            if (PendingItems.TryGetValue(m, out List<Item> list))
            {
                var index = list.Count;

                while (--index >= 0)
                {
                    if (index >= list.Count)
                    {
                        continue;
                    }

                    var item = list[index];

                    if (item != null)
                    {
                        if (m.Backpack != null && m.Alive && m.Backpack.TryDropItem(m, item, false))
                        {
                            if (item is IPromotionalToken && ((IPromotionalToken)item).ItemName != null)
                            {
                                // A token has been placed in your backpack. Double-click it to redeem your ~1_PROMO~.
                                m.SendLocalizedMessage(1075248, ((IPromotionalToken)item).ItemName.ToString());
                            }
                            else if (item.LabelNumber > 0 || item.Name != null)
                            {
                                var name = item.LabelNumber > 0 ? ("#" + item.LabelNumber) : item.Name;

                                // Your purchase of ~1_ITEM~ has been placed in your backpack.
                                m.SendLocalizedMessage(1156844, name);
                            }
                            else
                            {
                                // Your purchased item has been placed in your backpack.
                                m.SendLocalizedMessage(1156843);
                            }

                            list.RemoveAt(index);
                        }
                    }
                    else
                    {
                        list.RemoveAt(index);
                    }
                }

                if (list.Count == 0 && PendingItems.Remove(m))
                {
                    list.TrimExcess();
                }
            }
        }

        public static List<StoreEntry> GetSortedList(string searchString)
        {
            var list = new List<StoreEntry>();

            list.AddRange(Entries.Where(e => Insensitive.Contains(GetStringName(e.Name), searchString)));

            return list;
        }

        public static string GetStringName(TextDefinition[] text)
        {
            var str = string.Empty;

            foreach (var td in text)
            {
                if (td.Number > 0 && VendorSearch.StringList != null)
                {
                    str += String.Format("{0} ", VendorSearch.StringList.GetString(td.Number));
                }
                else if (!String.IsNullOrWhiteSpace(td.String))
                {
                    str += String.Format("{0} ", td.String);
                }
            }

            return str;
        }

        public static string GetStringName(TextDefinition text)
        {
            var str = text.String;

            if (text.Number > 0 && VendorSearch.StringList != null)
            {
                str = VendorSearch.StringList.GetString(text.Number);
            }

            return str ?? String.Empty;
        }

        public static List<StoreEntry> GetList(StoreCategory cat)
        {
            return Entries.Where(e => e.Category == cat).ToList();
        }

        public static void SortList(List<StoreEntry> list, SortBy sort)
        {
            switch (sort)
            {
                case SortBy.Name: 
                        list.Sort((a, b) => String.CompareOrdinal(GetStringName(a.Name), GetStringName(b.Name)));
                    break;
                case SortBy.PriceLower:
                        list.Sort((a, b) => a.Price.CompareTo(b.Price));
                    break;
                case SortBy.PriceHigher:
                        list.Sort((a, b) => b.Price.CompareTo(a.Price));
                    break;
                case SortBy.Newest:
                    break;
                case SortBy.Oldest:
                        list.Reverse();
                    break;
            }
        }

        public static int CartCount(Mobile m)
        {
            var profile = GetProfile(m, false);

            if (profile != null)
            {
                return profile.Cart.Count;
            }

            return 0;
        }

        public static int GetSubTotal(Dictionary<StoreEntry, int> cart)
        {
            if (cart == null || cart.Count == 0)
            {
                return 0;
            }

            var sub = 0.0;

            foreach (var kvp in cart)
            {
                sub += kvp.Key.Cost * kvp.Value;
            }

            return (int)sub;
        }

        public static int GetCurrency(Mobile m, bool sendMessage = false)
        {
            switch (Configuration.CurrencyImpl)
            {
                case CurrencyType.Sovereigns:
                {
                    if (m is PlayerMobile)
                    {
                        return ((PlayerMobile)m).AccountSovereigns;
                    }
                }
                    break;
                case CurrencyType.Gold:
                    return Banker.GetBalance(m);
                case CurrencyType.PointsSystem:
                {
                    var sys = PointsSystem.GetSystemInstance(Configuration.PointsImpl);

                    if (sys != null)
                    {
                        return (int)Math.Min(Int32.MaxValue, sys.GetPoints(m));
                    }
                }
                    break;
                case CurrencyType.Custom:
                    return Configuration.GetCustomCurrency(m);
            }

            return 0;
        }

        public static void TryPurchase(Mobile m)
        {
            var cart = GetCart(m);
            var total = GetSubTotal(cart);
            
            if (cart == null || cart.Count == 0 || total == 0)
            {
                // Purchase failed due to your cart being empty.
                m.SendLocalizedMessage(1156842); 
            }
            else if (total > GetCurrency(m, true))
            {
                if (m is PlayerMobile)
                {
                    BaseGump.SendGump(new NoFundsGump((PlayerMobile)m));
                }
            }
            else
            {
                var subtotal = 0;
                var fail = false;

                var remove = new List<StoreEntry>();

                foreach (var entry in cart)
                {
                    for (var i = 0; i < entry.Value; i++)
                    {
                        if (!entry.Key.Construct(m))
                        {
                            fail = true;

                            try
                            {
                                using (var op = File.AppendText("UltimaStoreError.log"))
                                {
                                    op.WriteLine("Bad Constructor: {0}", entry.Key.ItemType.Name);

                                    Utility.WriteConsoleColor(ConsoleColor.Red, "[Ultima Store]: Bad Constructor: {0}", entry.Key.ItemType.Name);
                                }
                            }
                            catch
                            { }
                        }
                        else
                        {
                            remove.Add(entry.Key);

                            subtotal += entry.Key.Cost;
                        }
                    }
                }

                if (subtotal > 0)
                {
                    DeductCurrency(m, subtotal);
                }

                var profile = GetProfile(m);

                foreach (var entry in remove)
                {
                    profile.RemoveFromCart(entry);
                }

                if (fail)
                {
                    // Failed to process one of your items. Please check your cart and try again.
                    m.SendLocalizedMessage(1156853); 
                }
            }
        }

        /// <summary>
        /// Should have already passed GetCurrency
        /// </summary>
        /// <param name="m"></param>
        /// <param name="amount"></param>
        public static int DeductCurrency(Mobile m, int amount)
        {
            switch (Configuration.CurrencyImpl)
            {
                case CurrencyType.Sovereigns:
                {
                    if (m is PlayerMobile && ((PlayerMobile)m).WithdrawSovereigns(amount))
                    {
                        return amount;
                    }
                }
                    break;
                case CurrencyType.Gold:
                {
                    if (Banker.Withdraw(m, amount, true))
                    {
                        return amount;
                    }
                }
                    break;
                case CurrencyType.PointsSystem:
                {
                    var sys = PointsSystem.GetSystemInstance(Configuration.PointsImpl);

                    if (sys != null && sys.DeductPoints(m, amount, true))
                    {
                        return amount;
                    }
                }
                    break;
                case CurrencyType.Custom:
                    return Configuration.DeductCustomCurrecy(m, amount);
            }

            return 0;
        }

        #region Player Persistence
        public static Dictionary<Mobile, PlayerProfile> PlayerProfiles { get; private set; }

        public static PlayerProfile GetProfile(Mobile m, bool create = true)
        {
            PlayerProfile profile;

            if ((!PlayerProfiles.TryGetValue(m, out profile) || profile == null) && create)
            {
                PlayerProfiles[m] = profile = new PlayerProfile(m);
            }

            return profile;
        }

        public static Dictionary<StoreEntry, int> GetCart(Mobile m)
        {
            var profile = GetProfile(m, false);

            if (profile != null)
            {
                return profile.Cart;
            }

            return null;
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(FilePath, Serialize);
        }

        public static void OnLoad()
        {
            Persistence.Deserialize(FilePath, Deserialize);
        }

        private static void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(_UltimaStoreContainer);

            writer.Write(PendingItems.Count);

            foreach (var kvp in PendingItems)
            {
                writer.Write(kvp.Key);
                writer.WriteItemList(kvp.Value, true);
            }

            writer.Write(PlayerProfiles.Count);

            foreach (var pe in PlayerProfiles)
            {
                pe.Value.Serialize(writer);
            }
        }

        private static void Deserialize(GenericReader reader)
        {
            reader.ReadInt();

            _UltimaStoreContainer = reader.ReadItem<UltimaStoreContainer>();

            var count = reader.ReadInt();

            for (var i = 0; i < count; i++)
            {
                var m = reader.ReadMobile();
                var list = reader.ReadStrongItemList<Item>();

                if (m != null && list.Count > 0)
                {
                    PendingItems[m] = list;
                }
            }

            count = reader.ReadInt();

            for (var i = 0; i < count; i++)
            {
                var pe = new PlayerProfile(reader);

                if (pe.Player != null)
                {
                    PlayerProfiles[pe.Player] = pe;
                }
            }
        }
        #endregion
    }

    [DeleteConfirm("This is the Ultima Store item display container. You should not delete this.")]
    public sealed class UltimaStoreContainer : Container
    {
        private static readonly List<Item> _DisplayItems = new List<Item>();

        public override bool Decays { get { return false; } }

        public override string DefaultName { get { return "Ultima Store Display Container"; } }

        public UltimaStoreContainer()
            : base(0) // No Draw
        {
            Movable = false;
            Visible = false;

            Internalize();
        }

        public UltimaStoreContainer(Serial serial)
            : base(serial)
        { }

        public void AddDisplayItem(Item item)
        {
            if (item == null)
            {
                return;
            }

            if (!_DisplayItems.Contains(item))
            {
                _DisplayItems.Add(item);
            }

            DropItem(item);
        }

        public Item FindDisplayItem(Type t)
        {
            var item = GetDisplayItem(t);

            if (item == null)
            {
                item = Loot.Construct(t);

                if (item != null)
                {
                    AddDisplayItem(item);
                }
            }

            return item;
        }

        public Item GetDisplayItem(Type t)
        {
            return _DisplayItems.FirstOrDefault(x => x.GetType() == t);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.WriteItemList(_DisplayItems, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();

            var list = reader.ReadStrongItemList();

            if (list.Count > 0)
            {
                Timer.DelayCall(o => o.ForEach(AddDisplayItem), list);
            }
        }
    }
}
