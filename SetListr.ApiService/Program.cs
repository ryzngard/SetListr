using System.Security.Claims;

using SetListr.ApiService;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<SetListManager>();

// Configure Auth
builder.Services.AddAuthentication()
                .AddKeycloakJwtBearer(
                    serviceName: "keycloak",
                    realm: "SetListr",
                    options =>
                    {
                        options.Audience = "setlistr.api";
                        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
                    });

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// app.MapGet("/bands", () => [band]).WithName("GetBands");

var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("SetListr.ApiService");

app.MapGet("/songs", async (HttpContext httpContext) =>
{
    var setListManager = httpContext.RequestServices.GetRequiredService<SetListManager>();
    var bandId = httpContext.Request.Query["bandId"];

    logger.LogInformation($"Getting songs for band {bandId}");

    if (Guid.TryParse(bandId, out var id) &&
        setListManager.GetSongsForBand(id) is { } songs)
    {
        return Results.Ok(songs);
    }

    return Results.BadRequest();
})
.WithName("GetSongsForBand");

app.MapGet("/setLists", async (HttpContext httpContext) =>
{
    var setListManager = httpContext.RequestServices.GetRequiredService<SetListManager>();
    var bandId = httpContext.Request.Query["bandId"];

    logger.LogInformation($"Getting set lists for band {bandId}");

    if (Guid.TryParse(bandId, out var id) &&
        setListManager.GetSetListsForBand(id) is { } setListsForBand)
    {
        return Results.Ok(setListsForBand);
    }

    var user = httpContext.User;

    logger.LogInformation($"Getting set lists for user :: {user.Identity?.Name}");
    var username = GetUsername(user);
    if (!string.IsNullOrEmpty(username)
        && setListManager.GetSetListsForUser(username) is { } setListsForUser)
    {
        return Results.Ok(setListsForUser);
    }

    return Results.Ok(setListManager.GetSetLists());
})
.WithName("GetSetListsFromBand")
.RequireAuthorization();

app.MapGet("/setList/{id}", async (HttpContext httpContext) =>
{
    var setListManager = httpContext.RequestServices.GetRequiredService<SetListManager>();
    var id = httpContext.Request.RouteValues["id"]?.ToString();

    logger.LogInformation($"Getting set list {id}");

    if (Guid.TryParse(id, out var setId) &&
        setListManager.GetSetList(setId) is { } setList)
    {
        return Results.Ok(setList);
    }

    return Results.BadRequest();
})
.WithName("GetSetList")
.RequireAuthorization();

app.MapDefaultEndpoints();

app.Run();


static string? GetUsername(ClaimsPrincipal user)
{
    if (user is not { Identity: { IsAuthenticated: true } })
    {
        return null;
    }

    return user.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
}