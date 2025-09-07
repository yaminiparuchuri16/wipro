using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class User1
    {
        [Key]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}