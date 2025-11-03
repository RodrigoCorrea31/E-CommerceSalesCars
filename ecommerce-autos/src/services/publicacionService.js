import { getPublicaciones } from '../api/publicacionApi';
import api from '../api/axiosConfig';

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

export const filtrarPublicaciones = async (filtros) => {
  const params = {};
  Object.entries(filtros).forEach(([key, value]) => {
    if (value !== null && value !== "" && value !== undefined) {
      params[key] = value;
    }
  });

  const { data } = await api.get('/publicacion/filtrar', { params });

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
