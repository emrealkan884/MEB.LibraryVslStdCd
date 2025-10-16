using Application.Features.YeniKatalogTalepleri.Commands.Approve;
using Application.Features.YeniKatalogTalepleri.Commands.Create;
using Application.Features.YeniKatalogTalepleri.Commands.Delete;
using Application.Features.YeniKatalogTalepleri.Commands.Reject;
using Application.Features.YeniKatalogTalepleri.Commands.Update;
using Application.Features.YeniKatalogTalepleri.Queries.GetById;
using Application.Features.YeniKatalogTalepleri.Queries.GetList;
using Application.Features.YeniKatalogTalepleri.Utilities;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.YeniKatalogTalepleri.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateYeniKatalogTalebiCommand, YeniKatalogTalebi>();
        CreateMap<YeniKatalogTalebi, CreatedYeniKatalogTalebiResponse>();

        CreateMap<UpdateYeniKatalogTalebiCommand, YeniKatalogTalebi>();
        CreateMap<YeniKatalogTalebi, UpdatedYeniKatalogTalebiResponse>();

        CreateMap<DeleteYeniKatalogTalebiCommand, YeniKatalogTalebi>();
        CreateMap<YeniKatalogTalebi, DeletedYeniKatalogTalebiResponse>();

        CreateMap<YeniKatalogTalebi, ApprovedYeniKatalogTalebiResponse>();
        CreateMap<YeniKatalogTalebi, RejectedYeniKatalogTalebiResponse>();

        CreateMap<YeniKatalogTalebi, GetByIdYeniKatalogTalebiResponse>()
            .ForMember(dest => dest.SuggestedMateryalTuru, opt => opt.MapFrom(src => MateryalTuruMapper.MapFromTalep(src)))
            .ForMember(dest => dest.SuggestedMateryalAltTuru, opt => opt.MapFrom(src => src.MateryalAltTuru));

        CreateMap<YeniKatalogTalebi, GetListYeniKatalogTalebiListItemDto>()
            .ForMember(dest => dest.SuggestedMateryalTuru, opt => opt.MapFrom(src => MateryalTuruMapper.MapFromTalep(src)))
            .ForMember(dest => dest.SuggestedMateryalAltTuru, opt => opt.MapFrom(src => src.MateryalAltTuru));

        CreateMap<IPaginate<YeniKatalogTalebi>, GetListResponse<GetListYeniKatalogTalebiListItemDto>>();
    }
}
