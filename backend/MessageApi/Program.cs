using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServicesRegistrations(builder.Configuration, builder.Environment);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t =>
        t.FullName!
         .Replace("MessageApi", string.Empty)
         .Replace(".", string.Empty));
});

builder.Services.AddHealthChecks();

builder.Services.AddCarter();

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