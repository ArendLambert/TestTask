using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public static class LetterHelper
    {
        private const string vowel = "aeiouyуеаоэяию";
        private const string consonants = "bcdfghjklmnpqrstvwxzйцкнгшщзхфвпрлджчсмтб";

        public static bool IsConsonants(LetterStats letter)
        {
            return consonants.Contains((letter.Letter.ToLower())[0]);
        }
        public static bool IsVowel(LetterStats letter)
        {
            return vowel.Contains((letter.Letter.ToLower())[0]);
        }
        public static bool IsLetter(char c)
        {
            return vowel.Contains(c.ToString().ToLower()) || consonants.Contains(c.ToString().ToLower());
        }
    }
}
