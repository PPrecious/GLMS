using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        public int ContractId { get; set; }

        public Contract? Contract { get; set; }

        [Required]
        public required string Description { get; set; }

        public decimal CostUSD { get; set; }

        public decimal CostZAR { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}