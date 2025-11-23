using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkWell.Infrastructure.Data;

namespace WorkWell.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Adicionar configuração específica para testes
            config.AddJsonFile("appsettings.Test.json", optional: true);
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:SqlServerConnection"] = "Data Source=:memory:;Mode=Memory;Cache=Shared",
                ["Jwt:SecretKey"] = "test-secret-key-for-unit-testing-purposes-only-32chars",
                ["Logging:LogLevel:Default"] = "Warning"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remover DbContext de produção
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<WorkWellDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Adicionar DbContext in-memory
            services.AddDbContext<WorkWellDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryWorkWellTests");
                options.EnableSensitiveDataLogging();
            });

            // Criar escopo para seed inicial
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<WorkWellDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                try
                {
                    db.Database.EnsureCreated();
                    SeedTestDatabase(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database with test data.");
                }
            }
        });

        builder.UseEnvironment("Test");
    }

    private void SeedTestDatabase(WorkWellDbContext db)
    {
        if (!db.Empresas.Any())
        {
            db.Empresas.Add(new WorkWell.Domain.Entities.Empresa
            {
                Nome = "Test Company",
                Cnpj = "12345678901234",
                Setor = "Test",
                DataCadastro = DateTime.UtcNow
            });
            db.SaveChanges();
        }
    }
}