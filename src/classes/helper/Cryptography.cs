using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SshKeyGenerator;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ClusterMaster3000.classes.helper
{
    class Cryptography
    {
        public struct SshKeyPair
        {
            public string PublicKey { get; set; }
            public string EncryptedPrivateKey { get; set; }
            public string IV { get; set; }
            public string KeyName { get; set; }
        }

        public struct EncryptedResult
        {
            public string EncryptedText { get; set; }
            public byte[] IV { get; set; }
        }

        public SshKeyPair GenerateSshKeyPair()
        {
            using (var keygen = new SshKeyGenerator.SshKeyGenerator(2048))
            {
                AppConfiguration config = new AppConfiguration();

                var publicSshKey = keygen.ToRfcPublicKey();
                var privateSshKey = keygen.ToPrivateKey();

                var encryptionKey = Convert.FromBase64String(config.EncryptionKey ?? throw new KeyNotFoundException("No encryption key found"));
                var encryptedPrivateSshKey = EncryptText(privateSshKey, encryptionKey);

                var keyName = Guid.NewGuid().ToString();

                var keyPair = new SshKeyPair
                {
                    KeyName = keyName,
                    PublicKey = publicSshKey,
                    EncryptedPrivateKey = encryptedPrivateSshKey.EncryptedText,
                    IV = Convert.ToBase64String(encryptedPrivateSshKey.IV)
                };

                return keyPair;
            }
        }

        public EncryptedResult EncryptText(string text, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateIV();
                aes.Key = key;

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(Encoding.Unicode.GetBytes(text));
                    cs.FlushFinalBlock();

                    var encryptedText = Convert.ToBase64String(ms.ToArray());

                    var result = new EncryptedResult
                    {
                        EncryptedText = encryptedText,
                        IV = aes.IV
                    };
                    return result;

                }
            }
        }

        //TODO: Not used currently
        public string DecryptText(string encryptedText, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                var convertedEcryptedText = Convert.FromBase64String(encryptedText);

                using (var ms = new MemoryStream(convertedEcryptedText))
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.Unicode))
                {
                    var decryptedText = sr.ReadToEnd();
                    return decryptedText;
                }
            }
        }

        public byte[] CreateEncryptionKey()
        {
            var guid = Guid.NewGuid().ToString();
            var key = SHA256.HashData(Encoding.UTF8.GetBytes(guid));

            return key;
        }
    }
}
