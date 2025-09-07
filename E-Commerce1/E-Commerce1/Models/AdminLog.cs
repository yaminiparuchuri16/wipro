using System.ComponentModel.DataAnnotations.Schema;
namespace E_Commerce1.Models
{
    public class AdminLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AdminId { get; set; }       // Users.Id
        public string Action { get; set; } = "";
        public string? Target { get; set; }
    }
}