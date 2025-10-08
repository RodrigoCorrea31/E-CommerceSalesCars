import { useEffect, useState } from "react";
import { obtenerPublicaciones } from "../services/publicacionService";

export const usePublicaciones = () => {
  const [publicaciones, setPublicaciones] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await obtenerPublicaciones();
        setPublicaciones(data);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  return { publicaciones, loading };
};
