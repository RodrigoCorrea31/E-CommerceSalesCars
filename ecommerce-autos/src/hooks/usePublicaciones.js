import { useState, useEffect } from "react";
import { obtenerPublicaciones, filtrarPublicaciones } from "../services/publicacionService";

export const usePublicaciones = () => {
  const [publicaciones, setPublicaciones] = useState([]);
  const [loading, setLoading] = useState(true);

  const cargarPublicaciones = async () => {
    setLoading(true);
    try {
      const data = await obtenerPublicaciones();
      setPublicaciones(data || []);
    } finally {
      setLoading(false);
    }
  };

  const aplicarFiltros = async (filtros) => {
    setLoading(true);
    try {
      const data = await filtrarPublicaciones(filtros);
      setPublicaciones(data || []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargarPublicaciones();
  }, []);

  return { publicaciones, loading, aplicarFiltros };
};
