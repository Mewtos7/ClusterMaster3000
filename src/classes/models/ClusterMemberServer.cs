// Ignore Spelling: Ipv

namespace ClusterMaster3000.classes.models
{
    internal class ClusterMemberServer
    {
        public required int ServerId { get; set; }
        public required string ServerName { get; set; }
        public required string PublicIpv6 { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
