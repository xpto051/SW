using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models.File
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public User User { get; set; }

        public string Localization { get; set; }

        public bool isDeleted { get; set; }

        public bool wasAccepted { get; set; }
    }
}
