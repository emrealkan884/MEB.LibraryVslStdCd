using Application.Features.OduncIslemler.Commands.Create;
using Application.Features.OduncIslemler.Commands.Delete;
using Application.Features.OduncIslemler.Commands.Update;
using Application.Features.OduncIslemler.Queries.GetById;
using Application.Features.OduncIslemler.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.OduncIslemler.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateOduncIslemiCommand, OduncIslemi>();
        CreateMap<OduncIslemi, CreatedOduncIslemiResponse>();

        CreateMap<UpdateOduncIslemiCommand, OduncIslemi>();
        CreateMap<OduncIslemi, UpdatedOduncIslemiResponse>();

        CreateMap<DeleteOduncIslemiCommand, OduncIslemi>();
        CreateMap<OduncIslemi, DeletedOduncIslemiResponse>();

        CreateMap<OduncIslemi, GetByIdOduncIslemiResponse>();

        CreateMap<OduncIslemi, GetListOduncIslemiListItemDto>();
        CreateMap<IPaginate<OduncIslemi>, GetListResponse<GetListOduncIslemiListItemDto>>();
    }
}
