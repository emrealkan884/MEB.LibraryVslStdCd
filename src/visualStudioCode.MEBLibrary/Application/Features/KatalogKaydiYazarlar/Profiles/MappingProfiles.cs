using Application.Features.KatalogKaydiYazarlar.Commands.Create;
using Application.Features.KatalogKaydiYazarlar.Commands.Delete;
using Application.Features.KatalogKaydiYazarlar.Commands.Update;
using Application.Features.KatalogKaydiYazarlar.Queries.GetById;
using Application.Features.KatalogKaydiYazarlar.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKaydiYazarlar.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateKatalogKaydiYazarCommand, KatalogKaydiYazar>();
        CreateMap<KatalogKaydiYazar, CreatedKatalogKaydiYazarResponse>();

        CreateMap<UpdateKatalogKaydiYazarCommand, KatalogKaydiYazar>();
        CreateMap<KatalogKaydiYazar, UpdatedKatalogKaydiYazarResponse>();

        CreateMap<DeleteKatalogKaydiYazarCommand, KatalogKaydiYazar>();
        CreateMap<KatalogKaydiYazar, DeletedKatalogKaydiYazarResponse>();

        CreateMap<KatalogKaydiYazar, GetByIdKatalogKaydiYazarResponse>();

        CreateMap<KatalogKaydiYazar, GetListKatalogKaydiYazarListItemDto>();
        CreateMap<IPaginate<KatalogKaydiYazar>, GetListResponse<GetListKatalogKaydiYazarListItemDto>>();
    }
}
