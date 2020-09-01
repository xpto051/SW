﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    [Table("companyresp")]
    public class CompanyResp
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public static implicit operator CompanyResp(Professor v)
        {
            throw new NotImplementedException();
        }
    }
}
