using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkWell.Domain.Entities;
using WorkWell.Infrastructure.Data;

namespace WorkWell.API;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WorkWellDbContext>();

        if (db.Database.IsRelational())
        {
            await db.Database.MigrateAsync();
        }
        else
        {
            await db.Database.EnsureCreatedAsync();
        }

        if (!await db.Empresas.AnyAsync())
        {
            var empresa = new Empresa
            {
                Nome = "WorkWell",
                Cnpj = "00000000000000",
                Setor = "Geral",
                DataCadastro = DateTime.UtcNow
            };

            db.Empresas.Add(empresa);
            await db.SaveChangesAsync();

            var departamento = new Departamento
            {
                Nome = "Geral",
                Descricao = "Departamento padrão",
                EmpresaId = empresa.Id
            };

            db.Departamentos.Add(departamento);
            await db.SaveChangesAsync();
        }
        else if (!await db.Departamentos.AnyAsync())
        {
            var empresaId = await db.Empresas.Select(e => e.Id).FirstAsync();
            var departamento = new Departamento
            {
                Nome = "Geral",
                Descricao = "Departamento padrão",
                EmpresaId = empresaId
            };

            db.Departamentos.Add(departamento);
            await db.SaveChangesAsync();
        }
    }
}
