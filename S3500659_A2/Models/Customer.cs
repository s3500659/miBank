using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.Models
{

    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string CustomerName { get; set; }

        [StringLength(11)]
        public string TFN { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(3)]
        [RegularExpression("^(NSW|VIC|ACT|QLD|WA|TAS|NT)$")]
        public string State { get; set; }

        [StringLength(4)] // TODO - Must be 4 digit number
        public string PostCode { get; set; }

        [StringLength(15)] // TODO - Fix phone number format
        [Required]
        public string Phone { get; set; }

        public virtual List<Account> Accounts { get; set; }

    }
}
