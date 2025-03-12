// Ignore Spelling: Ipv

using System.Security;

namespace ClusterMaster3000.classes.models
{
    //TODO: Check when and which method should update UpdatedAt
    class ClusterMemberServer
    {
        public required int ServerId { get; set; }
        public required string ServerName { get; set; }
        public required string PublicIpv6 { get; set; }
        public required string Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public required string SshPrivateKey { get; set; }
    }
}
