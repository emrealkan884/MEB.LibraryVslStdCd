using Application.Features.MateryalFormatDetaylari.Commands.Create;
using Application.Features.MateryalFormatDetaylari.Commands.Delete;
using Application.Features.MateryalFormatDetaylari.Commands.Update;
using Application.Features.MateryalFormatDetaylari.Queries.GetById;
using Application.Features.MateryalFormatDetaylari.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.MateryalFormatDetaylari.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateMateryalFormatDetayCommand, MateryalFormatDetay>();
        CreateMap<MateryalFormatDetay, CreatedMateryalFormatDetayResponse>();

        CreateMap<UpdateMateryalFormatDetayCommand, MateryalFormatDetay>();
        CreateMap<MateryalFormatDetay, UpdatedMateryalFormatDetayResponse>();

        CreateMap<DeleteMateryalFormatDetayCommand, MateryalFormatDetay>();
        CreateMap<MateryalFormatDetay, DeletedMateryalFormatDetayResponse>();

        CreateMap<MateryalFormatDetay, GetByIdMateryalFormatDetayResponse>();

        CreateMap<MateryalFormatDetay, GetListMateryalFormatDetayListItemDto>();
        CreateMap<IPaginate<MateryalFormatDetay>, GetListResponse<GetListMateryalFormatDetayListItemDto>>();
    }
}
