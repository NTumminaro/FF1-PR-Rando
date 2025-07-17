using CsvHelper;
using FF1_PRR.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FF1_PRR.Randomize
{
    public class MonsterEncounters
    {
        private readonly RandomizerLogger logger;

        public class MonsterSet
        {
            public int id { get; set; }
            public int monster_set1 { get; set; }
            public int monster_set1_rate { get; set; }
            public int monster_set2 { get; set; }
            public int monster_set2_rate { get; set; }
            public int monster_set3 { get; set; }
            public int monster_set3_rate { get; set; }
            public int monster_set4 { get; set; }
            public int monster_set4_rate { get; set; }
            public int monster_set5 { get; set; }
            public int monster_set5_rate { get; set; }
            public int monster_set6 { get; set; }
            public int monster_set6_rate { get; set; }
            public int monster_set7 { get; set; }
            public int monster_set7_rate { get; set; }
            public int monster_set8 { get; set; }
            public int monster_set8_rate { get; set; }
            public int monster_set9 { get; set; }
            public int monster_set9_rate { get; set; }
            public int monster_set10 { get; set; }
            public int monster_set10_rate { get; set; }
            public int monster_set11 { get; set; }
            public int monster_set11_rate { get; set; }
            public int monster_set12 { get; set; }
            public int monster_set12_rate { get; set; }
            public int monster_set13 { get; set; }
            public int monster_set13_rate { get; set; }
            public int monster_set14 { get; set; }
            public int monster_set14_rate { get; set; }
            public int monster_set15 { get; set; }
            public int monster_set15_rate { get; set; }
            public int monster_set16 { get; set; }
            public int monster_set16_rate { get; set; }
        }

        public MonsterEncounters(Random r1, string dataPath, bool shuffleEncounters)
        {
            logger = new RandomizerLogger();
            
            if (shuffleEncounters)
            {
                using var operation = logger.StartOperation("Monster Encounter Shuffling");
                
                try
                {
                    ShuffleMonsterEncounters(r1, dataPath);
                    operation.Complete("Monster encounters shuffled successfully");
                }
                catch (Exception ex)
                {
                    operation.Fail("Monster encounter shuffling failed", ex);
                    logger.Critical("Monster encounter shuffling failed", "MonsterEncounters", ex);
                    throw new RandomizationException("Monster encounter shuffling failed", ex);
                }
            }
        }

        private void ShuffleMonsterEncounters(Random r1, string dataPath)
        {
            string monsterSetPath = Path.Combine(dataPath, "monster_set.csv");
            
            // Validate file exists
            var fileValidation = ValidationUtility.ValidateFileExists(monsterSetPath, "Monster Set CSV");
            if (!fileValidation.IsValid)
            {
                throw new FileNotFoundException($"Monster set file not found: {monsterSetPath}");
            }

            logger.Info("Loading monster encounter data", "MonsterEncounters");

            // Load monster set data
            List<MonsterSet> monsterSets;
            using (var reader = new StreamReader(monsterSetPath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                monsterSets = csv.GetRecords<MonsterSet>().ToList();
            }

            logger.Info($"Loaded {monsterSets.Count} monster encounter areas", "MonsterEncounters");

            // Extract all monster party IDs (excluding 0 which means no encounter)
            var allMonsterPartyIds = ExtractAllMonsterPartyIds(monsterSets);
            
            logger.Info($"Found {allMonsterPartyIds.Count} unique monster parties to shuffle", "MonsterEncounters");

            // Shuffle the monster party IDs
            var shuffledIds = new List<int>(allMonsterPartyIds);
            ShuffleList(shuffledIds, r1);

            // Apply shuffled IDs back to monster sets
            ApplyShuffledIds(monsterSets, allMonsterPartyIds, shuffledIds);

            // Write back to file
            using (var writer = new StreamWriter(monsterSetPath))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(monsterSets);
            }

            logger.Info("Monster encounters shuffled and saved successfully", "MonsterEncounters");
        }

        private List<int> ExtractAllMonsterPartyIds(List<MonsterSet> monsterSets)
        {
            var allIds = new HashSet<int>();

            foreach (var set in monsterSets)
            {
                // Extract all non-zero monster party IDs from each set
                var ids = new int[]
                {
                    set.monster_set1, set.monster_set2, set.monster_set3, set.monster_set4,
                    set.monster_set5, set.monster_set6, set.monster_set7, set.monster_set8,
                    set.monster_set9, set.monster_set10, set.monster_set11, set.monster_set12,
                    set.monster_set13, set.monster_set14, set.monster_set15, set.monster_set16
                };

                foreach (var id in ids.Where(id => id > 0))
                {
                    allIds.Add(id);
                }
            }

            return allIds.ToList();
        }

        private void ShuffleList<T>(List<T> list, Random r1)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = r1.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private void ApplyShuffledIds(List<MonsterSet> monsterSets, List<int> originalIds, List<int> shuffledIds)
        {
            // Create mapping from original to shuffled IDs
            var idMapping = new Dictionary<int, int>();
            for (int i = 0; i < originalIds.Count; i++)
            {
                idMapping[originalIds[i]] = shuffledIds[i];
            }

            // Apply mapping to all monster sets
            foreach (var set in monsterSets)
            {
                set.monster_set1 = RemapId(set.monster_set1, idMapping);
                set.monster_set2 = RemapId(set.monster_set2, idMapping);
                set.monster_set3 = RemapId(set.monster_set3, idMapping);
                set.monster_set4 = RemapId(set.monster_set4, idMapping);
                set.monster_set5 = RemapId(set.monster_set5, idMapping);
                set.monster_set6 = RemapId(set.monster_set6, idMapping);
                set.monster_set7 = RemapId(set.monster_set7, idMapping);
                set.monster_set8 = RemapId(set.monster_set8, idMapping);
                set.monster_set9 = RemapId(set.monster_set9, idMapping);
                set.monster_set10 = RemapId(set.monster_set10, idMapping);
                set.monster_set11 = RemapId(set.monster_set11, idMapping);
                set.monster_set12 = RemapId(set.monster_set12, idMapping);
                set.monster_set13 = RemapId(set.monster_set13, idMapping);
                set.monster_set14 = RemapId(set.monster_set14, idMapping);
                set.monster_set15 = RemapId(set.monster_set15, idMapping);
                set.monster_set16 = RemapId(set.monster_set16, idMapping);
            }
        }

        private int RemapId(int originalId, Dictionary<int, int> idMapping)
        {
            // Keep 0 as 0 (no encounter)
            if (originalId == 0)
                return 0;

            // Return shuffled ID if mapping exists, otherwise keep original
            return idMapping.TryGetValue(originalId, out int shuffledId) ? shuffledId : originalId;
        }
    }
}