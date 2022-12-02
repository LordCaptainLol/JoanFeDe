namespace TechnicalTest.API
{
    using Microsoft.EntityFrameworkCore;
    using TechnicalTest.Infrastructure;

    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetService<TestDbContext>()?.Database.Migrate();
            }

            return app;
        }
    }
}
