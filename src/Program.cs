using ClusterMaster3000.classes.helper;
using ClusterMaster3000.classes.provider.database;
using ClusterMaster3000.classes.provider.platform;


namespace ClusterMaster3000
{
    public class Program
    {
        private readonly SqliteDatabase sqliteDatabase = new SqliteDatabase();
        private readonly HetznerServices hetznerServices = new HetznerServices();

        static async Task Main(string[] args)
        {
            var p = new Program();
            p.InitializeClusterOrchestrator();
            await p.CreateFirstServer();
        }
        private void InitializeClusterOrchestrator()
        {
            sqliteDatabase.CreateNewDatabaseIfNotExists();
            sqliteDatabase.CreateNewClusterMemberServerTableIfNotExists();
        }

        private async Task CreateFirstServer()
        {
            //Create server and save into database
            var sshKey = await hetznerServices.CreateSshKey();
            var createdServer = await hetznerServices.CreateServer(sshKey);
            var clusterMemberServer = JsonMapping.MapCreateServerResponseToClusterMemberServer(createdServer);
            sqliteDatabase.InsertNewClusterMemberServerRecord(clusterMemberServer);
        }
    }
}
