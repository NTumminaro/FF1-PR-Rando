using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FF1_PRR.Randomize
{
    public class BossNames
    {
        public BossNames(string dataPath, string messagePath, bool showBossNames)
        {
            if (showBossNames)
            {
                EnsureBossIINamesInSystemFile(messagePath);
                ApplyBossIINamesToMonsterCsv(dataPath);
            }
        }

        /// <summary>
        /// Ensures the system_en.txt file has the "II" boss name entries
        /// </summary>
        private void EnsureBossIINamesInSystemFile(string messagePath)
        {
            string systemFilePath = Path.Combine(messagePath, "system_en.txt");
            
            if (!File.Exists(systemFilePath))
            {
                return;
            }

            var lines = File.ReadAllLines(systemFilePath).ToList();
            var requiredEntries = new Dictionary<string, string>
            {
                { "MSG_MON_NAME_125", "Lich II" },
                { "MSG_MON_NAME_126", "Marilith II" },
                { "MSG_MON_NAME_127", "Kraken II" },
                { "MSG_MON_NAME_128", "Tiamat II" }
            };

            var existingEntries = new HashSet<string>();
            
            // Check which entries already exist
            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts.Length >= 1 && requiredEntries.ContainsKey(parts[0]))
                {
                    existingEntries.Add(parts[0]);
                }
            }

            // Add missing entries
            bool modified = false;
            foreach (var entry in requiredEntries)
            {
                if (!existingEntries.Contains(entry.Key))
                {
                    lines.Add($"{entry.Key}\t{entry.Value}");
                    modified = true;
                }
            }

            if (modified)
            {
                File.WriteAllLines(systemFilePath, lines);
            }
        }

        /// <summary>
        /// Updates monster.csv to use "II" boss name references for refight bosses
        /// </summary>
        private void ApplyBossIINamesToMonsterCsv(string dataPath)
        {
            string monsterCsvPath = Path.Combine(dataPath, "monster.csv");
            
            if (!File.Exists(monsterCsvPath))
            {
                return;
            }

            // Map refight boss monster IDs to their "II" name references
            var bossIINameMappings = new Dictionary<int, string>
            {
                { 121, "MSG_MON_NAME_125" }, // Lich II
                { 123, "MSG_MON_NAME_126" }, // Marilith II
                { 125, "MSG_MON_NAME_127" }, // Kraken II
                { 127, "MSG_MON_NAME_128" }  // Tiamat II
            };

            var lines = File.ReadAllLines(monsterCsvPath).ToList();
            bool modified = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("id,")) continue; // Skip header

                var parts = lines[i].Split(',');
                if (parts.Length < 2) continue;

                if (int.TryParse(parts[0], out int monsterId) && bossIINameMappings.ContainsKey(monsterId))
                {
                    // Update the name field (second column) with the "II" boss name reference
                    string correctNameRef = bossIINameMappings[monsterId];
                    if (parts[1] != correctNameRef)
                    {
                        parts[1] = correctNameRef;
                        lines[i] = string.Join(",", parts);
                        modified = true;
                    }
                }
            }

            if (modified)
            {
                File.WriteAllLines(monsterCsvPath, lines);
            }
        }
    }
}