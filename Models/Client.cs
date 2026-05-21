using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ContactDetails { get; set; } = string.Empty;

        [Required]
        public string Region { get; set; } = string.Empty;

        // ALWAYS initialize collections to avoid null errors
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}