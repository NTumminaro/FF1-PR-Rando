using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static FF1_PRR.Common.Common;
using FF1_PRR.Inventory;

namespace FF1_PRR.Randomize
{
	public class Magic
	{
		enum whiteMagic
		{
			wCure = 213,
			wProtect = 214,
			wDia = 215,
			wBlink = 216,
			wInvis = 221,
			wSilence = 222,
			wBlindna = 223,
			wNulShock = 224,
			wCura = 229,
			wNulBlaze = 230,
			wDiara = 231,
			wHeal = 232,
			wPoisona = 237,
			wNulFrost = 238,
			wFear = 239,
			wVox = 240,
			wCuraga = 245,
			wDiaga = 246,
			wLife = 247,
			wHealara = 248,
			wStona = 253,
			wProtera = 254,
			wExit = 255,
			wInvisira = 256,
			wCuraja = 261,
			wNulDeath = 262,
			wDiaja = 263,
			wHealaga = 264,
			wFullLife = 269,
			wNulAll = 270,
			wHoly = 271,
			wDispel = 272
		}

		enum blackMagic
		{
			bFire = 217,
			bFocus = 218,
			bSleep = 219,
			bThunder = 220,
			bBlizzard = 225,
			bTemper = 226,
			bDark = 227,
			bSlow = 228,
			bFira = 233,
			bThundara = 234,
			bHold = 235,
			bFocara = 236,
			bSleepra = 241,
			bConfuse = 242,
			bHaste = 243,
			bBlizzara = 244,
			bFiraga = 249,
			bTeleport = 250,
			bScourge = 251,
			bSlowra = 252,
			bThundaga = 257,
			bQuake = 258,
			bDeath = 259,
			bStun = 260,
			bBlizzaga = 265,
			bSaber = 266,
			bBreak = 267,
			bBlind = 268,
			bFlare = 273,
			bWarp = 274,
			bStop = 275,
			bKill = 276
		}

		public static List<int> bAll = Enum.GetValues(typeof(blackMagic)).Cast<int>().ToList();
		public static List<int> wAll = Enum.GetValues(typeof(whiteMagic)).Cast<int>().ToList();
		public static List<int> all = bAll.Concat(wAll).ToList();

		public static int WHITE_MAGIC = 1;
		public static int BLACK_MAGIC = 2;
		public static int DUPE_CURE_4 = 100;
		public static int MAGIC_ID_OFFSET = 208; // Offset for converting ability ID to shop content ID

		private List<ability> records;
		private string file;
		private string productpath;

