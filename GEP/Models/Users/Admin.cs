﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    [Table("admin")]
    public class Admin
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
