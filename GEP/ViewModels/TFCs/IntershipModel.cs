using GEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels.TFCs
{
    public class IntershipModel
    {
        public int Vagas { get; set; }
        public string Role { get; set; }
        public int CompanyRespId { get; set; }
        public string Description { get; set; }
    }
}
