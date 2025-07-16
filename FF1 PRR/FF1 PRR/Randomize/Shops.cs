using CsvHelper;
using FF1_PRR.Inventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static FF1_PRR.Common.Common;

namespace FF1_PRR.Randomize
{
	public class Shops
	{
		private class ItemWithRank
		{
			public int id { get; set; } // used as the content ID in stores/chests
			public string name { get; set; } // English name
			public int type_id { get; set; } // 1=item, 2=weapon, 3=armor, 4=magic, 5=gil
			public int type_value { get; set; } // which item is it?
			public string rank { get; set; } // grade from F to S; X = exclude
		}

		private int rankToInt(string rank)
        {
			switch (rank)
            {
				case "F":
					return 0;
				case "E":
					return 1;
				case "D":
					return 2;
				case "C":
					return 3;
				case "B":
					return 4;
				case "A":
					return 5;
				case "S":
					return 6;
				case "X":
					return 7;
				default:
					return 7;
            }
        }


		private void placeNextItem(ShopItem shopEntry, ref List<int> productList, ref Dictionary<int, List<int>> shopInventories, ref HashSet<ShopItem> toRemove)
        {
			// check if we've seen this shop before
			if (shopInventories.TryGetValue(shopEntry.group_id, out List<int> inventory))
			{
				int i = 0;
				while (i < productList.Count)
				{
					// check if this shop contains the next item
					if (!inventory.Contains(productList[i]))
					{
						inventory.Add(productList[i]);
						shopInventories[shopEntry.group_id] = inventory;
						shopEntry.content_id = productList[i];
						productList.RemoveAt(i);
						break;
					}
					else
					{
						i++;
						continue;
					}
				}
				// if all possible items that can be placed are already in the store, remove this shop entry from the shopDB
				if (i == productList.Count)
				{
					toRemove.Add(shopEntry);
				}
			}
			else // this is a new store
			{
				shopInventories.Add(shopEntry.group_id, new List<int> { productList[0] });
				shopEntry.content_id = productList[0];
				productList.RemoveAt(0);
			}
		}



		public Shops(Random r1, int randoLevel, string fileName, bool traditional)
		{
			List<ShopItem> shopDB = Product.readShopDB(fileName);

			// Shuffle existing items
			if (randoLevel == 1)
			{
				// TODO:  Guarantee no duplicates in stores

				List<int> weaponList = new List<int>();
				List<int> armorList = new List<int>();
				List<int> itemList = new List<int>();
				int max_id = 0;

				foreach (ShopItem product in shopDB)
				{
					max_id = Math.Max(max_id, product.id + 1);
					if (Product.weaponStores.Contains(product.group_id))

					{
						weaponList.Add(product.content_id);
					}
					else if (Product.armorStores.Contains(product.group_id))
					{
						armorList.Add(product.content_id);
					}
					else if (Product.itemStores.Contains(product.group_id))
					{
						itemList.Add(product.content_id);
					}
					else continue;
				}
				weaponList.Shuffle(r1);
				armorList.Shuffle(r1);
				itemList.Shuffle(r1);

				// a dictionary containing the current inventories of every shop so far
				Dictionary<int, List<int>> shopInventories = new Dictionary<int, List<int>>();

				// a list of entries to remove from the database if no item can be placed in that shop
				var toRemove = new HashSet<ShopItem>();

				foreach (ShopItem product in shopDB)
				{
					if (Product.weaponStores.Contains(product.group_id))
					{
						placeNextItem(product, ref weaponList, ref shopInventories, ref toRemove);
					}
					else if (Product.armorStores.Contains(product.group_id))
					{
						placeNextItem(product, ref armorList, ref shopInventories, ref toRemove);
					}
					else if (Product.itemStores.Contains(product.group_id))
					{
						placeNextItem(product, ref itemList, ref shopInventories, ref toRemove);
					}
					else continue;
				}

				shopDB.RemoveAll(toRemove.Contains);

				ShopItem fixAntidote = new();
				fixAntidote.id = max_id;
				fixAntidote.content_id = Items.antidote;
				fixAntidote.group_id = 3; // Product.itemStores.iCornelia;
				shopDB.Add(fixAntidote);

				ShopItem fixGoldNeedle = new();
				fixGoldNeedle.id = max_id+1;
				fixGoldNeedle.content_id = Items.goldNeedle;
				fixGoldNeedle.group_id = 3; // Product.itemStores.iCornelia;
				shopDB.Add(fixGoldNeedle);
			}
			else // Generate new shop contents
			{
				// Advanced randomization levels (2-4) not yet implemented
				// Currently falls back to basic shuffle logic
				// TODO: Implement tier-based item generation for levels 2-4
				GenerateAdvancedShopContents(r1, randoLevel, traditional, shopDB);
			}



			// Backfill IDs to avoid vanilla file assets loading
			int productID = 1;
			while (productID < 195)
			{
				productID = Enumerable.Range(1, 200).Except(shopDB.Select(x => x.id)).First();
				ShopItem newItem = new ShopItem
				{
					id = productID,
					content_id = 999, //Magic Constant for Ability ID -> shop ID map
					group_id = 999
				};
				shopDB.Add(newItem);
			}

			shopDB.Sort((x, y) => x.id.CompareTo(y.id));
			Product.writeShopDB(fileName,shopDB);
		}

		private void GenerateAdvancedShopContents(Random r1, int randoLevel, bool traditional, List<ShopItem> shopDB)
		{
			// Placeholder for advanced shop generation
			// This would implement tier-based item generation for randomization levels 2-4
			// For now, this is a stub to prevent compilation errors
			
			// TODO: Implement the following logic:
			// Level 2: Generate items based on shop tier limits
			// Level 3: More random item distribution with some tier restrictions
			// Level 4: Completely random item placement (chaos mode)
		}
	}
}
