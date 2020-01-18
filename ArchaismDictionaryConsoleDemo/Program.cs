using System;
using System.Drawing;
using Tesseract;
using System.Data;

namespace ArchaismDictionaryConsoleDemo
{
    class Program
    {
        public static string[,] dataBase = { { "архаизъм", "дума със старинен произход" }, { "игото", "робството" } };

        static void Main(string[] args)
        {
            string input;
            string outputImage;
            Console.WriteLine("Bulgarian Archaism Dictionary ");
            input = Console.ReadLine();

            outputImage = OCR(input);
            outputImage = outputImage.ToLower();

            Console.Write("\n" + FindWords(outputImage));
            Console.ReadKey();
        }

        public static string FindWords(string input)
        {
            string[] words = new string[99];
            string final = "Не намерихме дума, която да съвпада";

            words = SplitIntoWords(input); 

            foreach(string word in words)
            {
                for(int i = 0; i < dataBase.Length/2; i++)
                {
                    if(word == dataBase[i, 0])
                    {
                        final = dataBase[i, 0] + " - " + dataBase[i, 1] + ".";
                    }
                    else
                    {
                        char[] wordOne = word.ToCharArray();
                        char[] wordTwo = dataBase[i, 0].ToCharArray();

                        if(wordOne.Length > 4 && wordTwo.Length > 4)
                        {
                            int difference = 0;

                            if (wordOne.Length < wordTwo.Length)
                            {
                                for(int j = 0; j < wordOne.Length; j++)
                                {
                                    if(wordOne[j] != wordTwo[j])
                                    {
                                        difference++;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < wordTwo.Length; j++)
                                {
                                    if (wordOne[j] != wordTwo[j])
                                    {
                                        difference++;
                                    }
                                }
                            }

                            if(difference < 3)
                            {
                                final = dataBase[i, 0] + " - " + dataBase[i, 1] + ".";
                            }
                        }
                    }
                }
            }

            return final;
        }

        public static string[] SplitIntoWords(string input)
        {
            char[] symbolSplit = { ',', ' ', '\n', '.', ';'};

            string[] words = input.Split(symbolSplit);
            return words;
        }

        public static string OCR(string inputImage)
        {
            Bitmap img = new Bitmap(inputImage);
            TesseractEngine engine = new TesseractEngine("./tessdata", "bul", EngineMode.Default);

            Page page = engine.Process(img, PageSegMode.Auto);
            string text = page.GetText();

            return text;
        }
    }
}
