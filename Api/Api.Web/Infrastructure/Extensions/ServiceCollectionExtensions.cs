namespace Api.Web.Infrastructure.Extensions
{
    using Api.Services;
    using Api.Web.Infrastructure.Constants;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration
                        .GetSection(JwtConstants.JwtConfig)[JwtConstants.Issuer],
                    ValidAudience = configuration
                        .GetSection(JwtConstants.JwtConfig)[JwtConstants.Audience],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(JwtConstants.JwtConfig)[JwtConstants.Key]))
                };
            });

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            Type[] types = Assembly.GetAssembly(typeof(IService))
                .GetTypes();


            types
                .Where(t => t.IsClass && !t.IsAbstract && types.Any(s => s.IsInterface && s.IsAssignableFrom(t) && s.Name.ToLower().Contains(t.Name.ToLower())))
                .Select(t => new
                {
                    Interface = types
                .Where(i => i.IsAssignableFrom(t) && i.IsInterface)
                .FirstOrDefault(),
                    Implementation = t
                })
                .ToDictionary(k => k.Interface, k => k.Implementation).ToList().ForEach(s =>
                {
                    services.AddTransient(s.Key, s.Value);

                });

            return services;
        }
    }
}
