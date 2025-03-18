using System.Text;
using System.Text.RegularExpressions;
using ClusterMaster3000.clusterMasterService.helper;
using Renci.SshNet.Security;
using Renci.SshNet.Security.Cryptography;

namespace ClusterMaster3000Tests
{
    public class CryptographyTests
    {
        [Fact]
        public void CreateEncryptionKeyAndCheckIfNotEmpty()
        {
            // Arrange

            // Act
            var key = Cryptography.CreateEncryptionKey();

            // Assert
            Assert.NotEmpty(key);
        }

        [Fact]
        public void EncryptTextAndCheckIfOriginalStringIsNotEqualToEncryptedString()
        {
            // Arrange
            var text = "Hello World!";
            var key = Convert.FromBase64String("COfaVfgJtEnxzjSYa4x0FlXgfY2kqtZfdQ16nSp6Obo=");

            Cryptography cryptography = new Cryptography();

            // Act
            var result = cryptography.EncryptText(text, key);
            var encryptedText = result.EncryptedText;

            // Assert
            Assert.NotEqual(text, encryptedText);
        }

        [Fact]
        public void EncryptTextAndCheckIfEncryptedTextIsNotEmpty()
        {
            // Arrange
            var text = "Hello World!";
            var key = Convert.FromBase64String("COfaVfgJtEnxzjSYa4x0FlXgfY2kqtZfdQ16nSp6Obo=");

            Cryptography cryptography = new Cryptography();

            // Act
            var result = cryptography.EncryptText(text, key);
            var encryptedText = result.EncryptedText;

            // Assert
            Assert.NotEmpty(encryptedText);
        }

        [Fact]
        public void EncryptTextAndCheckIfAesIVIsNotEmpty()
        {
            // Arrange
            var text = "Hello World!";
            var key = Convert.FromBase64String("COfaVfgJtEnxzjSYa4x0FlXgfY2kqtZfdQ16nSp6Obo=");

            Cryptography cryptography = new Cryptography();

            // Act
            var result = cryptography.EncryptText(text, key);
            var aesIv = result.IV;

            // Assert
            Assert.NotEmpty(aesIv);
        }

        [Fact]
        public void EncryptTextAndDecryptTextAndCheckIfStringIsEqual()
        {
            // Arrange
            var text = "Hello World!";
            var key = Convert.FromBase64String("COfaVfgJtEnxzjSYa4x0FlXgfY2kqtZfdQ16nSp6Obo=");
            Cryptography cryptography = new Cryptography();

            // Act
            var encryptedText = cryptography.EncryptText(text, key);
            var decryptedText = cryptography.DecryptText(encryptedText.EncryptedText, key, encryptedText.IV);

            // Assert
            Assert.Equal(text, decryptedText);
        }

        [Fact]
        public void GenerateSshKeyPairAndCheckIfPublicKeyIsValidSshKey()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ENCRYPTIONKEY", "COfaVfgJtEnxzjSYa4x0FlXgfY2kqtZfdQ16nSp6Obo=");
            Cryptography cryptography = new Cryptography();

            // Act
            var sshKeyPair = cryptography.GenerateSshKeyPair();
            var publicKey = sshKeyPair.PublicKey;

            var sshKeyPattern = @"^(ssh-rsa|ssh-ed25519) [A-Za-z0-9+/=]+(\s.+)?$";
            var regexTest = Regex.IsMatch(publicKey, sshKeyPattern);

            var keyParts = publicKey.Split(' ');
            var testKeyPartsLenght = keyParts.Length >= 2;
            
            string keyData = keyParts[1];
            byte[] keyBytes = Convert.FromBase64String(keyData);
            var testKeyLenght = keyBytes.Length > 0;

            var testResults = false;
            if (regexTest && testKeyPartsLenght && testKeyLenght)
            {
                testResults = true;
            }

            // Assert
            Assert.True(testResults);
        }


        [Fact]
        public void GenerateSshKeyPairAndCheckIfEncryptedPrivateKeyIsEncrypted()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ENCRYPTIONKEY", "COfaVfgJtEnxzjSYa4x0FlXgfY2kqtZfdQ16nSp6Obo=");
            Cryptography cryptography = new Cryptography();

            // Act
            var sshKeyPair = cryptography.GenerateSshKeyPair();
            var privateKey = sshKeyPair.EncryptedPrivateKey;

            var containsNotPrivateKeyCharacteristics = (!privateKey.Contains("-----BEGIN OPENSSH PRIVATE KEY-----") || !privateKey.Contains("-----END OPENSSH PRIVATE KEY-----"));

            // Assert
            Assert.True(containsNotPrivateKeyCharacteristics);
        }

    }
}
