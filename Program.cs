using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Unach.Codesi.Encuestas.Api.Mappings;
using Unach.Codesi.Encuestas.Persistencia.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Cadenas de Conexión
string connSqlServer = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EncuestasContext>
    (
        dbContextOptions => dbContextOptions
            .UseSqlServer(connSqlServer)
    );
#endregion

//builder.Services.AddAutoMapper(typeof(PerfilMappings));

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

#region Automapper

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new PerfilMappings());
});

IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
