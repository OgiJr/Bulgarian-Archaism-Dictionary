using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using UIKit;
using Xamarin.Essentials;

namespace ArchaismDictionaryIOSApp
{
    public class Application
    {
        #region NetworkVariables
        /// <summary>
        /// A matrix string [wordName, wordDefinition]
        /// </summary>
        private static string[,] dataBase;
        public bool isConnected { get; set; }
        /// <summary>
        /// How many words there are in the JSON file
        /// </summary>
        private int wordCount;
        #endregion

        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
            Application app = new Application();
            app.DictionaryManager();
            FindWordInDictionary("Заптие");
        }

        #region DictionaryManager
        /// <summary>
        /// Searches for a word inside of the Network manager's database matrix
        /// </summary>
        /// <param name="input">This is the input word which is searched for inside of the matrix</param>
        /// <returns></returns>
        public static string FindWordInDictionary(string input)
        {
            string final = string.Empty;

            input = input.ToLower();

            string[] inputWords = input.Split("\n");

            foreach (string word in inputWords)
            {
                for (int i = 0; i < dataBase.Length / 2; i++)
                {
                    if (word == dataBase[i, 0])
                    {
                        final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                    }
                    else
                    {
                        if (word.Length > 4 && dataBase[i, 0].Length > 4)
                        {
                            if (word.Length < dataBase[i, 0].Length)
                            {
                                if (dataBase[i, 0].Contains(word) == true)
                                {
                                    final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                                }
                            }
                            else
                            {
                                if (dataBase[i, 0].Contains(word) == true)
                                {
                                    final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                                }
                            }
                        }
                    }
                }
            }
            return final;
        }

        /// <summary>
        /// Reads the JSON file from the internet and assigns its values to the dictionary matrix
        /// </summary>
        private void DictionaryManager()
        {
            string rawJSON;
            const string fileName = "dictionary.json";
            string jsonRead;

            WebClient client = new WebClient();
            jsonRead = client.DownloadString("http://archaismdictionary.bg/json_manager.php"); ;

            File.WriteAllText(FileSystem.AppDataDirectory + fileName, jsonRead);

            rawJSON = File.ReadAllText(FileSystem.AppDataDirectory + fileName);

            var list = JsonConvert.DeserializeObject<Dictionary.JSONClass>(rawJSON);

            wordCount = list.Property1[2].data.Length;

            dataBase = new string[wordCount, 2];

            for (int i = 0; i < wordCount; i++)
            {
                dataBase[i, 0] = list.Property1[2].data[i].word;
                dataBase[i, 1] = list.Property1[2].data[i].definition;
            }
        }
       #endregion
    }
}