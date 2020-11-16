using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
 
        [Table("trabalho_final")]
        public abstract class TFC
        {
            [Key]
            public int ID { get; set; }
            [Required]
            public int Vagas { get; set; }
            [DefaultValue(true)]
            public bool Proposta { get; set; }
            [DefaultValue(false)]
            public bool Aceite { get; set; }
            [Required]
            public string Description { get; set; }






    }
    }

