import api from "./axiosConfig";

export const registrarUsuario = (data) => api.post("/usuarios/registrar", data);
export const loginUsuario = async (data) => {
  const response = await api.post("/usuarios/login", data);
  return response.data; 
};export const obtenerOfertasUsuario = (id) => api.get(`/usuarios/${id}/ofertas`);
export const obtenerComprasUsuario = (id) => api.get(`/usuarios/${id}/compras`);
export const obtenerVentasUsuario = (id) => api.get(`/usuarios/${id}/ventas`);