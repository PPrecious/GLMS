using GLMS.Web.Models;

namespace GLMS.Web.Data
{
    public static class InMemoryDb
    {
        public static List<Client> Clients = new();
        public static List<Contract> Contracts = new();
        public static List<ServiceRequest> ServiceRequests = new();
    }
}