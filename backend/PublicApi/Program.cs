using Microsoft.AspNetCore.HttpOverrides;
using PublicApi;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "yarp.json",
    optional: false,
    reloadOnChange: builder.Environment.IsDevelopment());

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("Yarp"))
    .AddConfigFilter<DaprConfigFilter>()
    .AddTransforms<DaprTransformProvider>();

builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:8500";
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();

builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapHealthChecks("/hc");

app.UseForwardedHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();

