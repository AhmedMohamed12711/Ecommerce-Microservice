using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extentions;

public static class DbExtention
{
    public static IHost MigrationDatabase<TContext>(this IHost host)
    {
        using(var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Discount DB Migration Started");
                ApplyMigrations(config);
                logger.LogInformation("Discount DB Migration Completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Can't Create Database Migrations");
            }
        }
        return host;
     
    }

    private static void ApplyMigrations(IConfiguration config)
    {
        var retry=5;
        while (retry > 0)
        {
            try
            {
                using var connection =
                    new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();
                using var cmd = new NpgsqlCommand
                {
                    Connection=connection
                };
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Coupon (ID SERIAL PRIMARY KEY,
                                                                       ProductName VARCHAR(500) NOT NULL,
                                                                       Description TEXT,
                                                                       Amount INT   )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT COUNT(*) FROM Coupon";
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    cmd.CommandText = "INSERT INTO Coupon (ProductName,Description,Amount) VALUES('Egypt Adidas Quick Force Indoor Badminton Shoes','Adidas Discount',600)";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Coupon (ProductName,Description,Amount) VALUES('PowerFit 19 FH Rubber Spike Cricket Shoes','PowerFit Discount',700)";
                    cmd.ExecuteNonQuery();
                }
                break;
            }
            catch (Exception ex)
            {
                retry--;
                if(retry == 0)
                {
                    throw;
                }
                Thread.Sleep(2000);
            }
        }
    }
}
