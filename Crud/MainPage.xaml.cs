using Microsoft.EntityFrameworkCore;
using Crud.Data;
using System.Diagnostics;

namespace Crud
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
           

            InitializeComponent();
            VerificarUsuarios();
        }

        private void VerificarUsuarios()
        {

                using (var context = new AppDbContext(new DbContextOptions<AppDbContext>()))
                {
                    var usuarios = context.Usuarios.ToList();
                    Debug.WriteLine($"Usuarios encontrados: {usuarios.Count}");
                }
           
        }



    }

}
