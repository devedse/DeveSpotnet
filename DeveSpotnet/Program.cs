
using DeveSpotnet.Configuration;
using DeveSpotnet.Controllers.NewzNabApiControllerHelpers;
using DeveSpotnet.Db;
using DeveSpotnet.HostedServices;
using DeveSpotnet.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeveSpotnet
{
    public record DatabaseProvider(string Name, string Assembly);

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var sqliteProvider = new DatabaseProvider("Sqlite", typeof(Migrations.Sqlite.Marker).Assembly.GetName().Name!);
            var postgresProvider = new DatabaseProvider("Postgres", typeof(Migrations.Postgres.Marker).Assembly.GetName().Name!);

            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            // Bind the "UsenetSettings" section from configuration (including User Secrets) to the UsenetSettings POCO.
            builder.Services.Configure<UsenetSettings>(builder.Configuration.GetSection("UsenetSettings"));

            // Add services to the container.
            // Register the Usenet service as a singleton.
            builder.Services.AddSingleton<IUsenetService, UsenetService>();

            builder.Services.AddControllers()
                .AddXmlSerializerFormatters();

            // Register the custom filter (o=json or o=xml => override the Accept header)
            builder.Services.AddScoped<OverrideAcceptHeaderFilter>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen();

            builder.Services.AddHostedService<SpotRetrieverHostedService>();

            builder.Services.AddDbContext<DeveSpotnetDbContext>(options =>
            {
                var databaseProvider = configuration.GetValue<string>("DatabaseProvider");

                if (databaseProvider == sqliteProvider.Name)
                {
                    options.UseSqlite(configuration.GetConnectionString("Sqlite"), x => x.MigrationsAssembly(sqliteProvider.Assembly));
                }
                else if (databaseProvider == postgresProvider.Name)
                {
                    options.UseNpgsql(configuration.GetConnectionString("Postgres"), x => x.MigrationsAssembly(postgresProvider.Assembly));
                }
                else
                {
                    throw new InvalidOperationException($"Unknown database provider: {databaseProvider}");
                }
            });


            var app = builder.Build();

            // initialize database
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DeveSpotnetDbContext>();
                await db.Database.MigrateAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Use Swagger UI with your custom OpenAPI spec
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/openapi/v1.json", "API v1");
                c.RoutePrefix = string.Empty; // Optional: Makes Swagger UI available at the root
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
