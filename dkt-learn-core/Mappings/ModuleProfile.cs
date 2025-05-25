using AutoMapper;
using dkt_learn_core.shared.Models.Models;
using dkt_learn_core.shared.Models.Dtos;

namespace dkt_learn_core.Mappings;

public class ModuleProfile : Profile
{
   public ModuleProfile()
   {
      CreateMap<Module, ModuleResponseDto>();
      CreateMap<ModuleDto, Module>();
   }
}