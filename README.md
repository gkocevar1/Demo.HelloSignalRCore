# Hello SignalR Core
SignalR Core Demo

> First we'll create a standard 'Hello World' Web API with a Values controller.  
> Then we'll add SignalR for duplex communication, so that actions in the Values controller 
> 'push' results to the client after a specific time interval.  
> This will enable asynchonicity, for example, using queues.

## Create Web API
1. Create Server Web API
    ```
    cd server
    dotnet new webapi
    ```
2. Install Code Generation Tools
   - Edit server.csproj file by adding the following item group:
    ```xml
    <ItemGroup>
      <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.1.0-preview1-final" />
    </ItemGroup>
    ```
    - Add the following package reference:
    ```xml
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="2.1.1" />
    ```
    - Restore dependencies: `dotnet restore`
    - Build the server project: `dotnet build`
    - Add Customizable razor templates
    ```
    dotnet new -i "AspNetCore.WebApi.Templates::*"
    dotnet new webapi-templates
    ```
3. Update ValuesController
    - Add new ValuesController
    ```
    dotnet aspnet-codegenerator controller -name Values -api -actions -outDir Controllers -f
    ```
4. Run the Web API
    ```
    dotnet run
    ```
    - Browse to https://localhost:5001/api/values

## Add SignalR Core
1. Update `ConfigureServices` method in the `Startup` class
    ```csharp
    services.AddSignalR();
    ```
2. Update `Configure` method.
    ```csharp
    app.UseSignalR((options) => {
        options.MapHub<ValuesHub>("/Hubs/Values");
    });
    ```
3. Create Hubs folder and add IValuesClient.cs
    ```csharp
    public interface IValuesClient
    {
        Task Get(string message);
    }
    ```
4. Add ValuesHub.cs to Hubs folder
    ```csharp
    public class ValuesHub : Hub<IValuesClient>
    {
        public async Task Get(string message)
        {
            await Clients.Caller.Get(message);
        }
    }
    ```
5. Remove all methods accept second `Get` method, 
   then refactor `Get` method to accept a message parameter from the URI.
    ```csharp
    // GET: api/Values/message
    [HttpGet("{message}")]
    public IActionResult Get(string message)
    {
        return Ok(message);
    }
    ```
