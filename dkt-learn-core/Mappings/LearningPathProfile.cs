using AutoMapper;
using dkt_learn_core.shared.Models.Models;
using dkt_learn_core.shared.Models.Dtos;

namespace dkt_learn_core.Mappings;

public class LearningPathProfile : Profile
{
    public LearningPathProfile()
    {
        CreateMap<LearningPath, LearningPathResponseDto>();
        CreateMap<LearningPathDto, LearningPath>();
    }
}