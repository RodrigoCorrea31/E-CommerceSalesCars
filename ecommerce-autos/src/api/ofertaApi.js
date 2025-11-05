import api from "./axiosConfig";

export const crearOferta = (data, token) =>
  api.post("/ofertas", data, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  export const eliminarOferta = async (id) => {
  const token = localStorage.getItem("token");

  const response = await api.delete(`/ofertas/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return response.data;
};


export const getOfertaById = (id, token) =>
  api.get(`/ofertas/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });


export const aceptarOferta = async (id) => {
  const token = localStorage.getItem("token");

  const response = await api.post(`/ofertas/${id}/aceptar`, null, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return response.data;
};

export const rechazarOferta = async (id) => {
  const token = localStorage.getItem("token");

  const response = await api.post(`/ofertas/${id}/rechazar`, null, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return response.data;
};
