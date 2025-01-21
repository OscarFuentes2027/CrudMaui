using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data
{

    public class AppDbContext : DbContext
    {
        // Aqui defino una tabla llamada Usuarios utilizando el modelo Usuarios
        public DbSet<Models.Usuarios> Usuarios { get; set; }

        // Aqui defino el constructor de la clase
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Aqui estoy configurando la base de datos 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Filename={System.IO.Path.Combine(FileSystem.AppDataDirectory, "MauiCrudApp.db")}");

            }
        }
    }
}
