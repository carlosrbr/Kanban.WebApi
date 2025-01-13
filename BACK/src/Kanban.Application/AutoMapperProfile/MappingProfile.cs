namespace Kanban.Application.AutoMapperProfile
{
    using AutoMapper;
    using Kanban.Application.ViewModels;
    using Kanban.Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Card, CardViewModel>();
            CreateMap<CardViewModel, Card>()
            .ConstructUsing(src => new Card(src.Id, src.Titulo, src.Conteudo, src.Lista));
        }
    }
}
