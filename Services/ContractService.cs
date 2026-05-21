namespace GLMS.Web.Services
{
    public class ContractService
    {
        public bool IsContractValid(string status)
        {
            return status != "Expired" && status != "On Hold";
        }
    }
}
