import { getPublicacionById } from "../api/publicacionApi";

export const obtenerDetallePublicacion = async (idPublicacion) => {
  try {
    const { data } = await getPublicacionById(idPublicacion);

    const vendedor = data.vendedor
      ? {
          nombre: data.vendedor.name,
          email: data.vendedor.email,
          telefono: data.vendedor.telefono ?? "No disponible",
          reputacion: 0,
          cantidadVentas: 0,
        }
      : null;

    return {
      id: data.id,
      titulo: data.titulo,
      precio: data.precio,
      descripcion: data.descripcion,
      esUsado: data.esUsado,
      vehiculo: {
        marca: data.vehiculo?.marca ?? "N/D",
        modelo: data.vehiculo?.modelo ?? "N/D",
        anio: data.vehiculo?.anio ?? 0,
        color: data.vehiculo?.color ?? "No especificado",
        kilometraje: data.vehiculo?.kilometraje ?? 0,
        combustible: data.vehiculo?.combustible ?? 0,
        imagenes: data.vehiculo?.imagenes?.map(i => i.url) ?? [],
      },
      vendedor,
    };
  } catch (error) {
    console.error("Error obteniendo detalle de publicación:", error);
    throw error;
  }
};
