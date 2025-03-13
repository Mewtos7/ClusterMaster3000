using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SshKeyGenerator;

namespace ClusterMaster3000.classes.helper
{
    class Cryptography
    {
        public Dictionary<string, string> GenerateSshKeyPair()
        {
            using (var keygen = new SshKeyGenerator.SshKeyGenerator(2048))
            {
                var publicKey = keygen.ToRfcPublicKey();
                var privateKey = keygen.ToPrivateKey();

                var sshKeys = new Dictionary<string, string>
                {
                    { "public", publicKey },
                    { "private", privateKey }
                };

                return sshKeys;
            }
        }
    }
}
