using Commentaries.Api.Filters;
using Commentaries.Api.Utils;
using Commentaries.Application;
using Commentaries.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Commentaries.Api;

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
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        services
            .AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddOptions();
        services.AddHealthChecks();

        services.AddSwaggerGen(c =>
        {
            var version = VersionUtils.GetAssemblyFileVersion(Assembly.GetExecutingAssembly());

            c.SwaggerDoc($"v{version.GetMajorVersion()}",
                new OpenApiInfo { Title = Assembly.GetExecutingAssembly().GetName().Name, Version = version.GetMajorMinorVersion() });
            c.CustomSchemaIds(type => type.ToString());
            c.SupportNonNullableReferenceTypes();
        });

        services.AddInfrastructure(Configuration);
        services.AddApplication(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.UseEndpoints(endpoints => endpoints.MapHealthChecks("api/health"));

        app.UseSwagger();
        var assembly = Assembly.GetExecutingAssembly();
        var version = VersionUtils.GetAssemblyFileVersion(assembly);
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{assembly.GetName().Name} {version.GetMajorMinorVersion()}"); });
    }
}
