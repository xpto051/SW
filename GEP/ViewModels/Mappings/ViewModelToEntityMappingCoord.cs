using AutoMapper;
using GEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels.Mappings
{
    public class ViewModelToEntityMappingCoord : Profile
    {
        public ViewModelToEntityMappingCoord()
        {
            CreateMap<RegistrationCoordenatorViewModel, User>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
