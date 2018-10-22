using Api.Models.Partner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services.Interfaces
{
    public interface IPartnerService
    {
        Task<string> Create(PartnerCreateEditModel data);

        Task Edit(string partnerId, PartnerCreateEditModel data);

        Task<PartnerDetailsModel> Get(string partnerId);

        Task<IEnumerable<PartnerDetailsModel>> Get();

        Task<Dictionary<string, List<PartnerDetailsModel>>> GetGoupedByCity();

        Task Delete(string partnerId);

        Task Reorder(string[] orderedPartnerIds);
    }
}
