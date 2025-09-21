import api from "./axiosConfig";

export const getPublicaciones = () => api.get("/publicacion");
export const getPublicacionById = (id) => api.get(`/publicacion/${id}`);
export const crearPublicacion = (data) => api.post("/publicacion", data);
export const editarPublicacion = (id, data) => api.put(`/publicacion/${id}`, data);
export const eliminarPublicacion = (id) => api.delete(`/publicacion/${id}`);
export const filtrarPublicaciones = (params) => api.get("/publicacion", {params});