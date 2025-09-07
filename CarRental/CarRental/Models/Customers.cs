using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public class Customers
    {
        [Key]
        [Column("customersId")]
        public int CustomersID { get; set; }

        [Column("firstName")]
        public string? FirstName { get; set; }

        [Column("lastName")]
        public string? LastName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phoneNumber")]
        public string? PhoneNumber { get; set; }

    }
}
