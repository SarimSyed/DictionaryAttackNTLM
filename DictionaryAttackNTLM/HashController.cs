using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Text;

namespace DictionaryAttackNTLM
{
    //String sent into static class shouls be able to be hashed and return the hashed value
    //Should compare returned Hash with the Hash we are looking for
    class HashController
    {
        const string NTLM_HASH = "NTLM";
        const string MD5_HASH = "MD5";
        private string sourceData, _hash;
        private byte[] tmpSource, tmpHash;


        public HashController(string attackHash, string hashType = MD5_HASH)
        {
            sourceData = attackHash;
            if (hashType == MD5_HASH)
            {

                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    tmpSource = System.Text.Encoding.ASCII.GetBytes(sourceData);
                    tmpHash = md5.ComputeHash(tmpSource);

                    _hash = ByteArrayToString(tmpHash);

                }


                
            }
            else
            {

            //Might not need the original string but it doesnt hurt to keep it
            
            tmpSource = ASCIIEncoding.ASCII.GetBytes(attackHash);
            //Compute hash based on source data.
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            _hash = ByteArrayToString(tmpHash);


            }


        }



        //Properties
        public string SourceData {

            get
            {
                return sourceData;
            }
            set
            {
                sourceData = value;
            }
        
        }

        public string Hash
        {
            get
            {
                return _hash;
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
