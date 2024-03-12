
export interface Producto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
  categoriaId: number;
  proveedorId: number;
  imagen: string; 
  fechaCreacion: Date;
  fechaActualizacion: Date;
  busquedas: number;
  
}
