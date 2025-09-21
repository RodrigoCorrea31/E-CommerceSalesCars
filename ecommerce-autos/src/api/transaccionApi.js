import api from "./axiosConfig";

export const finalizarTransaccion = (id) => api.post(`/transaccion/${id}/finalizar`);