		public Magic(Random r1, int randoLevel, string fileName, string product, bool shuffleShops, bool keepPermissions)
		{
			file = fileName;
			productpath = product;
			using (StreamReader reader = new StreamReader(fileName))
			using (CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
			{
				records = csv.GetRecords<ability>().ToList();
			}
			shuffleMagic(r1, randoLevel, shuffleShops, keepPermissions);
		}

		public List<ability> getRecords()
		{
			return records;
		}

		public void writeToFile()
		{
			using (StreamWriter writer = new StreamWriter(file))
			using (CsvWriter csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(records);
			}
		}

		public class ability
		{
			public int id { get; set; }
			public int sort_id { get; set; }
			public int ability_lv { get; set; }
			public int ability_group_id { get; set; }
			public int type_id { get; set; }
			public int attribute_id { get; set; }
			public int attribute_group_id { get; set; }
			public int system_id { get; set; }
			public int use_value { get; set; }
			public int standard_value { get; set; }
			public int adding_hit_rate { get; set; }
			public int valid_hit_rate { get; set; }
			public int weak_hit_rate { get; set; }
			public int attack_count { get; set; }
			public int accuracy_rate { get; set; }
			public int Impact_status { get; set; }
			public int use_job_group_id { get; set; }
			public int condition_group_id { get; set; }
			public int renge_id { get; set; }
			public int menu_renge_id { get; set; }
			public int battle_renge_id { get; set; }
			public int content_flag_group_id { get; set; }
			public int invalid_reflection { get; set; }
			public int invalid_boss { get; set; }
			public int resistance_attribute { get; set; }
			public int battle_effect_asset_id { get; set; }
			public int menu_se_asset_id { get; set; }
			public int reaction_type { get; set; }
			public int menu_function_group_id { get; set; }
			public int battle_function_group_id { get; set; }
			public int buy { get; set; }
			public int sell { get; set; }
			public int sales_not_possible { get; set; }
			public int ability_wait { get; set; }
			public string process_prog { get; set; }
			public int data_a { get; set; }
			public int data_b { get; set; }
			public int data_c { get; set; }
		}

		public void shuffleMagic(Random r1, int randoLevel, bool shuffleShops, bool keepPermissions)
		{
			// Shuffle levels and price between the white spells and then the black spells.
			List<int> wMagic = new List<int> {
				4, 5, 6, 7,
				12, 13, 14, 15,
				20, 21, 22, 23,
				28, 29, 30, 31,
				36, 37, 38, 39,
				44, 45, 46, 47,
				52, 53, 54, 55,
				60, 61, 62, 63
		};
			List<int> bMagic = new List<int> {
				8, 9, 10, 11,
				16, 17, 18, 19,
				24, 25, 26, 27,
				32, 33, 34, 35,
				40, 41, 42, 43,
				48, 49, 50, 51,
				56, 57, 58, 59,
				64, 65, 66, 67
		};

			if (randoLevel == 1 || randoLevel == 2) // Standard and Pro
			{
				List<ability> spellbook = new List<ability>();
				List<int> levels = new List<int>();
				List<int> prices = new List<int>();
				// we have to go through it twice to get the level and price index in the correct order...
				foreach (ability spell in records)
				{
					if (spell.ability_group_id == 1 && spell.type_id == 1 && spell.id != DUPE_CURE_4)
					{
						spellbook.Insert(0, spell);
						spellbook[0].sort_id = spell.id + r1.Next(0, 100);
						levels.Add(spell.ability_lv);
						prices.Add(spell.buy);
					}
				}
				foreach (ability spell in records)
				{
					if (spell.ability_group_id == 1 && spell.type_id == 2 && spell.id != DUPE_CURE_4)
					{
						spellbook.Insert(0, spell);
						spellbook[0].sort_id = spell.id + r1.Next(0, 100);
						levels.Add(spell.ability_lv);
						prices.Add(spell.buy);
					}
				}
				if (randoLevel == 2)
				{
					// swap items by family
					sortByFamily(spellbook);
				}
				// sort by school, then sort ID
				spellbook.Sort((x, y) =>
				{
					int bySchool = x.type_id.CompareTo(y.type_id);
					if (bySchool == 0)
					{
						return x.sort_id.CompareTo(y.sort_id);
					}
					return bySchool;
				});

				foreach (ability spell in spellbook)
				{
					spell.ability_lv = levels[0];
					spell.buy = prices[0];
					levels.RemoveAt(0);
					prices.RemoveAt(0);
				}
			}

			if (randoLevel == 3 || randoLevel == 4) // Wild and Chaos
			{
				// Enhanced shuffling logic
				void shuffleList(List<int> list)
				{
					for (int i = list.Count - 1; i > 0; i--)
					{
						int j = r1.Next(i + 1);
						int temp = list[i];
						list[i] = list[j];
						list[j] = temp;
					}
				}

				// Shuffle by chunks to break patterns
				void shuffleByChunks(List<int> magicList, int chunkSize)
				{
					int numChunks = magicList.Count / chunkSize;
					List<int> chunks = Enumerable.Range(0, numChunks).ToList();
					shuffleList(chunks);

					for (int i = 0; i < numChunks; i++)
					{
						int chunkStart = chunks[i] * chunkSize;
						int chunkEnd = chunkStart + chunkSize;

						List<int> chunk = magicList.GetRange(chunkStart, chunkSize);
						shuffleList(chunk);

						for (int j = chunkStart; j < chunkEnd; j++)
						{
							magicList[j] = chunk[j - chunkStart];
						}
					}
				}

				shuffleByChunks(wMagic, 4);
				shuffleByChunks(bMagic, 4);

				// Additional shuffling rounds for better distribution
				for (int i = 0; i < 5; i++)
				{
					shuffleList(wMagic);
					shuffleList(bMagic);
				}

				// Apply the shuffled magic to the records
				for (int lnI = 0; lnI < 640; lnI++)
				{
					List<int> magic = lnI < 320 ? wMagic : bMagic;

					int ln1 = magic[r1.Next() % magic.Count];
					int ln2 = magic[r1.Next() % magic.Count];
					int buy = records[ln1].buy;
					int level = records[ln1].ability_lv;
					records[ln1].buy = records[ln2].buy;
					records[ln1].ability_lv = records[ln2].ability_lv;
					records[ln2].buy = buy;
					records[ln2].ability_lv = level;
				}
			}

			if (!keepPermissions)
			{
				List<int> permissions = new List<int> {
						42, 42, 27, 43,
						42, 42, 42, 42,
						42, 42, 27, 27,
						45, 45, 27, 46,
						45, 27, 46, 27,
						27, 46, 50, 46,
						24, 46, 24, 27,
						24, 24, 24, 24,

						44, 44, 44, 44,
						44, 44, 44, 44,
						44, 44, 44, 44,
						44, 44, 44, 44,
						47, 48, 49, 47,
						49, 29, 29, 29,
						49, 22, 22, 29,
						22, 22, 22, 22
				};

				int lnJ = 0;
				for (int lnI = 1; lnI <= 8; lnI++)
				{
					foreach (int wm in wMagic)
					{
						if (records[wm].ability_lv == lnI)
						{
							records[wm].use_job_group_id = permissions[lnJ];
							lnJ++;
						}
					}
				}
				for (int lnI = 1; lnI <= 8; lnI++)
				{
					foreach (int bm in bMagic)
					{
						if (records[bm].ability_lv == lnI)
						{
							records[bm].use_job_group_id = permissions[lnJ];
							lnJ++;
						}
					}
				}
			}

			//clear out old spell inventory
			List<ShopItem> shopDB = Product.readShopDB(productpath);
			shopDB = shopDB.FindAll(x => !Product.allMagicStores.Contains(x.group_id));

			//then, add the new spell inventory
			shopDB = determineSpells(r1, randoLevel, shuffleShops, shopDB);
			shopDB.Sort((x, y) => x.id.CompareTo(y.id));

			//and write out the product.csv
			Product.writeShopDB(productpath, shopDB);
			//and the ability.csv
			writeToFile();
		}


		private void sortByFamily(List<ability> spellbook)
		{
			List<List<int>> spellFamilies = new List<List<int>>()
			{
				new List<int>(){(int)blackMagic.bFire,    (int)blackMagic.bFira,    (int)blackMagic.bFiraga,  (int)blackMagic.bFlare},
				new List<int>(){(int)blackMagic.bBlizzard,(int)blackMagic.bBlizzara,(int)blackMagic.bBlizzaga,(int)blackMagic.bFlare},
				new List<int>(){(int)blackMagic.bThunder, (int)blackMagic.bThundara,(int)blackMagic.bThundaga,(int)blackMagic.bFlare},
				new List<int>(){(int)blackMagic.bFocus,   (int)blackMagic.bFocara},
				new List<int>(){(int)blackMagic.bSleep,   (int)blackMagic.bSleepra},
				new List<int>(){(int)blackMagic.bDark,    (int)blackMagic.bBlind},
				new List<int>(){(int)blackMagic.bSlow,    (int)blackMagic.bSlowra},
				new List<int>(){(int)blackMagic.bHold,    (int)blackMagic.bStun},
				new List<int>(){(int)blackMagic.bTeleport,(int)blackMagic.bWarp},
				new List<int>(){(int)blackMagic.bDeath,   (int)blackMagic.bKill},
				new List<int>(){(int)blackMagic.bStun,    (int)blackMagic.bBlind,   (int)blackMagic.bKill},

				new List<int>(){(int)whiteMagic.wCure,    (int)whiteMagic.wCura,    (int)whiteMagic.wCuraga,(int)whiteMagic.wCuraja},
				new List<int>(){(int)whiteMagic.wProtect, (int)whiteMagic.wProtera},
				new List<int>(){(int)whiteMagic.wDia,     (int)whiteMagic.wDiara,   (int)whiteMagic.wDiaga, (int)whiteMagic.wDiaja, (int)whiteMagic.wHoly},
				new List<int>(){(int)whiteMagic.wNulShock,(int)whiteMagic.wNulAll},
				new List<int>(){(int)whiteMagic.wInvis,   (int)whiteMagic.wInvisira},
				new List<int>(){(int)whiteMagic.wNulBlaze,(int)whiteMagic.wNulAll},
				new List<int>(){(int)whiteMagic.wHeal,    (int)whiteMagic.wHealara, (int)whiteMagic.wHealaga},
				new List<int>(){(int)whiteMagic.wNulFrost,(int)whiteMagic.wNulAll},
				new List<int>(){(int)whiteMagic.wLife,    (int)whiteMagic.wFullLife},
				new List<int>(){(int)whiteMagic.wNulDeath,(int)whiteMagic.wNulAll}
			};
			foreach (List<int> family in spellFamilies)
			{
				List<int> sort_ids = new List<int>();
				List<ability> spells = new List<ability>();
				foreach (int index in family)
				{
					ability spell = spellbook.Find(x => x.id == index - MAGIC_ID_OFFSET);
					spells.Add(spell);
					sort_ids.Add(spell.sort_id);
				}
				sort_ids.Sort();
				foreach (ability spell in spells)
				{
					spell.sort_id = sort_ids[0];
					sort_ids.RemoveAt(0);
				}
			}
		}

		private List<ShopItem> determineSpells(Random r1, int randoLevel, bool shuffleShops, List<ShopItem> shopDB)
		{
			int[,] magicMemory = new int[2, 8];
			int productID = GetNextAvailableProductId(shopDB);
			
			List<int> shopLookup = CreateShopLookup();
			var (wmShops, bmShops) = GetMagicShops(shuffleShops, r1);
			
			if (randoLevel == 4) // Chaos mode
			{
				shopLookup.Shuffle(r1);
				wmShops.Shuffle(r1);
				bmShops.Shuffle(r1);
			}

			foreach (ability spell in records)
			{
				if (spell.ability_group_id == 1 && spell.id != DUPE_CURE_4)
				{
					var shopItem = CreateMagicShopItem(spell, shopLookup, wmShops, bmShops, magicMemory, productID);
					shopDB.Add(shopItem);
					
					magicMemory[spell.type_id - 1, spell.ability_lv - 1]++;
					productID = GetNextAvailableProductId(shopDB);
				}
			}

			return shopDB;
		}

		private int GetNextAvailableProductId(List<ShopItem> shopDB)
		{
			return Enumerable.Range(1, 1000).Except(shopDB.Select(x => x.id)).First();
		}

		private List<int> CreateShopLookup()
		{
			return new List<int>
			{
				0,0,0,0,  // Level 1 spells: 4 spells per level, distributed across shops
				1,1,1,1,  // Level 2 spells
				2,2,2,2,  // Level 3 spells
				3,3,3,3,  // Level 4 spells
				4,4,4,4,  // Level 5 spells
				5,5,5,5,  // Level 6 spells
				6,6,8,8,  // Level 7 spells (note: shops 6 and 8)
				7,7,9,9   // Level 8 spells (note: shops 7 and 9)
			};
		}

		private (List<int> wmShops, List<int> bmShops) GetMagicShops(bool shuffleShops, Random r1)
		{
			List<int> wmShops = new List<int>(Product.whiteMagicStores);
			List<int> bmShops = new List<int>(Product.blackMagicStores);
			
			if (shuffleShops)
			{
				// Preserve the first shop (usually the starting town) but shuffle the rest
				var wmHead = wmShops[0];
				var bmHead = bmShops[0];
				
				var wmTail = wmShops.Skip(1).ToList();
				var bmTail = bmShops.Skip(1).ToList();
				
				wmTail.Shuffle(r1);
				bmTail.Shuffle(r1);
				
				wmShops = new List<int> { wmHead };
				wmShops.AddRange(wmTail);
				
				bmShops = new List<int> { bmHead };
				bmShops.AddRange(bmTail);
			}
			
			return (wmShops, bmShops);
		}

		private ShopItem CreateMagicShopItem(ability spell, List<int> shopLookup, List<int> wmShops, List<int> bmShops, int[,] magicMemory, int productID)
		{
			List<int> shopType = (spell.type_id == 1) ? wmShops : bmShops;
			int shopIndex = CalculateShopIndex(spell, shopLookup, magicMemory);
			
			// Ensure we don't go out of bounds
			if (shopIndex >= shopType.Count)
			{
				shopIndex = shopType.Count - 1;
			}
			
			return new ShopItem
			{
				id = productID,
				content_id = spell.id + MAGIC_ID_OFFSET, // Magic constant for ability ID -> shop ID mapping
				group_id = shopType[shopIndex]
			};
		}

		private int CalculateShopIndex(ability spell, List<int> shopLookup, int[,] magicMemory)
		{
			int levelIndex = (spell.ability_lv - 1) * 4; // 4 spells per level
			int spellCountForLevel = magicMemory[spell.type_id - 1, spell.ability_lv - 1];
			int lookupIndex = levelIndex + spellCountForLevel;
			
			// Ensure we don't go out of bounds of shopLookup
			if (lookupIndex >= shopLookup.Count)
			{
				lookupIndex = shopLookup.Count - 1;
			}
			
			return shopLookup[lookupIndex];
		}
	}
}
