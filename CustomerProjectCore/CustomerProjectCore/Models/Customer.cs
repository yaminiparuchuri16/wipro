using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomerProjectCore.Models
{
    public class Customer
    {
        [Key]
        [Column("custId")]
        public int CustId { get; set; }

        [Column("custName")]
        public string? CustName { get; set; }

        [Column("custUserName")]
        public string? CustUserName { get; set; }

        [Column("custPassword")]
        public string? CustPassword { get; set; }


        [Column("city")]
        public string? City { get; set; }

        [Column("state")]
        public string? State { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("mobileNo")]
        public string? MobileNo { get; set; }

    }
}