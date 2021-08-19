## .NET 6 Minimal API

*.NET 6 comes with a new template for C# WebAPIs which is much more minimal. This exercise walks through setting up some endpoints in that template without using controllers.*

### Prerequisites:

[.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)

[Visual Studio Code](https://code.visualstudio.com/)

### Loose Agenda:
Set up APIs in the .NET 6 WebAPI template without controllers

### Step by Step

#### Setup Playground

Create a directory for today's exercise and navigate to it in a terminal instance.

Run `dotnet new webapi` then open the directory in Visual Studio Code.

#### Clean up Controllers

Delete the `Controllers` directory.

Delete `WeatherForecast.cs`.

Open `Program.cs`

Replace `builder.Services.AddControllers();` with `builder.Services.AddEndpointsApiExplorer();`

Move these lines out of the if-block

```C#
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "net_6_minimal_api v1"));
```

Remove `app.UseAuthorization();` and `app.MapControllers();`

#### Add a GET endpoint

Just above `app.Run();` let's add the following code:

```C# 
app.MapGet("/example", () => {
    return "This is an example of a GET endpoint";
});
```

Run the code via `dotnet run` in the terminal instance and navigate to [localhost:5001/swagger](https://localhost:5001/swagger)

#### Dependency Injection

To demonstrate dependency injection, let's create a class which we can inject and call for a result.

Create a new file named `DemoService.cs` with the following code:
```C#
public class DemoService
{
    public string NeedlesslyConsumeResources() {
        return "Non-Zero Memory Consumption";
    }
}
```

Back in `Program.cs` just below `var builder = WebApplication.CreateBuilder(args);` add the following line

```C#
builder.Services.AddScoped<DemoService>();
```

Now let's add another GET endpoint with the following code:
```C#
app.MapGet("/demo", (DemoService _service) => {
    return _service.NeedlesslyConsumeResources();
});
```

*Note that dependency injection is occuring naturally through the delegate parameter.*

Run the code via `dotnet run` in the terminal instance and navigate to [localhost:5001/swagger](https://localhost:5001/swagger)

#### ActionResults

Let's end by demonstrating ActionResults as a return type in minimal APIs

One more GET endpoint, but this one will be tempermental.

```C#
app.MapGet("/actionresults", (DemoService _service, bool acceptable) => {
    if(acceptable) {
        return Results.Ok(_service.NeedlesslyConsumeResources());
    }

    return Results.BadRequest("It's not okay to needlessly consume resources.");
});
```

Run the code via `dotnet run` in the terminal instance and navigate to [localhost:5001/swagger](https://localhost:5001/swagger)

### Additional Documentation

- [.NET Preview 7 Announcement](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-7/)
- [David Fowler's Minimal API repository](https://github.com/davidfowl/dotnet6minimalapi/blob/main/Dotnet6_Minimal_API/Program.cs)
