import api from "./axiosConfig";

export const crearOferta = (data) => api.post("/ofertas", data);
export const aceptarOferta = (id) => api.post(`/ofertas/${id}/aceptar`);
export const rechazarOferta = (id) => api.post(`/ofertas/${id}/rechazar`);
