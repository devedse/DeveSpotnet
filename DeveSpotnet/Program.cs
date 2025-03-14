
using DeveSpotnet.Configuration;
using DeveSpotnet.Services;

namespace DeveSpotnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind the "UsenetSettings" section from configuration (including User Secrets) to the UsenetSettings POCO.
            builder.Services.Configure<UsenetSettings>(builder.Configuration.GetSection("UsenetSettings"));

            // Add services to the container.
            // Register the Usenet service as a singleton.
            builder.Services.AddSingleton<IUsenetService, UsenetService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen();


            var app = builder.Build();

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
