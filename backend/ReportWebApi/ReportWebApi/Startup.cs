using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ReportWebApi.Authorization;

namespace ReportWebApi;

public class Startup
{
    private readonly PolicyWithRoles _policyWithRoles = new()
    {
        PolicyName = "OnlyForProtheticUser",
        Roles = ["prothetic_user"]
    };

    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();

        services
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = _configuration.GetSection("KeycloakAuthority").Get<string>();
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

        services
            .AddAuthorizationBuilder()
            .AddPolicy(_policyWithRoles.PolicyName, policy =>
            {
                foreach (var role in _policyWithRoles.Roles)
                {
                    policy.RequireRole(role);
                }
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors(x => x
            // .AllowAnyOrigin()
            .WithOrigins(GetAllowedOrigins(_configuration))
            .AllowAnyMethod()
            .AllowAnyHeader());


        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/whoami",
                (ClaimsPrincipal user) => Results.Ok(user.Claims.Select(c => new { c.Type, c.Value })));
        });
    }

    private static string[] GetAllowedOrigins(IConfiguration configuration) =>
        configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
}