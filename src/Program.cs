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
        private readonly AppConfiguration appConfiguration = new AppConfiguration(); //TODO: Not optimal here, what is with reloading config when appsetting changes?

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
            sqliteDatabase.CreateNewSshKeyTableIfNotExists();
            if (appConfiguration.EncryptionKey == null)
            {
                var encryptionKey = cryptography.CreateEncryptionKey();
                var convertedEncryptionKey = Convert.ToBase64String(encryptionKey);
                appConfiguration.AddConfigurationinJson("ENCRYPTIONKEY", convertedEncryptionKey);
            }
        }

        private async Task CreateServer()
        {
            //Create ssh key and save to db
            var sshKeys = cryptography.GenerateSshKeyPair();
            await hetznerServices.CreatePublicSshKey(sshKeys);
            sqliteDatabase.InsertNewSshKeyRecord(sshKeys);
            

            //Create server and save to db
            var createdServerResponse = await hetznerServices.CreateServer(sshKeys.KeyName);
            var mappedClusterMemberServer = jsonMapping.MapServerFieldsToClusterMemberServer(createdServerResponse);
            sqliteDatabase.InsertNewClusterMemberServerRecord(mappedClusterMemberServer);
            sqliteDatabase.UpdateSshKeyRecord(sshKeys, mappedClusterMemberServer.ServerId);

        }
    }
}
