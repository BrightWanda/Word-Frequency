using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFrequency
{
    class Program
    {

        // This will discard digits 
        private static char[] delimiters_no_digits = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', ' ', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        ///  Tokenizes a text into an array of words, using the improved
        ///  tokenizer with the discard-digit option.
        /// </summary>
        /// <param name="text"> the text to tokenize</param>
        private static string[] Tokenize(string text)
        {
            string[] tokens = text.Split(delimiters_no_digits, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                // Change token only when it starts and/or ends with "'" and  
                // it has at least 2 characters. 

                if (token.Length > 1)
                {
                    if (token.StartsWith("'") && token.EndsWith("'"))
                        tokens[i] = token.Substring(1, token.Length - 2); // remove the starting and ending "'" 

                    else if (token.StartsWith("'"))
                        tokens[i] = token.Substring(1); // remove the starting "'" 

                    else if (token.EndsWith("'"))
                        tokens[i] = token.Substring(0, token.Length - 1); // remove the last "'" 
                }
            }

            return tokens;
        }

        /// <summary>
        ///  Make a string-integer dictionary out of an array of words.
        /// </summary>
        /// <param name="words"> the words out of which to make the dictionary</param>
        /// <returns> a string-integer dictionary</returns>
        private static Dictionary<string, int> ToStrIntDict(string[] words)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (string word in words)
            {
                // if the word is in the dictionary, increment its freq. 
                if (dict.ContainsKey(word))
                {
                    dict[word]++;
                }
                // if not, add it to the dictionary and set its freq = 1 
                else
                {
                    dict.Add(word, 1);
                }
            }

            return dict;
        }

        /// <summary>
        ///  Sort a string-int dictionary by its entries' values.
        /// </summary>
        /// <param name="strIntDict"> a string-int dictionary to sort</param>
        /// <returns> a string-integer dictionary sorted by integer values</returns>
        private static Dictionary<string, int> ListWordsByFreq(Dictionary<string, int> strIntDict)
        {
            // Copy keys and values to two arrays 
            string[] words = new string[strIntDict.Keys.Count];
            strIntDict.Keys.CopyTo(words, 0);

            int[] freqs = new int[strIntDict.Values.Count];
            strIntDict.Values.CopyTo(freqs, 0);

            //Sort by freqs: it sorts the freqs array, but it also rearranges 
            //the words array's elements accordingly (not sorting) 
            Array.Sort(freqs, words);

            //reverse both arrays 
            Array.Reverse(freqs);
            Array.Reverse(words);

            //Copy freqs and words to a new Dictionary<string, int> 
            Dictionary<string, int> dictByFreq = new Dictionary<string, int>();

            for (int i = 0; i < (freqs.Length > 10 ? 10 : freqs.Length); i++)
            {
                dictByFreq.Add(words[i], freqs[i]);
            }

            return dictByFreq;
        }

        static void Main(string[] args)
        {
            Begin:

            Console.WriteLine("Please Enter The Path To Your Text File: ");
            string filePath = Console.ReadLine();

            try
            {
                string fileText = System.IO.File.ReadAllText(@filePath);
                string[] words = Tokenize(fileText);

                if (words.Length > 0)
                {
                    // Make a string-int dictionary out of the array of words  
                    Dictionary<string, int> dict = ToStrIntDict(words);

                    // Sort dict by values 
                    dict = ListWordsByFreq(dict);

                    foreach (KeyValuePair<string, int> entry in dict)
                        Console.WriteLine(string.Format("{0} [{1}]", entry.Key, entry.Value));
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Path format entered is not allowed.\n");
                goto Begin;
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine("File does not exist from path provided\n");
                goto Begin;
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("\nPress any key to exit.");
            System.Console.ReadKey();

        }
    }
}


/*using (var mappedFile1 = MemoryMappedFile.CreateFromFile(filePath))
{
    using (Stream mmStream = mappedFile1.CreateViewStream())
    {
        using (StreamReader sr = new StreamReader(mmStream, ASCIIEncoding.ASCII))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                var lineWords = line.Split(' ');
            }
        }  
    }
}*/
