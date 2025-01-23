using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crud.Core.Entities;   

namespace Crud.Core.Entities
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Genero { get; set; }

        public DateTime FechaPublicacion { get; set; }

        [ForeignKey("Usuarios")]
        public int UsuarioId { get; set; }
        public Usuarios Usuario { get; set; }
    }
}
