﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    [Table("professor")]
    public class Professor
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
