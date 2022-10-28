 

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
 
namespace Infocity.Api.Infrastructure.Extensions;

 

public static class ControllerExtensions
{
    public static void AddControllerConfiguration(this IServiceCollection services, string dateFormat)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage));

                    return new BadRequestObjectResult(errors);
                };
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateFormatString = dateFormat;
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                };
            })
            .AddSessionStateTempDataProvider();
    }
}