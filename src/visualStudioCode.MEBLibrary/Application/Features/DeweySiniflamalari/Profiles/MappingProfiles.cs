using Application.Features.DeweySiniflamalari.Commands.Create;
using Application.Features.DeweySiniflamalari.Commands.Delete;
using Application.Features.DeweySiniflamalari.Commands.Update;
using Application.Features.DeweySiniflamalari.Queries.GetById;
using Application.Features.DeweySiniflamalari.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.DeweySiniflamalari.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateDeweySiniflamaCommand, DeweySiniflama>();
        CreateMap<DeweySiniflama, CreatedDeweySiniflamaResponse>();

        CreateMap<UpdateDeweySiniflamaCommand, DeweySiniflama>();
        CreateMap<DeweySiniflama, UpdatedDeweySiniflamaResponse>();

        CreateMap<DeleteDeweySiniflamaCommand, DeweySiniflama>();
        CreateMap<DeweySiniflama, DeletedDeweySiniflamaResponse>();

        CreateMap<DeweySiniflama, GetByIdDeweySiniflamaResponse>();

        CreateMap<DeweySiniflama, GetListDeweySiniflamaListItemDto>();
        CreateMap<IPaginate<DeweySiniflama>, GetListResponse<GetListDeweySiniflamaListItemDto>>();
    }
}
