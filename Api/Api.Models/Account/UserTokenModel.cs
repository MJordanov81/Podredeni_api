namespace Api.Models.Account
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using AutoMapper;

    public class UserTokenModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<User, UserTokenModel>()
                .ForMember(m => m.Role, cfg => cfg.MapFrom(u => u.Role.Name));
        }
    }
}
