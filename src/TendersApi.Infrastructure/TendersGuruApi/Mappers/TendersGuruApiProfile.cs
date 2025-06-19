using AutoMapper;
using TendersApi.Application.Models;
using TendersApi.Infrastructure.TendersWebApi.Models;

namespace TendersApi.Infrastructure.TendersGuruApi.Mappers;

public sealed class TendersGuruApiProfile : Profile
{
    public TendersGuruApiProfile()
    {
        CreateMap<TenderGuruApiModel, TenderModel>()
            .ConvertUsing(src => new TenderModel(src.Id, src.Date, src.Title, src.Description, src.AwardedValueEur,
                src.Awarded.SelectMany(a => a.Suppliers.Select(s => new SupplierModel(s.Id, s.Name)))));
    }
}
