using Azure.DigitalTwins.Core;
using Azure.Identity;

namespace Teqniqly.AzureDigitalTwinsQueryApi
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public partial class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddProblemDetails();

            builder.Services.AddSingleton(sp =>
            {
                var tokenCredential = new DefaultAzureCredential();

                return new DigitalTwinsClient(
                                       new Uri(builder.Configuration["DigitalTwinsEndpoint"]!),
                                                          tokenCredential);
            });

            builder.Services.AddScoped<IAzureDigitalTwinService, AzureDigitalTwinService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();
            app.UseRouting();

            app.MapGet("/api", () =>
            {
                return Results.Ok("Azure Digital Twins Query API");
            });

            app.MapPost("/api/q", (IAzureDigitalTwinService service, QueryRequest request) =>
            {
                return service.Query(request.Query);
            });

            app.Run();
        }
    }
}
