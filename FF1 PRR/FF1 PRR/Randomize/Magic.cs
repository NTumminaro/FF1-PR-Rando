using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class Magic
{
  private List<Spell> spells;
  private List<Product> products;
  private string abilityFilePath;
  private string productFilePath;

  // Constructor
  public Magic(string abilityFilePath, string productFilePath)
  {
    this.abilityFilePath = abilityFilePath;
    this.productFilePath = productFilePath;
    spells = LoadSpells(abilityFilePath);
    products = LoadProducts(productFilePath);
  }

  private List<Spell> LoadSpells(string filePath)
  {
    // Read the CSV and return a list of spells
    using (var reader = new StreamReader(filePath))
    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      PrepareHeaderForMatch = args => args.Header.ToLower()
    }))
    {
      return csv.GetRecords<Spell>().ToList();
    }
  }

  // Method to write the randomized spells to a file
  private void WriteSpells(List<Spell> spells, string filePath)
  {
    using (var writer = new StreamWriter(filePath))
    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      PrepareHeaderForMatch = args => args.Header.ToLower()
    }))
    {
      csv.WriteRecords(spells);
    }
  }

  private List<Product> LoadProducts(string filePath)
  {
    using (var reader = new StreamReader(filePath))
    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      PrepareHeaderForMatch = args => args.Header.ToLower()
    }))
    {
      return csv.GetRecords<Product>().ToList();
    }
  }

  private void WriteProducts(List<Product> products, string filePath)
  {
    using (var writer = new StreamWriter(filePath))
    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      PrepareHeaderForMatch = args => args.Header.ToLower()
    }))
    {
      csv.WriteRecords(products);
    }
  }

  // Method to randomize the spells
  public void ShuffleMagic(Random random, int randoLevel, bool shuffleSpellsPerShop = false, bool randomPrices = false, bool retainJobRequirements = true)
  {
    switch (randoLevel)
    {
      case 1: // Simple
        SimpleShuffle(random, shuffleSpellsPerShop, randomPrices, retainJobRequirements);
        break;
      case 2: // Standard
        SimpleShuffle(random, shuffleSpellsPerShop, randomPrices, retainJobRequirements);
        ShuffleShopsStandard(random);
        break;
      case 3: // Pro
        ProShuffle(random, shuffleSpellsPerShop, randomPrices, retainJobRequirements);
        break;
      case 4: // Chaos
        ChaosShuffle(random, shuffleSpellsPerShop, randomPrices, retainJobRequirements);
        break;
    }

    WriteSpells(spells, abilityFilePath);
    WriteProducts(products, productFilePath);
  }

  // Simple shuffle randomizes the spells and optionally the prices
  private void SimpleShuffle(Random random, bool shuffleSpellsPerShop, bool randomPrices, bool retainJobRequirements)
  {
    // Get all the white and black spells based on the type_id
    var whiteSpells = spells.Where(s => s.type_id == 1).ToList();
    var blackSpells = spells.Where(s => s.type_id == 2).ToList();

    // Shuffle the spells and apply the attributes
    ShuffleAndApplyAttributes(random, whiteSpells, randomPrices, shuffleSpellsPerShop, retainJobRequirements);
    ShuffleAndApplyAttributes(random, blackSpells, randomPrices, shuffleSpellsPerShop, retainJobRequirements);
  }

  private void ShuffleShopsStandard(Random random)
  {
    // Separate white and black magic shops
    var whiteMagicShopGroupIds = new List<int> { 4, 9, 14, 16, 20, 25, 28, 33, 35, 39 };
    var blackMagicShopGroupIds = new List<int> { 5, 10, 15, 17, 21, 26, 29, 34, 36, 40 };

    // Group spells by shop levels
    var groupedWhiteShops = products.Where(p => whiteMagicShopGroupIds.Contains(p.group_id))
                                    .GroupBy(p => p.group_id)
                                    .OrderBy(g => g.Key)
                                    .ToList();
    var groupedBlackShops = products.Where(p => blackMagicShopGroupIds.Contains(p.group_id))
                                    .GroupBy(p => p.group_id)
                                    .OrderBy(g => g.Key)
                                    .ToList();

    // Shuffle the group IDs, ensuring the spell levels stay the same
    ShuffleGroupIds(random, groupedWhiteShops, whiteMagicShopGroupIds);
    ShuffleGroupIds(random, groupedBlackShops, blackMagicShopGroupIds);
  }

  private void ShuffleGroupIds(Random random, List<IGrouping<int, Product>> groupedShops, List<int> groupIds)
  {
    var shuffledGroupIds = groupIds.OrderBy(_ => random.Next()).ToList();

    for (int i = 0; i < groupedShops.Count; i++)
    {
      var originalGroup = groupedShops[i];
      var newGroupId = shuffledGroupIds[i];

      foreach (var product in originalGroup)
      {
        product.group_id = newGroupId;
      }
    }
  }

  private void ProShuffle(Random random, bool shuffleSpellsPerShop, bool randomPrices, bool retainJobRequirements)
  {
    // Get all the white and black spells based on the type_id
    var whiteSpells = spells.Where(s => s.type_id == 1).ToList();
    var blackSpells = spells.Where(s => s.type_id == 2).ToList();

    ShuffleAndApplyAttributes(random, whiteSpells, randomPrices, shuffleSpellsPerShop, retainJobRequirements);
    ShuffleAndApplyAttributes(random, blackSpells, randomPrices, shuffleSpellsPerShop, retainJobRequirements);

    ShuffleShopsStandard(random);
  }

  private void ChaosShuffle(Random random, bool shuffleSpellsPerShop, bool randomPrices, bool retainJobRequirements)
  {
    ShuffleAndApplyAttributes(random, spells, randomPrices, shuffleSpellsPerShop, retainJobRequirements);

    if (shuffleSpellsPerShop)
    {
      ShuffleNumberOfSpellsPerShop(random);
    }
  }

  private void ShuffleAndApplyAttributes(Random random, List<Spell> spellList, bool randomPrices, bool shuffleTypeAndGroup, bool retainJobRequirements)
  {
    var shuffledAttributes = spellList.Select(s => new
    {
      s.attribute_id,
      s.attribute_group_id,
      s.system_id,
      s.use_value,
      s.standard_value,
      s.adding_hit_rate,
      s.valid_hit_rate,
      s.weak_hit_rate,
      s.attack_count,
      s.accuracy_rate,
      s.Impact_status,
      s.condition_group_id,
      s.renge_id,
      s.menu_renge_id,
      s.battle_renge_id,
      s.content_flag_group_id,
      s.invalid_reflection,
      s.invalid_boss,
      s.resistance_attribute,
      s.battle_effect_asset_id,
      s.menu_se_asset_id,
      s.reaction_type,
      s.menu_function_group_id,
      s.battle_function_group_id,
      s.ability_wait,
      s.process_prog,
      s.data_a,
      s.data_b,
      s.data_c
    }).OrderBy(_ => random.Next()).ToList();

    for (int i = 0; i < spellList.Count; i++)
    {
      var targetSpell = spellList[i];
      var sourceAttributes = shuffledAttributes[i];

      targetSpell.attribute_id = sourceAttributes.attribute_id;
      targetSpell.attribute_group_id = sourceAttributes.attribute_group_id;
      targetSpell.system_id = sourceAttributes.system_id;
      targetSpell.use_value = sourceAttributes.use_value;
      targetSpell.standard_value = sourceAttributes.standard_value;
      targetSpell.adding_hit_rate = sourceAttributes.adding_hit_rate;
      targetSpell.valid_hit_rate = sourceAttributes.valid_hit_rate;
      targetSpell.weak_hit_rate = sourceAttributes.weak_hit_rate;
      targetSpell.attack_count = sourceAttributes.attack_count;
      targetSpell.accuracy_rate = sourceAttributes.accuracy_rate;
      targetSpell.Impact_status = sourceAttributes.Impact_status;
      targetSpell.condition_group_id = sourceAttributes.condition_group_id;
      targetSpell.renge_id = sourceAttributes.renge_id;
      targetSpell.menu_renge_id = sourceAttributes.menu_renge_id;
      targetSpell.battle_renge_id = sourceAttributes.battle_renge_id;
      targetSpell.content_flag_group_id = sourceAttributes.content_flag_group_id;
      targetSpell.invalid_reflection = sourceAttributes.invalid_reflection;
      targetSpell.invalid_boss = sourceAttributes.invalid_boss;
      targetSpell.resistance_attribute = sourceAttributes.resistance_attribute;
      targetSpell.battle_effect_asset_id = sourceAttributes.battle_effect_asset_id;
      targetSpell.menu_se_asset_id = sourceAttributes.menu_se_asset_id;
      targetSpell.reaction_type = sourceAttributes.reaction_type;
      targetSpell.menu_function_group_id = sourceAttributes.menu_function_group_id;
      targetSpell.battle_function_group_id = sourceAttributes.battle_function_group_id;
      targetSpell.ability_wait = sourceAttributes.ability_wait;
      targetSpell.process_prog = sourceAttributes.process_prog;
      targetSpell.data_a = sourceAttributes.data_a;
      targetSpell.data_b = sourceAttributes.data_b;
      targetSpell.data_c = sourceAttributes.data_c;

      if (retainJobRequirements)
      {
        targetSpell.use_job_group_id = spellList[i].use_job_group_id;
      }
    }

    if (randomPrices)
    {
      foreach (var spell in spellList)
      {
        spell.buy = random.Next(50, 10000);
      }
    }
  }

  private void ShuffleNumberOfSpellsPerShop(Random random)
  {
    var whiteMagicShopGroupIds = new List<int> { 4, 9, 14, 16, 20, 25, 28, 33, 35, 39 };
    var blackMagicShopGroupIds = new List<int> { 5, 10, 15, 17, 21, 26, 29, 34, 36, 40 };

    ShuffleSpellsPerShop(random, whiteMagicShopGroupIds);
    ShuffleSpellsPerShop(random, blackMagicShopGroupIds);
  }

  private void ShuffleSpellsPerShop(Random random, List<int> shopGroupIds)
  {
    var magicShops = products.Where(p => shopGroupIds.Contains(p.group_id)).ToList();
    var groupedShops = magicShops.GroupBy(p => p.group_id)
                                 .ToDictionary(g => g.Key, g => g.ToList());

    foreach (var group in groupedShops.Values)
    {
      int spellCount = group.Count;
      int numSpells = random.Next(1, spellCount + 1);

      for (int i = 0; i < spellCount; i++)
      {
        group[i].group_id = random.Next(1, spellCount + 1);
      }
    }
  }
}

public class Spell
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

public class Product
{
  public int id { get; set; }
  public int content_id { get; set; }
  public int group_id { get; set; }
  public int coefficient { get; set; }
  public int purchase_limit { get; set; }
}
