using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace DictionaryAttackNTLM
{
    class Program
    {
        const int CHARS_IN_HASH = 32;
        const string PASSWORD_FILE_PATH = "FoundPassword.txt";
        static void Main(string[] args)
        {

            //Add links to the array to add dictionaries
            string[] DICTIONARIES = new string[] { "https://raw.githubusercontent.com/mikejakobsen/dictionary-attack/master/10_million_password_list_top_1000000.txt",
                 "https://raw.githubusercontent.com/mikejakobsen/dictionary-attack/master/10_million_password_list_top_1000000.txt"};
            string foundPassword = "Not found";
            bool isFound = false;
            bool validInput;
            const string NTLM_HASH = "NTLM";
            const string MD5_HASH = "MD5";
            const string HASH_TO_ATTACK = "45e47406582741ad9fac8b0fa661b4a3";
            const string INPUT_FILE_NAME = "AttackHash.txt";
            const string HASH_MENU = "Select whether your hash is MD5 or NTLM\n" +
                "1. MD5\n" +
                "2. NTLM\n";
            
            int userChoice;
            string hashType = MD5_HASH;
            
            string inputHash;
            Stopwatch timer = new Stopwatch();

            //Reading from files

            if (File.Exists(INPUT_FILE_NAME))
            {
                
                Console.WriteLine("AttackHash file found, reading content.");
                string[] lines = File.ReadAllLines("./attackHash.txt");
                inputHash = lines[0].ToUpper();
                Console.WriteLine("Content of file: " + inputHash);
                Console.WriteLine();

                if (lines[0].Length != CHARS_IN_HASH)
                {
                    Console.WriteLine("Hash in attackHash file invalid. Hashes must be 32 character long. Close the program and edit the txt file or continue and enter it manually.");
                    Console.WriteLine("Press enter to continue.");
                    Console.ReadKey();
                   
                    inputHash = GetUserInput();
                    Console.WriteLine();
                    
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(INPUT_FILE_NAME))
                {
                    writer.WriteLine("Replace this line with a hash");
                }
                Console.WriteLine();
                inputHash = GetUserInput();
                Console.WriteLine();
            }

            Console.WriteLine(HASH_MENU);
            validInput = int.TryParse(Console.ReadLine(), out userChoice);


            while (!validInput)
            {
                Console.WriteLine("Invalid Input, try again.");
                Console.WriteLine();
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine(HASH_MENU);
                validInput = int.TryParse(Console.ReadLine(), out userChoice);

            }



            switch (userChoice)
            {
                case 1:
                    hashType = MD5_HASH;
                    break;
                case 2:
                    hashType = NTLM_HASH;
                    break;

                default:
                    validInput = false;
                    break;
            }



            //List

            List<string> hashList = new List<string>();

            HashController hasher;

            for (int i = 0; i < DICTIONARIES.Length; i++)
            {

                Console.WriteLine("Searching in dictionary {0}", i+1);
                timer.Start();
                Console.WriteLine();

                string[] currentDictionary = GetDictionaryFromURL(DICTIONARIES[i]);
                //Console.WriteLine(currentDictionary.Length);

                for (int x = 0; x < currentDictionary.Length; x++)
                {
                    //Console.WriteLine("In second for loop");
                    hasher = new HashController(currentDictionary[x], hashType);
                   // Console.WriteLine(currentDictionary.Length);
                    string currentHash = hasher.Hash;

                    //Console.WriteLine("Current Hash" + currentHash);
                    if (currentHash == HASH_TO_ATTACK.ToUpper())
                    {
                        Console.WriteLine("Dictionary attack successful!");
                        Console.WriteLine();
                        foundPassword = currentDictionary[x];
                        isFound = true;
                        
                    }
                    
                    
                }


                if (isFound)
                {
                    timer.Stop();
                    break;
                }

            }
            if (!isFound)
            {
                Console.WriteLine("Dictionary attack unsuccessfull");
                Console.WriteLine("Password not found");
            }

            if (isFound)
            {
                Console.WriteLine("Password is :");
                Console.WriteLine(foundPassword);
                WriteToTxt(foundPassword);
            }
            double elapsedTime = timer.ElapsedMilliseconds;
            elapsedTime = elapsedTime / 1000;


            Console.WriteLine($"Execution Time: {elapsedTime} Seconds");

            Console.ReadKey();




        }

        //Retrieves list of words from online and returns an array
        static string[] GetDictionaryFromURL(string url)
        {
            string dictionaryTemp;
            using (WebClient client = new WebClient())
            {
                dictionaryTemp = client.DownloadString(url);
            }
            string[] dictionaryArray = dictionaryTemp.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            
            return dictionaryArray;
        }

        static string GetUserInput()
        {

            bool invalidInput = true;
            string hash = string.Empty;


            while (hash.Length != CHARS_IN_HASH)
            {
                Console.Clear();
                Console.WriteLine("Enter hash for dictionary attack (valid hashes are 32 character long): ");
                hash = Console.ReadLine().ToUpper();
                Console.WriteLine();


            }
            return hash;

        }

        static void WriteToTxt(string pass)
        {
            using (StreamWriter writer = new StreamWriter(PASSWORD_FILE_PATH))
            {
                writer.WriteLine(pass);
            }
        }
    }

}
