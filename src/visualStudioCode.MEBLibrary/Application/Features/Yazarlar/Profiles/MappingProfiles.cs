using Application.Features.Yazarlar.Commands.Create;
using Application.Features.Yazarlar.Commands.Delete;
using Application.Features.Yazarlar.Commands.Update;
using Application.Features.Yazarlar.Queries.GetById;
using Application.Features.Yazarlar.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Yazarlar.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateYazarCommand, Yazar>();
        CreateMap<Yazar, CreatedYazarResponse>();

        CreateMap<UpdateYazarCommand, Yazar>();
        CreateMap<Yazar, UpdatedYazarResponse>();

        CreateMap<DeleteYazarCommand, Yazar>();
        CreateMap<Yazar, DeletedYazarResponse>();

        CreateMap<Yazar, GetByIdYazarResponse>();

        CreateMap<Yazar, GetListYazarListItemDto>();
        CreateMap<IPaginate<Yazar>, GetListResponse<GetListYazarListItemDto>>();
    }
}