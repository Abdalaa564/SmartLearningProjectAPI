

using SmartLearning.Application.DTOs.Resource;

namespace SmartLearning.Application.Mappings
{
    public class ResourceProfile :Profile
    {
        public ResourceProfile()
        {
            CreateMap<CreateResourceDto, Resource>();
            CreateMap<Resource, ResourceResponseDto>();
        }
    }
}
