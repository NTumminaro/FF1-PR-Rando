using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FF1_PRR.Common
{
    public class ConfigurationManager
    {
        private const string DefaultVisualFlags = "0000"; // Updated to match 4-character format
        private const string DefaultFlags = "0000000010"; // Updated to match all dropdowns at index 0

        public class RandomizerConfig
        {
            public string Seed { get; set; }
            public string RandoFlags { get; set; }
            public string VisualFlags { get; set; }
            public string FF1PRFolder { get; set; }
            public List<string> SpriteSelections { get; set; } = new List<string>();
        }

        public static string ConvertFlagsToString(CheckBox[] boxes)
        {
            int number = 0;
            for (int i = 0; i < boxes.Length; i++)
            {
                number += boxes[i].Checked ? (int)Math.Pow(2, i) : 0;
            }
            return ConvertIntToChar(number);
        }

        public static void ApplyFlagsToCheckboxes(string flagSegment, CheckBox[] boxes)
        {
            int number = ConvertCharToInt(flagSegment[0]);
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].Checked = (number & (1 << i)) != 0;
            }
        }

        public static string ConvertIntToChar(int number)
        {
            if (number >= 0 && number <= 9)
                return number.ToString();
            if (number >= 10 && number <= 35)
                return Convert.ToChar(55 + number).ToString();
            if (number >= 36 && number <= 61)
                return Convert.ToChar(61 + number).ToString();
            if (number == 62) return "!";
            if (number == 63) return "@";
            return "";
        }

        public static int ConvertCharToInt(char character)
        {
            if (character >= '0' && character <= '9')
                return character - '0';
            if (character >= 'A' && character <= 'Z')
                return character - 'A' + 10;
            if (character >= 'a' && character <= 'z')
                return character - 'a' + 36;
            if (character == '!') return 62;
            if (character == '@') return 63;
            return 0;
        }

        public static RandomizerConfig GetDefaultConfig()
        {
            return new RandomizerConfig
            {
                Seed = (DateTime.Now.Ticks % 2147483647).ToString(),
                RandoFlags = DefaultFlags,
                VisualFlags = DefaultVisualFlags,
                FF1PRFolder = "",
                SpriteSelections = new List<string>()
            };
        }

        public static void GenerateNewSeed(RandomizerConfig config)
        {
            config.Seed = (DateTime.Now.Ticks % 2147483647).ToString();
        }
    }
}