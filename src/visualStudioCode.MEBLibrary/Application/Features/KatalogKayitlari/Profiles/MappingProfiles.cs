using Application.Features.KatalogKayitlari.Commands.Create;
using Application.Features.KatalogKayitlari.Commands.Delete;
using Application.Features.KatalogKayitlari.Commands.Update;
using Application.Features.KatalogKayitlari.Queries.GetById;
using Application.Features.KatalogKayitlari.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKayitlari.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateKatalogKaydiCommand, KatalogKaydi>();
        CreateMap<KatalogKaydi, CreatedKatalogKaydiResponse>();

        CreateMap<UpdateKatalogKaydiCommand, KatalogKaydi>();
        CreateMap<KatalogKaydi, UpdatedKatalogKaydiResponse>();

        CreateMap<DeleteKatalogKaydiCommand, KatalogKaydi>();
        CreateMap<KatalogKaydi, DeletedKatalogKaydiResponse>();

        CreateMap<KatalogKaydi, GetByIdKatalogKaydiResponse>();

        CreateMap<KatalogKaydi, GetListKatalogKaydiListItemDto>();
        CreateMap<IPaginate<KatalogKaydi>, GetListResponse<GetListKatalogKaydiListItemDto>>();
    }
}
