using Catalog.Application.Mapper;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data.Contexts;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options => 
//    {
//        options.Authority = "http://identityserver:9011";
//        options.RequireHttpsMetadata = true;

//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidIssuer = "http://identityserver:9011",
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidAudience = "Catalog",
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ClockSkew = TimeSpan.Zero
//        };
//        //Add this to docker to this communication
//        options.BackchannelHttpHandler = new HttpClientHandler
//        {
//            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
//        };
//        options.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine($"=====AUTHETICATE FAILED");
//                Console.WriteLine($"Exceptions:{context.Exception.Message}");
//                Console.WriteLine($"Authority:{options.Authority}");
//                return Task.CompletedTask;
//            }
//        };

//    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("CanRead", policy => policy.RequireClaim("scope", "catalogapi.read"));
//});

builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
builder.Services.AddMediatR(cgf => cgf.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    Assembly.GetAssembly(typeof(GetProductByIdQuery))));
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRepository, ProductRepository>();

//var userPolicy= new AuthorizationPolicyBuilder()
//    .RequireAuthenticatedUser().Build();
//builder.Services.AddControllers(config =>
//{
//    config.Filters.Add(new AuthorizeFilter(userPolicy));
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

builder.Services.AddApiVersioning(option =>
{
    option.ReportApiVersions = true;
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "This is API for Catalog Microservice in ecommerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ahmed Mohamed",
            Email = "ahmedmohamed00521@gmail.com",
            Url = new Uri("https://yourwebsite.eg")
        }
    });
});
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // BEFORE UseSwagger / routing
    app.Use((ctx, next) =>
    {
        if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var p) && !string.IsNullOrEmpty(p))
            ctx.Request.PathBase = p.ToString();   // e.g., "/catalog"
        return next();
    });

    app.UseSwagger(c =>
    {
        // Make the OpenAPI "servers" base path match the prefix so Try it out uses /catalog/...
        c.PreSerializeFilters.Add((doc, req) =>
        {
            var prefix = req.Headers["X-Forwarded-Prefix"].FirstOrDefault();
            if (!string.IsNullOrEmpty(prefix))
                doc.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
            { new() { Url = prefix } };
        });
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Catalog.API v1"); // relative path (no leading '/')
        c.RoutePrefix = "swagger";
    });

    app.UseSwagger();
    app.UseSwaggerUI();
    
}
app.UseCors("CorsPolicy");
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


