================================================================================

&#x20; **UO WILDLANDS - TOWN HOUSE SYSTEM**

&#x20; **Setup Guide \& Feature Reference**

================================================================================



FILES

\-----

&#x20; TownHouseController.cs  — Core system (controller, region, utilities, GM tools)

&#x20; TownHouseSignGump.cs    — Player-facing sign gump (purchase, access lists, etc.)

&#x20; TownHouseSystem.cs      — Server registry and GM index gump



All three files go in the same folder, e.g.:

&#x20; Scripts/Custom Commands/Townhouses/



\--------------------------------------------------------------------------------

**GM SETUP — HOW TO CREATE A TOWN HOUSE**

\--------------------------------------------------------------------------------



1\. **PLACE THE SIGN**

&#x20;  Use \[add TownHouseController to place a town house sign at the desired

&#x20;  location. The sign acts as the control point for the entire property.



**The sign must be "set movable false" either as a GM command or via \[props to remain on server restart. It can be relocated with \[move command.**



2\. **OPEN THE SIGN GUMP**

&#x20;  Double-click the sign (or use \[TownHouses to find it in the index).

&#x20;  The GM Configuration panel appears on the right side of the gump.



3\. **SET THE BOUNDS**

&#x20;  Click "Set Bounds" — you will be prompted to target two corners of the

&#x20;  property. Target the floor at opposite corners (e.g. front-left and

&#x20;  back-right). The system will calculate the rectangle automatically.



4\. **HIGHLIGHT BOUNDS (OPTIONAL)**

&#x20;  Click "Highlight Bounds" to place temporary markers showing the exact

&#x20;  region. Markers auto-delete after 30 seconds. Useful for verifying the

&#x20;  bounds before opening the property for sale.



5\. **CONFIGURE SETTINGS**

&#x20;  In the GM Configuration panel you can set:

&#x20;    - Price:    Purchase price in gold (drawn from bank)

&#x20;    - Locks:    Maximum number of lockdowns allowed (default 1500)

&#x20;    - Z-Range:  Vertical range of the region (min Z to max Z)

&#x20;  Click "APPLY SETTINGS" to save.



6\. **SET STATE**

&#x20;  Click "Cycle State" to toggle between Purchasable and Transferable.

&#x20;  Set it to Purchasable so players can buy it from the sign.



7\. **WIPE POLICY (OPTIONAL)**

&#x20;  Click "Wipe Policy" to toggle between:

&#x20;    - LeaveAsIs:          Items remain when the house changes hands (default)

&#x20;    - WipeItemsInBounds:  All items inside are deleted on eviction/abandonment



8\. **DONE**

&#x20;  The sign will display "For Sale: X gp" when single-clicked.

&#x20;  Players can now double-click the sign to purchase the property.



\--------------------------------------------------------------------------------

**GM MANAGEMENT — \[TownHouses COMMAND**

\--------------------------------------------------------------------------------



Type \[TownHouses to open the Town House Global Index Gump.

This shows all registered town houses with the following columns:



&#x20; MAP      — Which facet the house is on

&#x20; LOCATION — X, Y, Z coordinates of the sign

&#x20; STATE    — Purchasable / Transferable

&#x20; OWNER    — Current owner name, or "- Unowned -"



Actions available per row:

&#x20; Go     — Teleports you to the sign location

&#x20; Open   — Opens the sign gump for that house (same as double-clicking the sign)

&#x20; Evict  — Immediately removes the owner, resets the house to Purchasable,

&#x20;           and clears all access lists. Items remain in place but are

&#x20;           immovable until the new owner re-secures or re-lockdowns them.



Click Refresh to update the list.



\--------------------------------------------------------------------------------

**PLAYER FEATURES**

\--------------------------------------------------------------------------------



**PURCHASING**

&#x20; Players double-click the sign when State = Purchasable.

&#x20; Gold is withdrawn from their bank account.

&#x20; Ownership is assigned immediately.



**SIGN GUMP — MAIN TAB**

&#x20; Shows property status, owner name, lockdown count, secure count,

&#x20; total area in tiles, and available actions based on access level.



**ITEM SECURITY** (Friends and above)

&#x20; Lockdown Item   — Prevents an item from being moved (up to the lockdown limit)

&#x20; Secure Container — Locks a container; only Friends/CoOwners/Owner can open it.

&#x20;                    Container displays "Secured" in its tooltip.

&#x20; Release Item    — Returns an item/container to movable state.

&#x20;                   Requires Co-Owner access or above.



**ACCESS LISTS** (Co-Owners and above)

&#x20; Friends   — Can lockdown items and open secured containers.

&#x20; Co-Owners — All friend rights plus can manage friends, bans, and

&#x20;              release lockdowns/secures.

&#x20;             Only the Owner can add or remove Co-Owners.

&#x20; Bans      — Banned players cannot enter the house region.



&#x20; All lists support pagination. Click the button next to each list name

&#x20; to open the management screen. Click the red X button next to a name

&#x20; to remove them.



**PLAYER SALE** (Owner only)

&#x20; The owner can set a sale price via "Sell House (Set Price)".

&#x20; Once set, any player can purchase it directly from the sign.

&#x20; Gold goes to the seller's bank. Cancel Sale removes the listing.



**TRANSFER** (Owner only — Transferable state only)

&#x20; Targets another player and transfers full ownership to them.

&#x20; All access lists are cleared on transfer.



**ABANDON** (Owner only)

&#x20; Immediately clears ownership and resets to Purchasable.

&#x20; Items remain in place but immovable for the next owner to claim.



\--------------------------------------------------------------------------------

**REGION BEHAVIOUR**

\--------------------------------------------------------------------------------



&#x20; - Ban enforcement — banned players cannot step into the house region.

&#x20; - Secured container access — non-friends attempting to open a secured

&#x20;   container receive an access denied message.



\--------------------------------------------------------------------------------

**ITEM BEHAVIOUR ON EVICTION / ABANDONMENT**

\--------------------------------------------------------------------------------



&#x20; When a house is evicted (GM) or abandoned (player):

&#x20; - All lockdown and secure registrations are cleared.

&#x20; - Items remain physically in place but immovable.

&#x20; - IsLockedDown and IsSecure flags are cleared so the engine does not

&#x20;   treat them as belonging to the old owner.

&#x20; - The new owner can re-lockdown or re-secure items after purchasing.

&#x20; - If WipePolicy is set to WipeItemsInBounds, all items inside the

&#x20;   bounds are deleted instead.



\--------------------------------------------------------------------------------

**NOTES**

\--------------------------------------------------------------------------------



&#x20; - \[add TownHouseController is the only command needed to place a house.

&#x20; - Each sign is self-contained — no external region files or XML needed.

&#x20; - Multiple town houses can exist on the same shard with no conflicts.

&#x20; - TownHouseConfig.UseAccountOwnership (in TownHouseSystem.cs) controls

&#x20;   whether ownership is checked per-character (false) or per-account (true).

&#x20;   Default is false.



================================================================================

