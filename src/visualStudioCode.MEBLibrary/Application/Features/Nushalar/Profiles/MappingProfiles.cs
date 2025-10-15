using Application.Features.Nushalar.Commands.Create;
using Application.Features.Nushalar.Commands.Delete;
using Application.Features.Nushalar.Commands.Update;
using Application.Features.Nushalar.Queries.GetById;
using Application.Features.Nushalar.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Nushalar.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateNushaCommand, Nusha>();
        CreateMap<Nusha, CreatedNushaResponse>();

        CreateMap<UpdateNushaCommand, Nusha>();
        CreateMap<Nusha, UpdatedNushaResponse>();

        CreateMap<DeleteNushaCommand, Nusha>();
        CreateMap<Nusha, DeletedNushaResponse>();

        CreateMap<Nusha, GetByIdNushaResponse>();

        CreateMap<Nusha, GetListNushaListItemDto>();
        CreateMap<IPaginate<Nusha>, GetListResponse<GetListNushaListItemDto>>();
    }
}
