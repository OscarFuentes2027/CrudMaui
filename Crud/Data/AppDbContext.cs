using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Crud.Models;
using System.Diagnostics;

namespace Crud.Data
{

    public class AppDbContext : DbContext
    {
        // Aqui defino una tabla llamada Usuarios utilizando el modelo Usuarios
        public DbSet<Usuarios> Usuarios { get; set; }

        // Aqui defino el constructor de la clase
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Aqui estoy configurando la base de datos cd 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MauiCrudApp.db");
                Debug.WriteLine($"xixixixixixixi Usando base de datos en: {Path.GetFullPath(dbPath)}");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }



    }
}
