namespace Api.Models.Settings
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class SettingsViewEditModel : IMapFrom<Settings>
    {
        public bool ShowOutOfStock { get; set; }
    }
}
