using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
  public class Usuario
  {
    [Key
      ] public int Id { get; set; }

        public string Nombre { get; set; }

       public string Apellido { get; set; }

       public string Correo { get; set; }

       public string Password { get; set; }

      public string  Passwordv { get; set; }

      public string Token { get; set; }

     public string Role { get; set; }


  }
}
