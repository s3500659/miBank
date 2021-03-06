﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.Models
{
    public class Login
    {
        [Required]
        public string LoginID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [StringLength(8)]
        public DateTime ModifyDate { get; set; }
    }
}
