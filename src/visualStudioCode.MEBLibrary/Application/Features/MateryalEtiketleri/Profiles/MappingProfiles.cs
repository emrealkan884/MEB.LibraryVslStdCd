using Application.Features.MateryalEtiketleri.Commands.Create;
using Application.Features.MateryalEtiketleri.Commands.Delete;
using Application.Features.MateryalEtiketleri.Commands.Update;
using Application.Features.MateryalEtiketleri.Queries.GetById;
using Application.Features.MateryalEtiketleri.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.MateryalEtiketleri.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateMateryalEtiketCommand, MateryalEtiket>();
        CreateMap<MateryalEtiket, CreatedMateryalEtiketResponse>();

        CreateMap<UpdateMateryalEtiketCommand, MateryalEtiket>();
        CreateMap<MateryalEtiket, UpdatedMateryalEtiketResponse>();

        CreateMap<DeleteMateryalEtiketCommand, MateryalEtiket>();
        CreateMap<MateryalEtiket, DeletedMateryalEtiketResponse>();

        CreateMap<MateryalEtiket, GetByIdMateryalEtiketResponse>();

        CreateMap<MateryalEtiket, GetListMateryalEtiketListItemDto>();
        CreateMap<IPaginate<MateryalEtiket>, GetListResponse<GetListMateryalEtiketListItemDto>>();
    }
}
