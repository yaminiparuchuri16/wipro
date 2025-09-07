using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CustomerProjectCore.Models
{
    public class Wallet
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("walletId")]
        public int WalletId { get; set; }

        //[Required]
        [ForeignKey("Customer")]
        [Column("custId")]
        public int CustId { get; set; }

        [Column("walletType")]

        public string? WalletType { get; set; }

        [Column("walletAmount")]
        public decimal WalletAmount { get; set; }

        public Customer? Customer { get; set; }
    }
}