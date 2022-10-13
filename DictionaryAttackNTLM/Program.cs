using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DictionaryAttackNTLM
{
    class Program
    {
        static void Main(string[] args)
        {
            const string HASH_TO_ATTACK = "45e47406582741ad9fac8b0fa661b4a";


            //Get the dictionary content
            string dictionaryTemp;
            //below retrieves the first dictionary
            using (WebClient client = new WebClient())
            {
                dictionaryTemp = client.DownloadString("https://gist.githubusercontent.com/wchargin/8927565/raw/d9783627c731268fb2935a731a618aa8e95cf465/words");
            }

            //Splits words by newline
            string[] currentDictionary =dictionaryTemp.Split(new string[] { Environment.NewLine }, StringSplitOptions.None) ;

            string sourceData;
            byte[] tmpSource, tmpHash;

            
            for (int i = 0; i < currentDictionary.Length; i++)
            {
                tmpSource = ASCIIEncoding.ASCII.GetBytes(currentDictionary[i]);
                //Compute hash based on source data.
                tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            }
            
        }
    }
    static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }
}
