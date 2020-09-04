using AutoMapper;
using GEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProf : Profile
    {
        public ViewModelToEntityMappingProf()
        {
            CreateMap<RegistrationProfViewModel, User>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
