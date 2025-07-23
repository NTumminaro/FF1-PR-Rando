using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FF1_PRR.Randomize
{
    public class PartyFiesta
    {
        private readonly Random random;
        private readonly string dataPath;

        public PartyFiesta(Random r1, string dataPath, PartyFiestaOptions options)
        {
            this.random = r1;
            this.dataPath = dataPath;
            
            if (options.EnablePartyFiesta)
            {
                ApplyPartyFiesta(options);
            }
        }

        private void ApplyPartyFiesta(PartyFiestaOptions options)
        {
            string initializeDataPath = Path.Combine(dataPath, "initialize_data.csv");
            
            if (!File.Exists(initializeDataPath))
            {
                return;
            }

            var lines = File.ReadAllLines(initializeDataPath).ToList();
            var partyComposition = GenerateRandomParty(options);
            
            // Update the initialize_data.csv with the random party
            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length < 3) continue;

                switch (parts[1])
                {
                    case "INIT_PARTY_1":
                        parts[2] = partyComposition[0].ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                    case "INIT_PARTY_2":
                        parts[2] = partyComposition[1].ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                    case "INIT_PARTY_3":
                        parts[2] = partyComposition[2].ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                    case "INIT_PARTY_4":
                        parts[2] = partyComposition[3].ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                    case "INIT_CORPS_1":
                        parts[2] = (partyComposition[0] > 0 ? 1 : 0).ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                    case "INIT_CORPS_2":
                        parts[2] = (partyComposition[1] > 0 ? 1 : 0).ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                    case "INIT_CORPS_3":
                        parts[2] = (partyComposition[2] > 0 ? 1 : 0).ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                    case "INIT_CORPS_4":
                        parts[2] = (partyComposition[3] > 0 ? 1 : 0).ToString();
                        lines[i] = string.Join(",", parts);
                        break;
                }
            }

            File.WriteAllLines(initializeDataPath, lines);
        }

        private int[] GenerateRandomParty(PartyFiestaOptions options)
        {
            var party = new int[4];
            var availableJobs = new List<int> { 1, 2, 3, 4, 5, 6 }; // Warrior, Thief, Monk, Red Mage, White Mage, Black Mage

            switch (options.PartyFiestaMode)
            {
                case PartyFiestaMode.RandomFull:
                    // 4 random jobs, duplicates allowed
                    for (int i = 0; i < 4; i++)
                    {
                        party[i] = availableJobs[random.Next(availableJobs.Count)];
                    }
                    break;

                case PartyFiestaMode.RandomNoDuplicates:
                    // 4 different jobs, no duplicates
                    var shuffledJobs = availableJobs.OrderBy(x => random.Next()).ToList();
                    for (int i = 0; i < 4; i++)
                    {
                        party[i] = shuffledJobs[i];
                    }
                    break;

                case PartyFiestaMode.RandomWithEmpty:
                    // 1-4 characters, with possible empty slots
                    int partySize = random.Next(1, 5); // 1 to 4 characters
                    var selectedJobs = availableJobs.OrderBy(x => random.Next()).Take(partySize).ToList();
                    
                    for (int i = 0; i < 4; i++)
                    {
                        if (i < selectedJobs.Count)
                        {
                            party[i] = selectedJobs[i];
                        }
                        else
                        {
                            party[i] = 0; // Empty slot
                        }
                    }
                    break;

                case PartyFiestaMode.SoloRun:
                    // Single random character
                    party[0] = availableJobs[random.Next(availableJobs.Count)];
                    party[1] = party[2] = party[3] = 0;
                    break;

                case PartyFiestaMode.DuoRun:
                    // Two random characters
                    var duoJobs = availableJobs.OrderBy(x => random.Next()).Take(2).ToList();
                    party[0] = duoJobs[0];
                    party[1] = duoJobs[1];
                    party[2] = party[3] = 0;
                    break;

                default:
                    // Default to normal party (Warrior, Thief, White Mage, Black Mage)
                    party[0] = 1; // Warrior
                    party[1] = 2; // Thief
                    party[2] = 5; // White Mage
                    party[3] = 6; // Black Mage
                    break;
            }

            return party;
        }
    }

    public class PartyFiestaOptions
    {
        public bool EnablePartyFiesta { get; set; }
        public PartyFiestaMode PartyFiestaMode { get; set; }
    }

    public enum PartyFiestaMode
    {
        None = 0,
        RandomFull = 1,           // 4 random jobs, duplicates allowed
        RandomNoDuplicates = 2,   // 4 different jobs, no duplicates  
        RandomWithEmpty = 3,      // 1-4 characters, with possible empty slots
        SoloRun = 4,             // Single character challenge
        DuoRun = 5               // Two character challenge
    }
}