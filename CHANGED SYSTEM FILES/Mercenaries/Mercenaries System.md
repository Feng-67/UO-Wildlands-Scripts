# **Custom Mercenaries System for ServUO**

A complete, feature-packed mercenary hiring framework for **ServUO**. This system allows players to hire high-tier NPC mercenaries directly from their bank balance. Mercenaries possess unique death-prevention mechanics, active tactical kiting, passive health/mana/stamina regeneration, and automatic ammunition collection.

Mercenaries are a modification of hireables normally found at Jhelom Duelling Pits on both facets. Four of the standard hirables have been enhanced and given features. Enhanced versions are labelled as Legendary on their tooltip title.


## **Key Features**

* **Bank Draft Hiring:** Hire NPCs via context menu or speech keywords (hire) using gold directly from the player's bank account.  
* **Death Override & Healer Recovery:** Mercenaries never drop corpses, lose gear, or turn into ghosts upon taking fatal damage. They temporarily leave the map to recovery, automatically returning to their master fully healed after a 60-second cooldown (may need to increase cooldown longer for balance - 2 to 5 minutes).
* **Passive Regeneration:** Ticking passive stat recovery replenishes Hits, Stamina, and Mana every 2 seconds.  
* **Smart Ranged Kiting:** Ranged and spellcasting mercenaries dynamically step back from melee attackers to maintain distance (players may prefer to avoid moving around during combat).
* **Auto-Ammo Collection:** Ranged mercenaries actively scan the ground and collect nearby arrows or bolts into their backpack (pointless feature?).
* **Dynamic AI Switching:** Equipping or unequipping ranged weapons automatically toggles between AI\_Archer and AI\_Melee routines (pointless feature?).
* **Bonded on Hire:** Bonding allows players to recall with the Mercenary, transferred to another player, shrunk if custom shrinking is available on the server, or traded if shrunk.

## **Core Mechanics & Configuration**

### **1\. Base Hiring & Costs**

Mercenaries consume **4 Control Slots** and cost **1,000,000 Gold** (withdrawn directly from the bank upon hiring).

### **2\. Death & Recovery Routine**

When a mercenary reaches 0 HP:

1. The death event is canceled (OnBeforeDeath returns false).  
2. Combat state and status effects (poisons) are cleared.  
3. The mercenary plays a visual/audio effect and moves to Map.Internal.  
4. After **60 seconds**, the mercenary teleports back to the owner's exact location at full health.  
5. **Server Restart Safe:** Recovery timers are serialized via \_recoveryStartedUtc and recalculate remaining time upon deserialization.

### **3\. Regeneration & Tactical AI**

* **Regeneration:** Restores \+20 HP, Stamina, and Mana every 2.0 seconds.  
* **Kiting:** Checks surrounding targets every 0.5 seconds; if an attacker comes within 2 tiles, the mercenary backs away.  
* **Ammo Pickup:** Automatically collects Arrow and Bolt items within 2 tiles into the backpack every 5 seconds.


## **Included Classes**

| Class | Title | Primary Skills (120 GM) | Default Equipment / Notes |
| :---- | :---- | :---- | :---- |
| **BaseHire**  | *N/A (Base Class)*  | *N/A*  | Core logic, serialization, recovery timers, and automations. |
| **HireFighter**  | *Legendary Swordsman*  | Swords, Tactics, Parry, Bushido, Chivalry, Anatomy | Random swords/axes, shield, chain/ring/leather armor, Book of Chivalry. |
| **HireMage**  | *Legendary Mage*  | Magery, EvalInt, Meditation, MagicResist, Wrestling, Inscribe | Full Spellbook, Robe, Wizard's Hat. Uses AI\_Mage. |
| **HirePaladin**  | *Legendary Paladin*  | Swords, Tactics, Parry, Chivalry, Anatomy | Full Plate Armor, Viking Sword, Metal Kite Shield, 20k Tithing. |
| **HireRangerArcher**  | *Legendary Archer*  | Archery, Tactics, Anatomy, MagicResist, Chivalry | Bow, Quiver, Leather/Studded Armor. Uses AI\_Archer. |

## **Installation**

1. Copy the code files into your ModernUO custom scripts directory (e.g., Scripts/Mobiles/Custom/Mercenaries/).  
2. Compile the server solution.  
3. Spawn any mercenary in-game using the \[add command:  
   * \[add HireFighter

   * \[add HireMage

   * \[add HirePaladin

   * \[add HireRangerArcher

4. Say **"hire"** near them for the hire cost. Right-click the NPC and select **Hire** with sufficient funds to hire the Mercenary. 