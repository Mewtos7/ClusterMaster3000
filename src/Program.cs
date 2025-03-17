using ClusterMaster3000.classes.helper;
using ClusterMaster3000.classes.provider.database;
using ClusterMaster3000.classes.provider.platform;


namespace ClusterMaster3000
{
    public class Program
    {
        private readonly SqliteDatabase sqliteDatabase = new SqliteDatabase();
        private readonly HetznerServices hetznerServices = new HetznerServices();
        private readonly Cryptography cryptography = new Cryptography();
        private readonly JsonMapping jsonMapping = new JsonMapping();
        private readonly AppConfiguration appConfiguration = new AppConfiguration();

        static async Task Main(string[] args)
        {
            var p = new Program();
            p.InitializeClusterOrchestrator();
            await p.CreateServer();
        }
        private void InitializeClusterOrchestrator()
        {
            sqliteDatabase.CreateNewDatabaseIfNotExists();
            sqliteDatabase.CreateNewClusterMemberServerTableIfNotExists();
            if (appConfiguration.EncryptionKey == null)
            {
                var encryptionKey = cryptography.CreateEncryptionKey();
                var convertedEncryptionKey = Convert.ToBase64String(encryptionKey);
                appConfiguration.AddConfigurationinJson("ENCRYPTIONKEY", convertedEncryptionKey);
            }
        }

        private async Task CreateServer()
        {
            //Create server and save into database
            var sshKeys = cryptography.GenerateSshKeyPair();
            var sshKeyId = await hetznerServices.CreatePublicSshKey(sshKeys.PublicKey.ToString());
            var createdServerResponse = await hetznerServices.CreateServer(sshKeyId);
            var mappedClusterMemberServer = jsonMapping.MapServerFieldsToClusterMemberServer(createdServerResponse);
            sqliteDatabase.InsertNewClusterMemberServerRecord(mappedClusterMemberServer);

            //Clear sensitive data
            //mappedClusterMemberServer.SshPrivateKey = "";
            //sshKeys.Clear();

        }
    }
}
