using Application.Features.OtoriteKayitlari.Commands.Create;
using Application.Features.OtoriteKayitlari.Commands.Delete;
using Application.Features.OtoriteKayitlari.Commands.Update;
using Application.Features.OtoriteKayitlari.Queries.GetById;
using Application.Features.OtoriteKayitlari.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.OtoriteKayitlari.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateOtoriteKaydiCommand, OtoriteKaydi>();
        CreateMap<OtoriteKaydi, CreatedOtoriteKaydiResponse>();

        CreateMap<UpdateOtoriteKaydiCommand, OtoriteKaydi>();
        CreateMap<OtoriteKaydi, UpdatedOtoriteKaydiResponse>();

        CreateMap<DeleteOtoriteKaydiCommand, OtoriteKaydi>();
        CreateMap<OtoriteKaydi, DeletedOtoriteKaydiResponse>();

        CreateMap<OtoriteKaydi, GetByIdOtoriteKaydiResponse>();

        CreateMap<OtoriteKaydi, GetListOtoriteKaydiListItemDto>();
        CreateMap<IPaginate<OtoriteKaydi>, GetListResponse<GetListOtoriteKaydiListItemDto>>();
    }
}
