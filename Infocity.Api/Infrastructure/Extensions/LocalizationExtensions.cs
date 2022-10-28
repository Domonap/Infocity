#region

using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;

#endregion

namespace Infocity.Api.Infrastructure.Extensions;

public static class LocalizationExtensions
{
    public static RequestLocalizationOptions UseLocalization(this IApplicationBuilder app)
    {
        CultureInfo[] supportedCultures =
        {
            new("ru"),
            new("en")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        var requestProvider = new RouteDataRequestCultureProvider();
        localizationOptions.RequestCultureProviders.Insert(0, requestProvider);
        app.UseRequestLocalization(localizationOptions);

        return localizationOptions;
    }
}