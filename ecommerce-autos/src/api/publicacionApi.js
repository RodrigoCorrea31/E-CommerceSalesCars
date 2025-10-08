import api from "./axiosConfig";

export const getPublicaciones = () => api.get("/publicacion");
export const getPublicacionById = (id) => api.get(`/publicacion/${id}`);

export const crearPublicacion = (data, token) =>
  api.post("/publicacion", data, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

export const uploadImagen = (file, token) => {
  const formData = new FormData();
  formData.append("file", file);
  return api.post("/publicacion/upload-imagen", formData, {
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "multipart/form-data",
    },
  });
};

export const editarPublicacion = (id, data) => api.put(`/publicacion/${id}`, data);
export const eliminarPublicacion = (id) => api.delete(`/publicacion/${id}`);
export const filtrarPublicaciones = (params) => api.get("/publicacion", { params });
