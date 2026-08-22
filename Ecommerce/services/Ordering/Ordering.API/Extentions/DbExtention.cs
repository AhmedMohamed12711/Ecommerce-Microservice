using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Runtime.CompilerServices;

namespace Ordering.API.Extentions;

public static class DbExtention
{
    public static IHost Migratedatabase<Tcontext>(this IHost host,Action<Tcontext,IServiceProvider> seeder) where Tcontext :DbContext
    {
        using (var scope = host.Services.CreateScope())
        { 
            var service= scope.ServiceProvider;
            var logger= service.GetService<ILogger<Tcontext>>();
            var context= service.GetService<Tcontext>();

            try
            {
                logger.LogInformation($"Started db migration :{typeof(Tcontext).Name}");
                var retry = Policy.Handle<SqlException>().
                    WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: retryAttemps => TimeSpan.FromSeconds(Math.Pow(2, retryAttemps)),
                    onRetry: (exception, span, count) =>
                    {
                        logger.LogInformation($"Retrying beacuase of {exception} {span}");
                    }
                    );
                retry.Execute(() => Callseeder(context, seeder, service));
                logger.LogInformation($"Finshed db migration :{typeof(Tcontext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"An error occured while migration db :{typeof(Tcontext).Name}");
            }
                return host;

        }
    }

    private static void Callseeder<Tcontext>(Tcontext? context, Action<Tcontext, IServiceProvider> seeder, IServiceProvider service) where Tcontext : DbContext
    {
       context.Database.Migrate();
       seeder(context,service);
    }
}
