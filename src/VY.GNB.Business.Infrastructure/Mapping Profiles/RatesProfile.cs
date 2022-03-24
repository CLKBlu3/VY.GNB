using AutoMapper;
using System;
using VY.GNB.Business.Implementation.Domain;
using VY.GNB.Dtos;

namespace VY.GNB.Business.Implementation.Mapping_Profiles
{
    public class RatesProfile : Profile
    {
        public RatesProfile()
        {
            CreateMap<RateDto, RateDomain>()
                .ForMember(x => x.From, opt => opt.MapFrom(y => y.From))
                .ForMember(x => x.Ratio, opt => opt.MapFrom(y => Double.Parse(y.Ratio, System.Globalization.CultureInfo.InvariantCulture)))
                .ForMember(x => x.To, opt => opt.MapFrom(y => y.To));
        }
    }
}
