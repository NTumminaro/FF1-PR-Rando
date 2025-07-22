# Final Fantasy 1 Pixel Remaster Randomizer
This is a fork of gameboy9's [FF1PRR](https://github.com/gameboy9/FF1-PR-Rando) with the goal of adding more features and improving the randomizer.
Without gameboy9's work, this project would not be possible.
Below is the original README.md from gameboy9's repository with some modifications to reflect the changes made in this fork.
I realize my verion number control is a bit of a leap, but I considered gameboy9's randomizer my 1.0, and there's no going back now.

## Installation:
1. Install Final Fantasy Pixel Remaster from Steam.
3. Download and unpack the [FF1PRR Release](https://github.com/gameboy9/Memoria.FFPR/releases/download/0.1/FF1.PRR.zip) in the directory of your choice.
4. Run the .exe and set the `FF1 PR Folder` to the location of your game installation (usually `C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY PR`)

## Usage:

![Screenshot (489)](https://github.com/user-attachments/assets/6ff2b0ec-f8c6-4fb2-98da-c01c22b137d2)


Select the settings you would like to be included in the randomization and press the `Randomize!` button. You do not need to restore the game to its base ("vanilla") configuration before randomizing, but if you want to turn off the randomizer and return your game to its original state, press the `Restore vanilla` button.

The bottom left corner of the program window will give you status information about the state of the randomizer. On slower computers, the program may take a second or two to complete randomization.

The `Seed` value controls the randomization. If you want to play the same game again, or wish to race against a friend, simply ensure the same flags are selected and use the same seed value as before. `Gameplay Flags` can be set with preset values, as well as loaded from the string in the `Gameplay Flags` box. This allows you to share your settings with others.

### Key Item randomization
The `Randomize Key Items` flag is the heart of the randomizer. This will change the locations of the key items and events necessary to progress in the game. For example, you normally obtain the Mystic Key from waking the Elf Prince, but you might instead obtain it from Dr. Unne in Melmond after trading him the Rosetta Stone. Like in the HMS Jayne randomizer (and unlike the NES FFR), the four Crystals are also included in this randomization; you could restore the power of Earth by talking with the Sage Sadda in his cave, or the power of Fire by defeating the Kraken.

As a guarantee, every Key Item is always obtainable in every seed. You may not need the nitro powder or even the Airship to complete the game (for example), but it will always be *possible* to obtain every item.

You begin your quest with the ship and with the bridge between Coneria and Pravoka already built. Accordingly, two potential Key Item locations will award you only a single Potion for completing their requirements.

To complete the game, you must find the Lute and the Mystic Key, and restore the powers of the four elemental Crystals. Then, proceed to the Temple of Chaos and complete the final dungeon.

### Shop and Treasure shuffle
At present, these flags do as they say: shuffle all possible chest contents or shop contents among their peers.

If you want a greater challenge, you may optionally exclude the "new" items from shops, making things like Ether and Phoenix Down only obtainable from chests.

### Randomize Magic
This will shuffle the White and Black Magic spellbooks so any spell can appear at any level. Coneria will still stock all level 1 spells, but they could be Scourge, Blind, Firaga, and Stun just as easily as anything else.

If the `Keep Permissions` flag is checked, the spells will retain their casting restrictions (for example, only a Black Wizard may cast Flare). If unchecked, the same number of spells each level will be restricted, but they could be any spells (so a Red Mage could only learn two of the four 1st level White Magic spells, for example).

### Other flags
|Flag|Effect|
|----|------|
|Reduce Chaos HP|This will set Chaos base HP to the value specified with the slider|
|Harder bosses|Increase the HP of most bosses, excluding Chaos.|
|Fiends drop Ribbons|Defeating one of the four elemental Fiends will award a Ribbon in addition to a Key Item, but Ribbons will not appear in chests.|
|XP/Gil Boost|For faster games, inceases the amount of XP and Gil awarded in combat.|
|Rebalance item prices|Increases the price of the "new" items such as Ether and Phoenix Down to make them harder to access during the early game.|
|Restore crit rating|Sets the weapon critical hit rate to the originally intended value, which results in most endgame weapons aside from the Vorpal Sword and Sasuke's Katana having much lower chances to crit.|
|Wands add INT|To increase the value of weapons for caster classes, they provide a small INT bonus.|
|Reduce encounter rate|This restores the correct Ocean encounter rate of 1/3 that of land, and slightly decreases encounter chance on the overworld and dungeons, resulting in about 20% fewer encounters.|

### Known Issues
This is a beta build of the randomizer, so there is a non-zero chance you run into an issue that breaks the game. If you find yourself in a situation where you cannot progress, please let me know so I can fix it. Here are some known issues that you may encounter:

- Bottled faerie and airship can only be obtained from their vanilla locations.
- Mystic Key doors seem like they requires defeating Astos (or the boss at Astos's location) or turning in the jolt tonic.
- Some cutscenes have incorrect or misleading dialogue or have strange behavior.
- When using Hidden Chaos, Jack in the Box, or Shuffle Canoe, there is a chance that the game will softlock. Some of the NPCs and key item locations have intricate scripts, and it's possible there are edge cases that I have not accounted for.
- Sometimes the game gives you 2 or more canoes when using canoe shuffle, this is only cosmetic, and does not change how the randomizer progresses.
- When Dock Anywhere is enabled, some locations do not let you pass the collision on the edge of a continent. This is due to diagonal continent edges, and fixing it requires manual tweaking of the attribute.csv, and its massive.
- **There is an issue regarding Dock Anywhere and landing on a town or a cave entrance, prevent you from entering your ship again upon exit.**
- Currently, only the English language is supported. A future release will allow you to play in any language.

### More information
If you'd like to discuss this version of the randomizer or follow its continued development, you're welcome to join my discord [MisterStealYourGil](https://discord.gg/7TqXF3S)!

### Added Features and Changes
- Added the Seed number and setting string to the Main Menu and New Game screen.
- Added the option to change your overworld sprites to sprites from various FFPR games.
- Added the ability to load seting flags without closing the program.
- Shuffling regular treasure will now function correctly and not be ignored.
- Added Hidden Chaos and Jack in the Box gamemodes. When both are selected, Chaos will be either in a random chest or in a random NPC.
- Added a flag for Dock anywhere. This allows you to park your ship at any land formation, allowing for new logic and routing.
- Added Shuffle Canoe flag. This will include the canoe in the key item shuffle, allowing for new logic and routing.
- Added better tracking for the Crystal lighting up in the inventory.
- Added a cosmetic tab and further integrated the Cosmetic Flags.
- added a cosmetic setting to change NPC sprites with multiple settings (1 to 1 sprite shuffling, all random, and Oops all garland).
- Added a cosmetic setting to change the airship sprite.
- Added a cosmetic setting to change the ship sprite (there are not alot of ship options).
- Tweaked the shuffling magic logic for wild and chaos to be less systematic.
- Added and updated boss shuffling logic. Added options to ignore Garland when boss shuffling as well as replace Vampire (Boss) with Warmech.
- Added class restriction shuffling for magic (i.e. warriors are considered white mages for magic)
- Added Tiamat II, Lich II, Marilith II, and Kraken II as names for clarity (will be a cosmetic option in the future).
- Fixed Duplicate items in first item shop.
- Added Overworld encounter shuffling.
- Added a cosmetic setting to shuffle the battle background.

### Planned Features
- Add a gamemode called "Chaos Rush" where you must defeat Chaos as quickly as possible.
- Add a gamemode called "Shard Hunt" where you must collect all 28 shards to open Chaos Shrine instead of the 4 Crystals.
- Add the option to disable encounter toggling.
- Add a setting to Disable the flash when stepping on damage tiles.
- Add a setting to Reimplement spike tiles instead of npcs (possibly put a sprite there that represent the spike tile, akin to stepping on a teleporter).
- Add a setting to use differnt pixel remaster music.
