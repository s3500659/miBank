using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S3500659_A2.Models
{
    public class Payee
    {
        [Required]
        public int PayeeID { get; set; }

        [Required, StringLength(50)]
        public string PayeeName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(3)]
        public string State { get; set; }

        [StringLength(4)]
        public string PostCode { get; set; }

        [Required, StringLength(15)]
        public string Phone { get; set; }

        public virtual List<BillPay> BillPays { get; set; }

    }
}