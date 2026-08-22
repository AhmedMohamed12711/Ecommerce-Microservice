using Common.Logging;
using Discount.API.Services;
using Discount.Application.Mapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extentions;
using Discount.Infrastructure.Repositories;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Discountprofile).Assembly);
builder.Services.AddMediatR(cgf => cgf.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    Assembly.GetAssembly(typeof(GetDiscountQuery))!));

builder.Services.AddScoped<IDiscountRepository,DiscountRepository>();
builder.Services.AddGrpc();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MigrationDatabase<Program>();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/",  async context =>
    {
        await context.Response.WriteAsync("Communication with grpc ednpoint must be made through a gprc client ");
    });
});

app.Run();
