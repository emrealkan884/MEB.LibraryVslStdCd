using Application.Features.Kutuphaneler.Commands.Create;
using Application.Features.Kutuphaneler.Commands.Delete;
using Application.Features.Kutuphaneler.Commands.Update;
using Application.Features.Kutuphaneler.Queries.GetById;
using Application.Features.Kutuphaneler.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Kutuphaneler.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateKutuphaneCommand, Kutuphane>();
        CreateMap<Kutuphane, CreatedKutuphaneResponse>();

        CreateMap<UpdateKutuphaneCommand, Kutuphane>();
        CreateMap<Kutuphane, UpdatedKutuphaneResponse>();

        CreateMap<DeleteKutuphaneCommand, Kutuphane>();
        CreateMap<Kutuphane, DeletedKutuphaneResponse>();

        CreateMap<Kutuphane, GetByIdKutuphaneResponse>();

        CreateMap<Kutuphane, GetListKutuphaneListItemDto>();
        CreateMap<IPaginate<Kutuphane>, GetListResponse<GetListKutuphaneListItemDto>>();
    }
}
