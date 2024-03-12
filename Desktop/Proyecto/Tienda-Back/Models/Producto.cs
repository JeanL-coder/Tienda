using System;
using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
  public class Producto
  {
    [Key]
    public int ID { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int CategoriaID { get; set; }
    public int ProveedorID { get; set; }
    public byte[] Imagen { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public int Busquedas { get; set; }
  }
}

