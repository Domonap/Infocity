using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infocity.Api.Dal.Repositories;
using Infocity.Api.Domain.Configurations;
using Infocity.Api.Domain.Contracts;
using Infocity.Api.Infrastructure.Authentications;
using Infocity.Api.Infrastructure.Extensions;
using Infocity.Api.Infrastructure.Factories;
using Infocity.Api.Infrastructure.Providers;
using Infocity.Api.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var assemblyConfigurationAttribute = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
var configurationName = assemblyConfigurationAttribute?.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{configurationName}.json", true)
    .Build();

builder.Services.AddOptions()
    .Configure<DatabaseConfiguration>(configuration.GetSection(nameof(DatabaseConfiguration)))
    .AddSingleton(configuration);


var dateFormat = configuration.GetSection("DateFormat").Value;
var databaseConfiguration = configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();


// Add services to the container. 

builder.Services.AddCors();
builder.Services.AddCorsPolicy();
builder.Services.AddControllerConfiguration(dateFormat);
builder.Services.AddDb(databaseConfiguration);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<ICredentialsProvider, CredentialsProvider>();
builder.Services.AddTransient<IObjectResultFactory, ObjectResultFactory>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();


builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


if (!builder.Environment.IsProduction())
    builder.Services.AddSwagger();

var app = builder.Build();


if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
else if (app.Environment.IsStaging())
    app.UseForwardedHeaders();

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
    app.UseStaticFiles();
    app.UseSwaggerConfiguration();
}
else
{
    app.UseHsts();
}

app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseRequestLocalization(app.UseLocalization());


await app.RunAsync();