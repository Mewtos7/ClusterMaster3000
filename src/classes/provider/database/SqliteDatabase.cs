// Ignore Spelling: databaseprovider

using System.Data.SQLite;
using ClusterMaster3000.classes.models;

namespace ClusterMaster3000.classes.provider.database
{
    class SqliteDatabase
    {
        //TODO: Handle Exceptions
        public readonly string databaseName = "clusterMaster3000.db";
        public readonly string clusterMemberTable = "clusterMember";

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
	                        EntryUpdatedAt DATETIME NOT NULL,
                            SshPrivateKey TEXT);";
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
                    @"INSERT INTO clusterMember (ServerId, ServerName, PublicIpv6, Status, ServerCreatedAt, EntryUpdatedAt, SshPrivateKey) 
                      VALUES (@ServerId, @ServerName, @PublicIpv6, @Status, @ServerCreatedAt, @EntryUpdatedAt, @SshPrivateKey);";

                command.Parameters.AddWithValue("@ServerId", clusterMemberServer.ServerId);
                command.Parameters.AddWithValue("@ServerName", clusterMemberServer.ServerName);
                command.Parameters.AddWithValue("@PublicIpv6", clusterMemberServer.PublicIpv6);
                command.Parameters.AddWithValue("@Status", clusterMemberServer.Status);
                command.Parameters.AddWithValue("@ServerCreatedAt", clusterMemberServer.CreatedAt);
                command.Parameters.AddWithValue("@EntryUpdatedAt", EntryUpdatedAt);
                command.Parameters.AddWithValue("@SshPrivateKey", clusterMemberServer.SshPrivateKey);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
