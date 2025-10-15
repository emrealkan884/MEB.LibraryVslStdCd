using Application.Features.KatalogKonulari.Commands.Create;
using Application.Features.KatalogKonulari.Commands.Delete;
using Application.Features.KatalogKonulari.Commands.Update;
using Application.Features.KatalogKonulari.Queries.GetById;
using Application.Features.KatalogKonulari.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKonulari.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateKatalogKonuCommand, KatalogKonu>();
        CreateMap<KatalogKonu, CreatedKatalogKonuResponse>();

        CreateMap<UpdateKatalogKonuCommand, KatalogKonu>();
        CreateMap<KatalogKonu, UpdatedKatalogKonuResponse>();

        CreateMap<DeleteKatalogKonuCommand, KatalogKonu>();
        CreateMap<KatalogKonu, DeletedKatalogKonuResponse>();

        CreateMap<KatalogKonu, GetByIdKatalogKonuResponse>();

        CreateMap<KatalogKonu, GetListKatalogKonuListItemDto>();
        CreateMap<IPaginate<KatalogKonu>, GetListResponse<GetListKatalogKonuListItemDto>>();
    }
}
