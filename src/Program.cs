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
        }

        private async Task CreateServer()
        {
            //Create server and save into database
            var sshKeys = cryptography.GenerateSshKeyPair();
            var sshKeyId = await hetznerServices.DepositPublicSshKey(sshKeys["public"]);
            var createdServerResponse = await hetznerServices.CreateServer(sshKeyId);
            var mappedClusterMemberServer = jsonMapping.MapServerFieldsToClusterMemberServer(createdServerResponse, sshKeys["private"]);
            sqliteDatabase.InsertNewClusterMemberServerRecord(mappedClusterMemberServer);

            //Clear sensitive data
            mappedClusterMemberServer.SshPrivateKey = "";
            sshKeys.Clear();

        }
    }
}
