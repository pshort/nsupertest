using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSuperTest.Registration.NetCoreServer;

namespace NSuperTest.Data
{
    public static class RegistrationExtensions
    {
        public static NetCoreServerBuilder<T> WithInMemoryContext<CX, T>(this NetCoreServerBuilder<T> builder)
            where T : class
            where CX : DbContext
        {
            builder.WithServices(services =>
            {
                var desc = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CX>));
                services.Remove(desc);
                
                services.AddDbContext<CX>(opts =>
                {
                    opts.UseSqlite("DataSource=:memory:?cache=shared");
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<CX>();

                db.Database.EnsureCreated();
            });
            return builder;
        }
    }
}