namespace Kanban.Application.AutoMapperProfile
{
    using AutoMapper;
    using Kanban.Application.ViewModels;
    using Kanban.Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Card, CardViewModel>().ReverseMap();
        }
    }
}
