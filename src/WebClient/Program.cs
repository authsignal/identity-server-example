using System.IdentityModel.Tokens.Jwt;
using Authsignal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
      options.DefaultScheme = "Cookies";
      options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
      options.Authority = "https://localhost:5001";

      options.ClientId = "web";
      options.ClientSecret = "secret";
      options.ResponseType = "code";

      options.Scope.Clear();
      options.Scope.Add("openid");
      options.Scope.Add("profile");

      options.SaveTokens = true;
    });

builder.Configuration.AddJsonFile("appsettings.json");

var apiSecretKey = builder.Configuration.GetValue<string>("AuthsignalSecret")!;
var apiUrl = builder.Configuration.GetValue<string>("AuthsignalUrl");

builder.Services.AddSingleton<IAuthsignalClient>(_ => new AuthsignalClient(apiSecretKey: apiSecretKey, apiUrl: apiUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
