
namespace ClusterMaster3000.clusterMasterService.models
{
    //TODO: Check when and which method should update UpdatedAt
    public class ClusterMemberServer
    {
        public required string ServerId { get; set; }
        public required string ServerName { get; set; }
        public required string PublicIpv6 { get; set; }
        public required string Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
