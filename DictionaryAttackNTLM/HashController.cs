using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Text;

namespace DictionaryAttackNTLM
{
    //String sent into static class shouls be able to be hashed and return the hashed value
    class HashController
    {
        private string sourceData;
        private byte[] tmpSource, tmpHash;

        public HashController(string attackHash)
        {
            //Might not need the original string but it doesnt hurt to keep it
            sourceData = attackHash;
            tmpSource = ASCIIEncoding.ASCII.GetBytes(attackHash);
            //Compute hash based on source data.
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
        }

        //Readonly
        public string SourceData {

            get
            {
                return sourceData;
            }
        
        }

        public string Hash
        {
            get
            {
                return ByteArrayToString(tmpHash);
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
}
