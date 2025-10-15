using Application.Features.Raflar.Commands.Create;
using Application.Features.Raflar.Commands.Delete;
using Application.Features.Raflar.Commands.Update;
using Application.Features.Raflar.Queries.GetById;
using Application.Features.Raflar.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Raflar.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateRafCommand, Raf>();
        CreateMap<Raf, CreatedRafResponse>();

        CreateMap<UpdateRafCommand, Raf>();
        CreateMap<Raf, UpdatedRafResponse>();

        CreateMap<DeleteRafCommand, Raf>();
        CreateMap<Raf, DeletedRafResponse>();

        CreateMap<Raf, GetByIdRafResponse>();

        CreateMap<Raf, GetListRafListItemDto>();
        CreateMap<IPaginate<Raf>, GetListResponse<GetListRafListItemDto>>();
    }
}
