namespace Api.Models.Partner
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class PartnerDetailsModel : IMapFrom<Partner>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string WebUrl { get; set; }

        public string Details { get; set; }
    }
}
