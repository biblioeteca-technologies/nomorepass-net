using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace nomorepass_net
{
    public class NmpCrypto
    {

        /// <summary>
        /// Method for deriving a key and initialization vector (IV) from a password and salt value.                                            <para></para> 
        /// It uses the MD5 hash function to repeatedly hash the password and salt together.                                                    <br></br>
        /// The resulting bytes are concatenated together until the desired key and IV lengths are reached.                                     <br></br>
        /// The key and IV are then returned as a single byte array, with the key bytes at the beginning and the IV bytes following.            <br></br>
        /// This method is used in the encryption and decryption process to generate unique key and IV for each encryption.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="keyLength"></param>
        /// <param name="ivLength"></param>
        public byte[] DeriveKeyAndIv(string password, byte[] salt, int keyLength, int ivLength)
        {
            byte[] d = new byte[0];
            byte[] d_i = new byte[0];
            while (d.Length < keyLength + ivLength)
            {
                d_i = MD5.Create().ComputeHash(d_i.Concat(Encoding.UTF8.GetBytes(password)).Concat(salt).ToArray());
                d = d.Concat(d_i).ToArray();
            }
            return d;
        }


        /// <summary>
        /// AES encryption - Encrypt a string of text using the AES (Advanced Encryption Standard) algorithm with a randomly generated salt.                                                <para></para> 
        /// It first creates a random 8-byte salt, then it derives the key and initialization vector using the password and the salt using the DeriveKeyAndIv method.                       <br></br>
        /// Then it creates an instance of Aes object, sets the key, iv, and cipher mode and creates an encryptor using this Aes object.                                                    <br></br>
        /// Then it encrypts the given text using the encryptor and concatenates it with "Salted__" and the salt. Finally, it returns the encrypted text as a base64 encoded string.        <br></br>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Encrypt(string text, string password)
        {
            Random random = new Random();

            byte[] salt = Enumerable.Range(0, 8).Select(i => (byte)random.Next(256)).ToArray();
            byte[] keyAndIv = DeriveKeyAndIv(password, salt, 32, 16);

            byte[] key = keyAndIv.Take(32).ToArray();
            byte[] iv = keyAndIv.Skip(32).Take(16).ToArray();

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] res = Encoding.UTF8.GetBytes("Salted__").Concat(salt).ToArray();
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetByteCount(text));
                    res = res.Concat(encryptedBytes).ToArray();
                    return Convert.ToBase64String(res);
                }
            }
        }

        /// <summary>
        /// Decrypt fuction - Takes in a string of text and a password as input, and uses the Advanced Encryption Standard (AES) algorithm to decrypt the text.                                   <para></para>
        /// The method takes in a string "text" which is the encrypted text and a string "pass" which is the password used to encrypt the text.                                                   <br></br>
        /// The encrypted text is converted from a base64 string to a byte array.                                                                                                                 <br></br>
        /// The first 8 bytes of the byte array is the salt used to encrypt the text, which is then used to derive a key and initialization vector (IV) using the DeriveKeyAndIv method.          <br></br>
        /// The key and IV are used to create an AES (Advanced Encryption Standard) object and set the key, IV, and cipher mode.                                                                  <br></br>
        /// An ICryptoTransform object is then created using the AES object's CreateDecryptor() method.                                                                                           <br></br>
        /// This object is used to decrypt the encrypted bytes and the decrypted text is returned as a string.                                                                                    <br></br>
        /// </summary>
        public string Decrypt(string text, string pass)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] salt = input.Skip(8).Take(8).ToArray();
            byte[] keyAndIv = DeriveKeyAndIv(pass, salt, 32, 16);
            byte[] key = keyAndIv.Take(32).ToArray();
            byte[] iv = keyAndIv.Skip(32).Take(16).ToArray();

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedBytes = input.Skip(16).ToArray();
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    string decrypted = Encoding.UTF8.GetString(decryptedBytes);
                    return decrypted;
                }
            }
        }

    }
}
