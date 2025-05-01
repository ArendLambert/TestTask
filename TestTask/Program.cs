using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.Extensions;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
            using (IReadOnlyStream inputStream1 = GetInputStream(args[0]))
            using (IReadOnlyStream inputStream2 = GetInputStream(args[1]))
            {
                IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
                IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

                RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
                RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

                PrintStatistic(singleLetterStats);
                PrintStatistic(doubleLetterStats);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            IDictionary<string, LetterStats> letterStatsDictionary = new Dictionary<string, LetterStats>();
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                if (!LetterHelper.IsLetter(c))
                {
                    continue;
                }                  
                IncStatistic(new LetterStats(c.ToString()), letterStatsDictionary);
            }

            return letterStatsDictionary.Values.ToList();
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            IDictionary<string, LetterStats> letterStatsDictionary = new Dictionary<string, LetterStats>();
            char previousChar = '\0';
            using (stream)
            {
                stream.ResetPositionToStart();
                while (!stream.IsEof)
                {
                    char c = stream.ReadNextChar();
                    if (char.ToUpper(c) == char.ToUpper(previousChar) && LetterHelper.IsLetter(c))
                    {
                        IncStatistic(new LetterStats(c.ToString().ToUpper() + previousChar.ToString().ToUpper()), letterStatsDictionary);                     
                    }
                    previousChar = c;
                }
            }
            return letterStatsDictionary.Values.ToList();
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            switch (charType)
            {
                case CharType.Consonants:
                    letters.RemoveAll(x => LetterHelper.IsConsonants(x));
                    return;
                case CharType.Vowel:
                    letters.RemoveAll(x => LetterHelper.IsVowel(x));
                    return;
            }

        }        

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            int count = 0;
            foreach (LetterStats letter in letters.OrderBy(x => x.Letter))
            {
                Console.WriteLine($"{letter.Letter} : {letter.Count}");
                count += letter.Count;
            }
            Console.WriteLine($"ИТОГО: {count}");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(LetterStats letterStats, IDictionary<string, LetterStats> letterStatsDictionary)
        {
            if(letterStatsDictionary.TryGetValue(letterStats.Letter, out LetterStats letter))
            {
                letter.IncreaseCount();
            }
            else
            {
                letterStatsDictionary.Add(letterStats.Letter, letterStats);
                letterStats.IncreaseCount();
            }
        }
    }
}
