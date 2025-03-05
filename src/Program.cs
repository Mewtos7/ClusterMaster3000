using System;
using System.Buffers;
using System.Text.Json;
using ClusterMaster3000.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace ClusterMaster3000
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var p = new Program();
            await p.OrchestrateServers();
        }

        private async Task OrchestrateServers()
        {
            //Create and Check database
            SqliteDatabase sqliteDatabase = new SqliteDatabase();
            var databaseCreationMessage = sqliteDatabase.CreateNewDatabaseStructureIfNotExists("clustermaster.db");
            //Create Server
            HetznerServices hetznerServices = new HetznerServices();
            var serverCreationMessage = await hetznerServices.CreateServer();

            

        }
    }
}
