using Api.Common.Mapping;
using Api.Domain.Entities;

namespace Api.Models.Partner
{
    public class PartnerLocationDetailsModel : IMapFrom<PartnerLocation>
    {
        public string City { get; set; }

        public string Address { get; set; }
    }
}
