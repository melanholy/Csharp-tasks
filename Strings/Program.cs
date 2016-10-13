using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

namespace Strings
{
    class Program
    {
        static Random rand = new Random();

        static void Main()
        {
            var text = File.ReadAllLines("Text.txt");
            var words = WordsStatistics(text);
            var i = 0;
            foreach (var sentence in text)
                if (sentence.Length != 0 && char.IsLetter(sentence[sentence.Length - 1])&&i<=4)
                {
                    i++;
                    Console.WriteLine("В тексте есть предложения, разорванные переходом на новую строку");
                    Console.WriteLine("Например: " + sentence);

                }
            Console.WriteLine();
            Console.Write("Наиболее часто встречающиеся слова: ");
            foreach (var word in Max(words, 50)) Console.Write(word + ", ");
            Console.WriteLine();
            Console.WriteLine();
            text = RemovePrepositions(text);
            var sentences = text
                .SelectMany(x => x.TrimStart().Replace("“", "").Replace("”", "").ToLowerInvariant()
                .Split(new string[] { "… ", "—", ", ", ". ", "! ", "? ", "; ", ": ", "(", ")", "." }, System.StringSplitOptions.RemoveEmptyEntries)).ToArray();
            var bigrams = Bigrams(sentences);
            Console.Write("Наиболее часто встречающиеся биграммы: ");
            foreach (var word in Max(bigrams, 20)) Console.Write(word + ", ");
            Console.WriteLine();
            Console.WriteLine();
            IntelliSense(bigrams);
            var ngrams = Ngrams(sentences);
                        Console.Write("Наиболее часто встречающиеся 3-граммы: ");
            foreach (var word in Max(ngrams, 10)) Console.Write(word + ", ");
            Console.WriteLine();
            Console.WriteLine();
            IntelliSenseImrpoved(ngrams, bigrams);
        }

        static Dictionary<string, int> WordsStatistics(string[] text)
        {
            var words = new Dictionary<string, int>();
            var separators = new char[] { ' ', '\t', '\n', '\r', ',', '.', '!', '?', ';', ':', '(', ')', '-', '”', '“', '—', '…' };
            foreach (var word in text
                .SelectMany(z => z.ToLowerInvariant().Split(separators , StringSplitOptions.RemoveEmptyEntries))
                .ToArray())
                if (!words.ContainsKey(word)) words[word] = 1;
                else words[word]++;
            return words;
        }

        static string[] RemovePrepositions(string[] text)
        {
            var textW = text.Select(x => x.Replace(" a ", " ").Replace(" the ", " ").Replace(" to ", " ").Replace(" to,", ",")
                    .Replace("—the ", "—").Replace("“to ", "“")
                    .Replace(" to.", ".").Replace("—to ", "—").Replace("—a ", "—")
                    .Replace("mr.", "mr").Replace("mrs.", "mrs").Replace("“the ", "“")).ToArray();
            File.WriteAllLines("TextW.txt", textW);
            return textW;
        }

        static Dictionary<string, int> Bigrams(string[] sentences)
        {
            var bigrams = new Dictionary<string, int>();
            foreach (var sentence in sentences)
            {
                var words = sentence.Split(' ').ToArray();
                for (var t = 0; t < words.Length - 1; t++)
                {
                    var bigram = words[t] + " " + words[t + 1];
                    if (!bigrams.ContainsKey(bigram)) bigrams[bigram] = 1;
                    else bigrams[bigram]++;
                }
            }
            return bigrams;
        }

        static Dictionary<string, int> Ngrams(string[] sentences)
        {
            var ngrams = new Dictionary<string, int>();
            foreach (var sentence in sentences)
            {
                var words = sentence.Split(' ').ToArray();
                for (var t = 0; t < words.Length - 2; t++)
                {
                    var format = String.Format("{0} {1} {2}", words[t], words[t + 1], words[t + 2]);
                    if (!ngrams.ContainsKey(format)) ngrams[format] = 1;
                    else ngrams[format]++;
                }
            }
            return ngrams;
        }

        static string[] Max(Dictionary<string, int> dictionary, int count)
        {
            var max = new int[count];
            var maxw = new string[count];
            foreach (var something in dictionary)
                for (var i = 0; i < count; i++)
                    if (max[i] < something.Value)
                    {
                        max[i] = something.Value;
                        maxw[i] = something.Key;
                        break;
                    }
            foreach (var b in max)
                Console.Write(b + " ");
            return maxw;
        }

        static void IntelliSense(Dictionary<string, int> bigrams)
        {
            while (true)
            {
                string startWord = Console.ReadLine();
                if (startWord == "") break;
                Magic(startWord, 0);
                for (var i = 0; i < 9; i++)
                {
                    var maxKey = FindMaxKey(bigrams, startWord, 0);
                    if (maxKey != "") Magic(maxKey, 1);
                    else break;
                    startWord = maxKey.Split(' ')[1];
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void IntelliSenseImrpoved(Dictionary<string, int> ngrams, Dictionary<string, int> bigrams)
        {
            while (true)
            {
                string startWord = Console.ReadLine();
                if (startWord == "") break;
                Magic(startWord, 0);
                var maxKey = FindMaxKey(bigrams, startWord, 0);
                if (maxKey != "") Magic(maxKey, 1);
                else goto end;
                var startWords = startWord + " " + maxKey.Split(' ')[1];
                for (var i = 0; i < 8; i++)
                {
                    var b = false;
                    maxKey = FindMaxKey(ngrams, startWords, 1);
                    if (maxKey != "") Magic(maxKey, 2);
                    else
                    {
                        maxKey = FindMaxKey(bigrams, startWords.Split(' ')[1], 0);
                        b = true;
                        if (maxKey != "") Magic(maxKey, 1);
                        else break;
                    }
                    if (b==false) startWords = maxKey.Split(' ')[1] + " " + maxKey.Split(' ')[2];
                    else startWords = maxKey;
                }
                end:
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static string FindMaxKey(Dictionary<string, int> grams, string startWord, int type)
        {
            string max1Key = "", max2Key = "", max3Key = "", maxKey = "";
            int max1Val = 0, max2Val = 0, max3Val = 0;
            foreach (var ngram in grams)
                if (type == 0 && (ngram.Key.Split(' ')[0] == startWord && ngram.Value > max1Val)
                    || type == 1 && (ngram.Key.Split(' ')[0] + " " + ngram.Key.Split(' ')[1] == startWord && ngram.Value > max1Val))
                {
                    max3Key = max2Key;
                    max3Val = max2Val;
                    max2Key = max1Key;
                    max2Val = max1Val;
                    max1Val = ngram.Value;
                    max1Key = ngram.Key;
                }
            if (max1Key != "" || max2Key != "" || max3Key != "") 
                switch (rand.Next(3))
                {
                    case 0:
                        case0:
                        if (max1Key != "") maxKey = max1Key;
                        else goto case1;
                        break;
                    case 1:
                        case1:
                        if (max2Key != "") maxKey = max2Key;
                        else goto case2;
                        break;
                    case 2:
                        case2:
                        if (max3Key != "") maxKey = max3Key;
                        else goto case0;
                        break;
                }
            return maxKey;
        }

        static void Magic(string maxKey, int type)
        {
            foreach (var ch in maxKey.Split(' ')[type] + " ")
            {
                Console.Write(ch);
                Thread.Sleep(rand.Next(200));
            }
        }
    }
}