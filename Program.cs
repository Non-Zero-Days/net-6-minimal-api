using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<DemoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "net_6_minimal_api", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "net_6_minimal_api v1"));

app.UseHttpsRedirection();

app.MapGet("/example", () => {
    return "This is an example of a GET endpoints";
});

app.MapGet("/demo", (DemoService _service) => {
    return _service.NeedlesslyConsumeResources();
});

app.MapGet("/actionresults", (DemoService _service, [FromQuery]bool acceptable) => {
    if(acceptable) {
        return Results.Ok(_service.NeedlesslyConsumeResources());
    }

    return Results.BadRequest("It's not okay to needlessly consume resources.");
});

app.Run();
