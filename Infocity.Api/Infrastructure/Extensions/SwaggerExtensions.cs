#region

using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

#endregion

namespace Infocity.Api.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.TagActionsBy(api => new List<string> {api.GroupName});

            c.SwaggerDoc("service", new OpenApiInfo
            {
                Version = "v1",
                Title = "Демонстрационный API",
                Description = "Демонстрационный API для ООО Городской информационный центр Инфосити ",
                Contact = new OpenApiContact {Name = "Gia Kvaratskeliya", Email = "gia@domonap.ru"},
                License = new OpenApiLicense
                    {Name = "Use under GNU v.3", Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.ru.html")}
            });

            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header - Username and password"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.DescribeAllParametersInCamelCase();
        });

        services.AddSwaggerGenNewtonsoftSupport();
    }

    public static void UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseStaticFiles().UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/service/swagger.json", "Демонстрационный API");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "Демонстрационный API";
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableFilter();
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.InjectJavascript("/swagger-ui/custom.js");
            });
    }
}