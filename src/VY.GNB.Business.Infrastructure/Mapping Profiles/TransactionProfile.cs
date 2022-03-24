using AutoMapper;
using VY.GNB.Dtos;
using VY.GNB.Infrastructure.Contracts.Entities;

namespace VY.GNB.Business.Implementation.Mapping_Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionDto, Transaction>()
                .ForMember(x => x.Amount, opt => opt.MapFrom(y => y.Amount))
                .ForMember(x => x.Currency, opt => opt.MapFrom(y => y.Currency))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.ProductName))
                .ReverseMap();
        }
    }
}
