using System.Net.Http.Json;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public class PortfolioApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public const string TestDatabase = "PortfolioDb_Test";
    public const string GastEmail = "gast@example.test";
    public const string EditorEmail = "editor@example.test";
    public const string AdminEmail = "admin@example.test";

    public string Wachtwoord { get; } = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            var connection = new SqlConnectionStringBuilder(config.Build().GetConnectionString("Default"))
            {
                InitialCatalog = TestDatabase
            };

            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = connection.ConnectionString
            });
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
        ControleerTestDatabase(db);

        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        await MaakGebruikerAsync(userManager, GastEmail, "Gast");
        await MaakGebruikerAsync(userManager, EditorEmail, "Gast", "Editor");
        await MaakGebruikerAsync(userManager, AdminEmail, "Gast", "Admin");
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
            ControleerTestDatabase(db);
            await db.Database.EnsureDeletedAsync();
        }

        await base.DisposeAsync();
    }

    public async Task LeegDataAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
        ControleerTestDatabase(db);

        await db.Projects.ExecuteDeleteAsync();
        await db.Blogposts.ExecuteDeleteAsync();
    }

    public HttpClient MaakClient()
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public async Task<HttpClient> LoginAlsAsync(string email)
    {
        var client = MaakClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password = Wachtwoord
        });
        response.EnsureSuccessStatusCode();
        return client;
    }

    private async Task MaakGebruikerAsync(UserManager<User> userManager, string email, params string[] rollen)
    {
        var user = new User
        {
            UserName = email,
            Email = email
        };

        var result = await userManager.CreateAsync(user, Wachtwoord);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Testgebruiker {email} kon niet worden gemaakt.");
        }

        await userManager.AddToRolesAsync(user, rollen);
    }

    private static void ControleerTestDatabase(PortfolioDbContext db)
    {
        var database = db.Database.GetDbConnection().Database;
        if (database != TestDatabase)
        {
            throw new InvalidOperationException($"Tests gestopt: verwachtte {TestDatabase} maar kreeg {database}.");
        }
    }
}
