using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class ProfileViewModel
    {
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

        [StringLength(4)] 
        public string PostCode { get; set; }

        [StringLength(15)] 
        [Required]
        public string Phone { get; set; }

        
    }
}
