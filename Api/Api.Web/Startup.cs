namespace Api.Web
{
    using Api.Data;
    using Api.Web.Models.Config;
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Implementations;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionWork")));

            services.Configure<SmtpConfiguration>(Configuration.GetSection("Smtp"));

            services.Configure<EkontApiConfiguration>(Configuration.GetSection("EkontApiConfiguration"));

            //Registers a class JwtConfiguration with properties from appsettings.json section "JwtConfiguration"
            services.AddSingleton(Configuration.GetSection(Infrastructure.Constants.JwtConstants.JwtConfig).Get<JwtConfiguration>());

            services.AddSingleton(Configuration.GetSection("AdminCred").Get<AdminCredentials>());

            //Registers a token service
            services.AddTransient<ITokenService, JwtTokenService>();

            services.AddTransient<IMailService, SmtpMailService>();

            services.AddTransient<IHttpService, HttpService>();

            //Registers Jwt authentication service
            services.AddJwtAuthenticationService(this.Configuration);

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddAutoMapper();

            services.AddDomainServices();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
