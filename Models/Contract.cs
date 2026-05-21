using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        // navigation property
        public Client? Client { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string ServiceLevel { get; set; } = string.Empty;

        public string SignedAgreementPath { get; set; } = string.Empty;

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
            = new List<ServiceRequest>();
    }
}