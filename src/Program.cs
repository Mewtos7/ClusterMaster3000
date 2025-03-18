using ClusterMaster3000.clusterMasterService;

namespace ClusterMaster3000
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var program = new Program();
            ClusterMasterService clusterMasterService = new ClusterMasterService();
            await clusterMasterService.ExecuteClusterMasterService();
        }
    }
}
