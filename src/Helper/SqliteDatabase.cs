using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ClusterMaster3000.Helper
{
    internal class SqliteDatabase
    {
        public string CreateNewDatabaseStructureIfNotExists(string databaseName)
        {

            if (File.Exists(databaseName))
            {
                return "databaseAlreadyExists";
            }
            try
            {
                string databasePath = $"Data Source={databaseName}";
                SQLiteConnection.CreateFile(databaseName);

                using (var connection = new SQLiteConnection(databasePath))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = File.ReadAllText("queries/createNewServerTable.sql");
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Database or table could not be created, see {ex.Message}");
            }
            return "success";
        }
    }
}
