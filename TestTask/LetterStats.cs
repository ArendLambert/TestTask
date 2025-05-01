using System.Linq;

namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public class LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter { get; private set; }

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count { get; private set; } = 0;

        public LetterStats(string letter)
        {
            Letter = letter;
        }

        public void IncreaseCount()
        {
            Count++;
        }
    }
}
