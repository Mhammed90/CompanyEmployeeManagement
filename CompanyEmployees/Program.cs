using CompanyEmployees;
using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using CompanyEmployees.Presentation;
using CompanyEmployees.Presentation.ActionFilters;
using Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.ConfigureLoggerService();
// ///swageer
// builder.Services.AddSwaggerGen(c =>
// {
//     // Define Swagger doc version
//     c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//     {
//         Title = "CompanyEmployees API",
//         Version = "v1"
//     });
// });
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddOpenApi();
builder.Services.ConfigureCors();
builder.Services.ConfigureRepositories();
builder.Services.AddAutoMapper(typeof(Program));  
builder.Services.AddScoped<ValidationFilterAttribute>();

// for enable custom response 
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    })
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
builder.Services.ConfigureIisIntegration();
builder.Services.ConfigureServices();
builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
// Configure the HTTP request pipeline.

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi(); 
 //   app.UseSwagger(); // <-- This actua/lly generates swagger.json
    //
    // app.UseSwaggerUI(c =>
    // {
    //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyEmployees API v1");
    //     c.RoutePrefix = string.Empty; // Swagger UI at http://localhost:5287
    // });
}
if (app.Environment.IsProduction()) app.UseHsts();
app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();