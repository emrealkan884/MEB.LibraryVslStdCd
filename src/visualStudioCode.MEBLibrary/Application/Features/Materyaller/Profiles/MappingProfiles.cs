using Application.Features.Materyaller.Commands.Create;
using Application.Features.Materyaller.Commands.Delete;
using Application.Features.Materyaller.Commands.Update;
using Application.Features.Materyaller.Queries.GetById;
using Application.Features.Materyaller.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Materyaller.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateMateryalCommand, Materyal>();
        CreateMap<Materyal, CreatedMateryalResponse>();

        CreateMap<UpdateMateryalCommand, Materyal>();
        CreateMap<Materyal, UpdatedMateryalResponse>();

        CreateMap<DeleteMateryalCommand, Materyal>();
        CreateMap<Materyal, DeletedMateryalResponse>();

        CreateMap<Materyal, GetByIdMateryalResponse>();

        CreateMap<Materyal, GetListMateryalListItemDto>();
        CreateMap<IPaginate<Materyal>, GetListResponse<GetListMateryalListItemDto>>();
    }
}
