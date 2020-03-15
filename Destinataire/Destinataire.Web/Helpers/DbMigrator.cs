using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Destinataire.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Destinataire.Web.Helpers
{
    public static class DbMigrator
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<DestinaireContext>())
                {
                    try
                    {
                        var migrations = appContext.Database.GetPendingMigrations();
                        if (migrations.Any())
                        {
                            appContext.Database.Migrate();
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }

            return host;
        }
    }
}