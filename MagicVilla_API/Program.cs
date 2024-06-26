
using MagicVilla_API.Datos;
using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IVillaRepository,VillaRepository>();
            builder.Services.AddScoped<INumeroVillaRepository, NumeroVillaRepository>();

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<Context>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Conexion"));
            });
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
