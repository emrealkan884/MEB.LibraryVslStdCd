using Application.Features.KutuphaneBolumleri.Commands.Create;
using Application.Features.KutuphaneBolumleri.Commands.Delete;
using Application.Features.KutuphaneBolumleri.Commands.Update;
using Application.Features.KutuphaneBolumleri.Queries.GetById;
using Application.Features.KutuphaneBolumleri.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KutuphaneBolumleri.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateKutuphaneBolumuCommand, KutuphaneBolumu>();
        CreateMap<KutuphaneBolumu, CreatedKutuphaneBolumuResponse>();

        CreateMap<UpdateKutuphaneBolumuCommand, KutuphaneBolumu>();
        CreateMap<KutuphaneBolumu, UpdatedKutuphaneBolumuResponse>();

        CreateMap<DeleteKutuphaneBolumuCommand, KutuphaneBolumu>();
        CreateMap<KutuphaneBolumu, DeletedKutuphaneBolumuResponse>();

        CreateMap<KutuphaneBolumu, GetByIdKutuphaneBolumuResponse>();

        CreateMap<KutuphaneBolumu, GetListKutuphaneBolumuListItemDto>();
        CreateMap<IPaginate<KutuphaneBolumu>, GetListResponse<GetListKutuphaneBolumuListItemDto>>();
    }
}
