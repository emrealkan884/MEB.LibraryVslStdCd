using Application.Features.Etkinlikler.Commands.Create;
using Application.Features.Etkinlikler.Commands.Delete;
using Application.Features.Etkinlikler.Commands.Update;
using Application.Features.Etkinlikler.Queries.GetById;
using Application.Features.Etkinlikler.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Etkinlikler.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateEtkinlikCommand, Etkinlik>();
        CreateMap<Etkinlik, CreatedEtkinlikResponse>();

        CreateMap<UpdateEtkinlikCommand, Etkinlik>();
        CreateMap<Etkinlik, UpdatedEtkinlikResponse>();

        CreateMap<DeleteEtkinlikCommand, Etkinlik>();
        CreateMap<Etkinlik, DeletedEtkinlikResponse>();

        CreateMap<Etkinlik, GetByIdEtkinlikResponse>();

        CreateMap<Etkinlik, GetListEtkinlikListItemDto>();
        CreateMap<IPaginate<Etkinlik>, GetListResponse<GetListEtkinlikListItemDto>>();
    }
}
