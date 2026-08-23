using Duende.IdentityServer;
using eShop.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace eShop.Identity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        var isBuilder = builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.KeyManagement.Enabled = false;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri = "http://localhost:9011";
                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
                options.Authentication.CookieSameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Authentication.CheckSessionCookieSameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            })
            .AddTestUsers(TestUsers.Users).AddDeveloperSigningCredential(persistKey: true, filename: "tempkey.jwk");

        // in-memory, code config
        isBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
        isBuilder.AddInMemoryApiScopes(Config.ApiScopes);
        isBuilder.AddInMemoryApiResources(Config.ApiResource);
        isBuilder.AddInMemoryClients(Config.Clients);

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("AllowAngular");
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapDefaultControllerRoute();
        app.MapRazorPages();

        return app;
    }
}