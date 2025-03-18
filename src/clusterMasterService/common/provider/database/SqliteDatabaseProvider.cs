using System.Data.SQLite;
using ClusterMaster3000.clusterMasterService.models;
using ClusterMaster3000.clusterMasterService.helper;

namespace ClusterMaster3000.clusterMasterService.common.provider.database
{
    public class SqliteDatabaseProvider
    {
        //TODO: Handle Exceptions
        //TODO: Better using variables database and table names to decouple from the implementation
        private readonly string databaseName = "clusterMaster3000.db";
        private readonly string clusterMemberTable = "clusterMember";
        private readonly string sshKeyTable = "sshKeys";

        public void CreateNewDatabaseIfNotExists()
        {

            if (File.Exists(databaseName))
            {
                return;
            }
            string databasePath = $"Data Source={databaseName}";
            SQLiteConnection.CreateFile(databaseName);
        }

        //TODO: Handle Exceptions
        public void CreateNewClusterMemberServerTableIfNotExists()
        {
            string databasePath = $"Data Source={databaseName}";
            using (var connection = new SQLiteConnection(databasePath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $@"CREATE Table IF NOT EXISTS {clusterMemberTable} (
	                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
	                        ServerId TEXT NOT NULL, 
	                        ServerName TEXT NOT NULL, 
	                        PublicIpv6 TEXT, 
	                        Status TEXT NOT NULL, 
	                        ServerCreatedAt DATETIME NOT NULL, 
	                        EntryUpdatedAt DATETIME NOT NULL);";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        //TODO: Handle Exceptions
        public void CreateNewSshKeyTableIfNotExists()
        {
            string databasePath = $"Data Source={databaseName}";
            using (var connection = new SQLiteConnection(databasePath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $@"CREATE Table IF NOT EXISTS {sshKeyTable} (
	                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT,
	                        ServerId TEXT, 
	                        SshPrivateKey TEXT,
                            AesIv TEXT);";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertNewSshKeyRecord(Cryptography.SshKeyPair keyPair)
        {
            string databasePath = $"Data Source={databaseName}";
            using (var connection = new SQLiteConnection(databasePath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $@"INSERT INTO {sshKeyTable} (SshPrivateKey,AesIv, Name) 
                        VALUES (@SshPrivateKey, @AesIv, @Name);";

                command.Parameters.AddWithValue("@SshPrivateKey", keyPair.EncryptedPrivateKey);
                command.Parameters.AddWithValue("@AesIv", keyPair.IV);
                command.Parameters.AddWithValue("@Name", keyPair.KeyName);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UpdateSshKeyRecord(Cryptography.SshKeyPair keyPair, string serverId)
        {
            string databasePath = $"Data Source={databaseName}";
            using (var connection = new SQLiteConnection(databasePath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $@"UPDATE {sshKeyTable} SET ServerId = {serverId} WHERE Name  = @KeyName;";

                command.Parameters.AddWithValue("@KeyName", keyPair.KeyName);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        //TODO: Handle Exceptions
        public void InsertNewClusterMemberServerRecord(ClusterMemberServer clusterMemberServer)
        {
            var databasePath = $"Data Source={databaseName}";
            var EntryUpdatedAt = DateTime.UtcNow;

            using (var connection = new SQLiteConnection(databasePath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $@"INSERT INTO {clusterMemberTable} (ServerId, ServerName, PublicIpv6, Status, ServerCreatedAt, EntryUpdatedAt) 
                        VALUES (@ServerId, @ServerName, @PublicIpv6, @Status, @ServerCreatedAt, @EntryUpdatedAt);";

                command.Parameters.AddWithValue("@ServerId", clusterMemberServer.ServerId);
                command.Parameters.AddWithValue("@ServerName", clusterMemberServer.ServerName);
                command.Parameters.AddWithValue("@PublicIpv6", clusterMemberServer.PublicIpv6);
                command.Parameters.AddWithValue("@Status", clusterMemberServer.Status);
                command.Parameters.AddWithValue("@ServerCreatedAt", clusterMemberServer.CreatedAt);
                command.Parameters.AddWithValue("@EntryUpdatedAt", EntryUpdatedAt);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
