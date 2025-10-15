using Application.Features.Rezervasyonlar.Commands.Create;
using Application.Features.Rezervasyonlar.Commands.Delete;
using Application.Features.Rezervasyonlar.Commands.Update;
using Application.Features.Rezervasyonlar.Queries.GetById;
using Application.Features.Rezervasyonlar.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Rezervasyonlar.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateRezervasyonCommand, Rezervasyon>();
        CreateMap<Rezervasyon, CreatedRezervasyonResponse>();

        CreateMap<UpdateRezervasyonCommand, Rezervasyon>();
        CreateMap<Rezervasyon, UpdatedRezervasyonResponse>();

        CreateMap<DeleteRezervasyonCommand, Rezervasyon>();
        CreateMap<Rezervasyon, DeletedRezervasyonResponse>();

        CreateMap<Rezervasyon, GetByIdRezervasyonResponse>();

        CreateMap<Rezervasyon, GetListRezervasyonListItemDto>();
        CreateMap<IPaginate<Rezervasyon>, GetListResponse<GetListRezervasyonListItemDto>>();
    }
}
