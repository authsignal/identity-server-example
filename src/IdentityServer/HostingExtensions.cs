using Authsignal;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
  public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
  {
    // uncomment if you want to add a UI
    builder.Services.AddRazorPages();

    builder.Services.AddIdentityServer(options =>
        {
          // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
          options.EmitStaticAudienceClaim = true;
        })
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryClients(Config.Clients)
        .AddTestUsers(TestUsers.Users);

    builder.Configuration.AddJsonFile("appsettings.json");

    var apiUrl = builder.Configuration.GetValue<string>("AuthsignalUrl");
    var apiSecretKey = builder.Configuration.GetValue<string>("AuthsignalSecret");

    builder.Services.AddSingleton<IAuthsignalClient>(_ => new AuthsignalClient(apiSecretKey: apiSecretKey, apiUrl: apiUrl));

    return builder.Build();
  }

  public static WebApplication ConfigurePipeline(this WebApplication app)
  {
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    // uncomment if you want to add a UI
    app.UseStaticFiles();
    app.UseRouting();

    app.UseIdentityServer();

    // uncomment if you want to add a UI
    app.UseAuthorization();
    app.MapRazorPages().RequireAuthorization();

    return app;
  }
}
