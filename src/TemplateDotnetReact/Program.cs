using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;
using TemplateDotnetReact.Development;

bool exportOpenApi = args.Contains("--export-openapi", StringComparer.Ordinal);
var builder = WebApplication.CreateBuilder(args.Where(arg => arg != "--export-openapi").ToArray());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SupportNonNullableReferenceTypes());

if (!exportOpenApi &&
    builder.Environment.IsDevelopment() &&
    builder.Configuration.GetValue<bool>("ClientApp:StartDevServer"))
{
    builder.Services.AddHostedService<ClientAppDevServer>();
}

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/greeting", () => TypedResults.Ok(new Greeting("ASP.NET Core からこんにちは")))
    .WithName("GetGreeting");

app.MapFallback("/api/{**path}", () => Results.NotFound());
app.MapFallbackToFile("index.html");

if (exportOpenApi)
{
    // Start routing so Swagger can discover minimal APIs. Use an ephemeral loopback port.
    app.Urls.Clear();
    app.Urls.Add("http://127.0.0.1:0");
    await app.StartAsync();

    var document = app.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1");
    var schemaPath = Path.Combine(app.Environment.ContentRootPath, "clientapp", "openapi.json");
    using (var writer = File.CreateText(schemaPath))
    {
        document.SerializeAsV3(new OpenApiJsonWriter(writer));
    }

    await app.StopAsync();
    return;
}

await app.RunAsync();

internal sealed record Greeting(string Message);

// Expose the entry point to WebApplicationFactory.
public partial class Program;
