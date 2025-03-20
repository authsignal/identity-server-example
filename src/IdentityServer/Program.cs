using IdentityServer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
  var builder = WebApplication.CreateBuilder(args);

  // Configure Kestrel to use specific ports
  builder.WebHost.ConfigureKestrel(options => {
      options.ListenLocalhost(5001, opts => opts.UseHttps());
  });

  builder.Host.UseSerilog((ctx, lc) => lc
      .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
      .Enrich.FromLogContext()
      .ReadFrom.Configuration(ctx.Configuration));

  var app = builder
      .ConfigureServices()
      .ConfigurePipeline();

  app.Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "Unhandled exception");
}
finally
{
  Log.Information("Shut down complete");
  Log.CloseAndFlush();
}