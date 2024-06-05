using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServicesRegistrations(builder.Configuration, builder.Environment);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

builder.Services.AddCarter();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddDaprClient();

var app = builder.Build();

app.MapHealthChecks("/hc");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.MapSubscribeHandler();

app.Run();
