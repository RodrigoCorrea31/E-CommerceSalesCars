import { getPublicaciones } from '../api/publicacionApi';

export const obtenerPublicaciones = async () => {
  const { data } = await getPublicaciones();
  return data.map(pub => ({
    id: pub.id,
    titulo: pub.titulo,
    precio: pub.precio,
    estado: pub.estado,
    marca: pub.vehiculo?.marca ?? "N/D",
    modelo: pub.vehiculo?.modelo ?? "N/D",
    anio: pub.vehiculo?.anio ?? 0,
    kilometraje: pub.vehiculo?.kilometraje ?? 0,
    combustible: pub.vehiculo?.combustible ?? 0,
    color: pub.vehiculo?.color ?? "No especificado",
    imagenes: pub.vehiculo?.imagenes?.map(img => img.url) ?? []
  }));
};
