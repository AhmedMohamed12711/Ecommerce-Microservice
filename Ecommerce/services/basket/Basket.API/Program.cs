using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Mapper;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();


builder.Services.AddAutoMapper(typeof(BasketMappingProfile).Assembly);
builder.Services.AddMediatR(cgf => cgf.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    Assembly.GetAssembly(typeof(CreateShoppingCartCommand))!));

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    cg => cg.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!));
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ct, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddApiVersioning(option =>
{
    option.ReportApiVersions = true;
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl= true;

});
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "http://identityserver:9011";
//        options.RequireHttpsMetadata = false;

//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidIssuer = "http://identityserver:9011",
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidAudience = "Basket",
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



builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v1",
        Description = "This is API for Basket Microservice in ecommerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ahmed Mohamed",
            Email = "ahmedmohamed00521@gmail.com",
            Url = new Uri("https://yourwebsite.eg")
        }
    });


    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v2",
        Description = "This is API for Basket Microservice v2 in ecommerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ahmed Mohamed",
            Email = "ahmedmohamed00521@gmail.com",
            Url = new Uri("https://yourwebsite.eg")
        }
    });
    options.DocInclusionPredicate((version, apidescription) =>
    {
        if (!apidescription.TryGetMethodInfo(out var methodInfo))
        {
            return false;
        }
        var versions = methodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

        return versions?.Any(v => $"v{v.MajorVersion}" == version || v.ToString() == version) ?? false;
    });

});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});
//var userPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser().Build();
//builder.Services.AddControllers(config =>
//{
//    config.Filters.Add(new AuthorizeFilter(userPolicy));
//});
//redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CachSettings:ConnectionString");
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders =ForwardedHeaders.XForwardedFor|ForwardedHeaders.XForwardedProto
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.Use((ctx, next) =>
    {
        if(ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix",out var prefix)&&
        !string.IsNullOrEmpty(prefix))
        {
            ctx.Request.PathBase = prefix.ToString();
        }
        return next();
    });
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Basket.API v1");
        c.SwaggerEndpoint("v2/swagger.json", "Basket.API v2");
        c.RoutePrefix = "swagger";
    });
}

//app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
