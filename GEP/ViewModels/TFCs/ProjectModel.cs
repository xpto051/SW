using GEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels.TFCs
{
    //para o post dos projetos
    public class ProjectModel
    {
        public int Vagas { get; set; }
        public string Theme { get; set; }
        public int ProfessorID { get; set; }
        public string Description { get; set; }


    }
}
