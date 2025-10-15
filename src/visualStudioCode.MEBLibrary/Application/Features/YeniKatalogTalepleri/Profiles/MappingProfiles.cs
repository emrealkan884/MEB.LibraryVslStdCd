using Application.Features.YeniKatalogTalepleri.Commands.Create;
using Application.Features.YeniKatalogTalepleri.Commands.Delete;
using Application.Features.YeniKatalogTalepleri.Commands.Update;
using Application.Features.YeniKatalogTalepleri.Queries.GetById;
using Application.Features.YeniKatalogTalepleri.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.YeniKatalogTalepleri.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateYeniKatalogTalebiCommand, YeniKatalogTalebi>();
        CreateMap<YeniKatalogTalebi, CreatedYeniKatalogTalebiResponse>();

        CreateMap<UpdateYeniKatalogTalebiCommand, YeniKatalogTalebi>();
        CreateMap<YeniKatalogTalebi, UpdatedYeniKatalogTalebiResponse>();

        CreateMap<DeleteYeniKatalogTalebiCommand, YeniKatalogTalebi>();
        CreateMap<YeniKatalogTalebi, DeletedYeniKatalogTalebiResponse>();

        CreateMap<YeniKatalogTalebi, GetByIdYeniKatalogTalebiResponse>();

        CreateMap<YeniKatalogTalebi, GetListYeniKatalogTalebiListItemDto>();
        CreateMap<IPaginate<YeniKatalogTalebi>, GetListResponse<GetListYeniKatalogTalebiListItemDto>>();
    }
}
