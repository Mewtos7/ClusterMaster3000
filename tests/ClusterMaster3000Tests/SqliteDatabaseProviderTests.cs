using System;
using System.Data.SQLite;
using ClusterMaster3000.clusterMasterService.common.provider.database;
using ClusterMaster3000.clusterMasterService.helper;
using ClusterMaster3000.clusterMasterService.models;

namespace ClusterMaster3000Tests
{
    public class SqliteDatabaseProviderTests
    {
        [Fact]
        public void CreateDatabaseAndCheckIfItExists()
        {
            // Arrange
            var databaseProvider = new SqliteDatabaseProvider();
            var databaseName = "clusterMaster3000.db";


            // Act
            databaseProvider.CreateNewDatabaseIfNotExists();

            // Assert
            Assert.True(File.Exists(databaseName));
        }

        [Fact]
        public void CreateNewClusterMemberServerTableAndCheckIfItExists()
        {
            // Arrange
            var databaseProvider = new SqliteDatabaseProvider();
            databaseProvider.CreateNewDatabaseIfNotExists();

            // Act
            databaseProvider.CreateNewClusterMemberServerTableIfNotExists();
            var connection = new SQLiteConnection($"Data Source=clusterMaster3000.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='clusterMember';", connection);
            var result = command.ExecuteScalar();
            connection.Close();

            // Assert
            Assert.True(result is "clusterMember");
        }

        [Fact]
        public void CreateNewSshKeyTableAndCheckIfItExists()
        {
            // Arrange
            var databaseProvider = new SqliteDatabaseProvider();
            databaseProvider.CreateNewDatabaseIfNotExists();

            // Act
            databaseProvider.CreateNewSshKeyTableIfNotExists();
            var connection = new SQLiteConnection($"Data Source=clusterMaster3000.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='sshKeys';", connection);
            var result = command.ExecuteScalar();
            connection.Close();

            // Assert
            Assert.True(result is "sshKeys");
        }

        [Fact]
        public void InsertNewSshKeyRecordAndCheckIfItExists()
        {
            // Arrange
            var databaseProvider = new SqliteDatabaseProvider();
            databaseProvider.CreateNewDatabaseIfNotExists();
            var sshPrivateKey = "TEST";
            var keyName = "testKey";
            var keyPair = new Cryptography.SshKeyPair
            {
                PublicKey = "1234555",
                EncryptedPrivateKey = sshPrivateKey,
                IV = "123445",
                KeyName = keyName
            };

            // Act
            databaseProvider.InsertNewSshKeyRecord(keyPair);
            var connection = new SQLiteConnection($"Data Source=clusterMaster3000.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand($"SELECT SshPrivateKey FROM sshKeys WHERE Name = '{keyName}';", connection);
            var result = command.ExecuteScalar();
            connection.Close();

            // Assert
            Assert.True(sshPrivateKey.Equals(result));
        }

        [Fact]
        public void InsertNewClusterMemberServerRecordAndCheckIfItExists()
        {
            // Arrange
            var databaseProvider = new SqliteDatabaseProvider();
            databaseProvider.CreateNewDatabaseIfNotExists();
            var serverName = "test";
            var serverId = "10000";
            var clusterMember = new ClusterMemberServer
            {
                CreatedAt = DateTime.UtcNow,
                PublicIpv6 = "1234",
                ServerId = serverId,
                ServerName = serverName,
                Status = "test"
            };

            // Act
            databaseProvider.InsertNewClusterMemberServerRecord(clusterMember);
            var connection = new SQLiteConnection($"Data Source=clusterMaster3000.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand($"SELECT ServerName FROM clusterMember WHERE ServerId = '{serverId}';", connection);
            var result = command.ExecuteScalar();
            connection.Close();

            // Assert
            Assert.True(serverName.Equals(result));
        }

        [Fact]
        public void UpdateSshRecordAndCheckIfItExists()
        {
            // Arrange
            var databaseProvider = new SqliteDatabaseProvider();
            databaseProvider.CreateNewDatabaseIfNotExists();
            var sshPrivateKey = "TEST";
            
            var keyName = "testKey";
            var keyPair = new Cryptography.SshKeyPair
            {
                PublicKey = "1234555",
                EncryptedPrivateKey = sshPrivateKey,
                IV = "123445",
                KeyName = keyName
            };

            var serverId = "10000";

            // Act
            databaseProvider.UpdateSshKeyRecord(keyPair, serverId);
            var connection = new SQLiteConnection($"Data Source=clusterMaster3000.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand($"SELECT ServerId FROM sshKeys WHERE Name = '{keyName}';", connection);
            var result = command.ExecuteScalar();
            connection.Close();

            // Assert
            Assert.True(serverId.Equals(result));
        }


    }
}

