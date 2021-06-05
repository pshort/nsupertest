using System;
using System.Dynamic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NSuperTest.Server;

namespace NSuperTest.Data
{
    public static class ServerExtensions
    {
        public static void SetupTestData<T>(this IServer server, string filePath)
            where T : DbContext
        {
            var services = server.GetServices();
            using var scoped = services.CreateScope();

            var context = scoped.ServiceProvider.GetRequiredService<T>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();

            var data = System.IO.File.ReadAllText(filePath);

            var json = JObject.Parse(data);

            var sets = context.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            (typeof(DbSet<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition())));

            foreach (var set in sets)
            {
                // get the collection for the db set as a jobject
                var tokens = json.SelectToken(set.Name);
                // get the generic type of the dbset
                var type = set.PropertyType.GetGenericArguments().First();

                foreach (var obj in tokens) // foreach thing in the collection
                {
                    // strong type it
                    var strong = obj.ToObject(type);
                    // stick it in the ctx
                    context.Add(strong);
                }
            }

            context.SaveChanges();
        }
    }
}