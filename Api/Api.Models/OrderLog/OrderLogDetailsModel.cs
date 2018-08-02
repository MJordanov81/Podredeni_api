namespace Api.Models.OrderLog
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using Api.Models.Infrastructure.Constants;
    using AutoMapper;
    using System;

    public class OrderLogDetailsModel : IMapFrom<OrderLog>, IHaveCustomMapping
    {
        public string Action { get; set; }

        public DateTime DateTime { get; set; }

        public string Username { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<OrderLog, OrderLogDetailsModel>()
                .ForMember(m => m.Username, cfg => cfg.MapFrom(l => l.User.Name?? ModelConstants.AnonymousUser));
        }
    }
}
