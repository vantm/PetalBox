using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t =>
        t.FullName!
         .Replace("OrderApi", string.Empty)
         .Replace(".", string.Empty));
});

builder.Services.AddHealthChecks();

builder.Services.AddCarter();

builder.Services.AddServicesRegistrations(builder.Configuration, builder.Environment);

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
