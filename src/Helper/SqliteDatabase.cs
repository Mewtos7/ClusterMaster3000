using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ClusterManager3000.Helper
{
    internal class SqliteDatabase
    {
        public void CreateNewDatabaseIfNotExists(string databaseName)
        {
            if (!File.Exists(databaseName))
            {
                string databasePath = $"Data Source={databaseName}";
                try
                {
                    SQLiteConnection.CreateFile(databaseName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Database could not be created, see {ex.Message}");
                }

            }
        }
    }
}
