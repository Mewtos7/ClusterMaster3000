using ClusterMaster3000.clusterMasterService.models;
using ClusterMaster3000.clusterMasterService.helper;
using ClusterMaster3000.clusterMasterService.common.provider;
using ClusterMaster3000.clusterMasterService.common.provider.database;
using ClusterMaster3000.classes.provider.platform;

namespace ClusterMaster3000.clusterMasterService
{
    public class ClusterMasterService
    {

        private readonly SqliteDatabaseProvider sqliteDatabase = new SqliteDatabaseProvider();
        
        public async Task ExecuteClusterMasterService()
        {
            InitializeClusterOrchestrator();
            await CreateServer();
        }

        private void InitializeClusterOrchestrator()
        {
            //Load config and create database structure
            //TODO: Inject Config and make reload mechanism for config if possible
            AppConfiguration appConfiguration = new AppConfiguration();
            sqliteDatabase.CreateNewDatabaseIfNotExists();
            sqliteDatabase.CreateNewClusterMemberServerTableIfNotExists();
            sqliteDatabase.CreateNewSshKeyTableIfNotExists();
            if (appConfiguration.EncryptionKey == null)
            {
                var encryptionKey = Cryptography.CreateEncryptionKey();
                var convertedEncryptionKey = Convert.ToBase64String(encryptionKey);
                appConfiguration.AddConfigurationInJson("ENCRYPTIONKEY", convertedEncryptionKey);
            }
        }

        private async Task CreateServer()
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            Cryptography cryptography = new Cryptography();
            HetznerServicesProvider hetznerServices = new HetznerServicesProvider();

            //Create ssh key and save to db
            var sshKeys = cryptography.GenerateSshKeyPair();
            await hetznerServices.CreatePublicSshKey(sshKeys);
            sqliteDatabase.InsertNewSshKeyRecord(sshKeys);

            //Create server and save to db, update ssh record
            var createdServerResponse = await hetznerServices.CreateServer(sshKeys.KeyName);
            var mappedClusterMemberServer = JsonMapping.MapServerFieldsToClusterMemberServer(createdServerResponse);
            sqliteDatabase.InsertNewClusterMemberServerRecord(mappedClusterMemberServer);
            sqliteDatabase.UpdateSshKeyRecord(sshKeys, mappedClusterMemberServer.ServerId);

        }
    }
}
