using System.Security;
using System.Text.Json;
using ClusterMaster3000.classes.models;

namespace ClusterMaster3000.classes.helper
{
    class JsonMapping
    {
        public ClusterMemberServer MapServerFieldsToClusterMemberServer(string createdServerResponse, string sshPrivateKey)
        {
            using JsonDocument jsonDocument = JsonDocument.Parse(createdServerResponse);
            JsonElement root = jsonDocument.RootElement;

            ClusterMemberServer clusterMemberServer = new ClusterMemberServer()
            {
                ServerId = root.GetProperty("server").GetProperty("id").GetInt32(),
                ServerName = root.GetProperty("server").GetProperty("name").GetString() ?? "unknown",
                PublicIpv6 = root.GetProperty("server").GetProperty("public_net").GetProperty("ipv6").GetProperty("ip").GetString() ?? "unknown",
                Status = root.GetProperty("server").GetProperty("status").GetString() ?? "unknown",
                CreatedAt = root.GetProperty("server").GetProperty("created").GetDateTimeOffset().UtcDateTime,
                SshPrivateKey = sshPrivateKey
            };
            return clusterMemberServer;
        }
    }
}
