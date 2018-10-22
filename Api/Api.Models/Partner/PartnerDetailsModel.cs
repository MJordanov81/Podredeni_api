namespace Api.Models.Partner
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using System.Collections.Generic;
    using AutoMapper;
    using System.Linq;

    public class PartnerDetailsModel : IMapFrom<Partner>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string WebUrl { get; set; }

        public string Category { get; set; }

        public int Index { get; set; }

        public ICollection<PartnerLocationDetailsModel> PartnerLocations { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Partner, PartnerDetailsModel>()
                .ForMember(p => p.PartnerLocations, cfg => cfg.MapFrom(p => p.PartnerLocations.Select(pl => new PartnerLocationDetailsModel() { Address = pl.Address, City = pl.City })));

        }
    }
}
