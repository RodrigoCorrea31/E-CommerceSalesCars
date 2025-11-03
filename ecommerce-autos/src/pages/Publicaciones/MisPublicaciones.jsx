import React, { useEffect, useState } from "react";
import Navbar from "../../components/layout/Navbar";
import Footer from "../../components/layout/Footer";
import { useAuth } from "../../context/AuthContext";
import { getMisPublicaciones } from "../../api/publicacionApi";
import { useNavigate } from "react-router-dom";
import "../../styles/MisPublicaciones.css";

export default function MisPublicaciones() {
  const { usuario } = useAuth();
  const [publicaciones, setPublicaciones] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPublicaciones = async () => {
      try {
        setLoading(true);
        const token = localStorage.getItem("token");
        const res = await getMisPublicaciones(token);
        setPublicaciones(res.data);
      } catch (err) {
        console.error(err);
        setError("Error al cargar tus publicaciones.");
      } finally {
        setLoading(false);
      }
    };

    if (usuario) {
      fetchPublicaciones();
    }
  }, [usuario]);

  if (!usuario) {
    return (
      <div className="MisOfertasContainer">
        <Navbar />
        <div className="login-requerido-container">
          <h2>No has iniciado sesión</h2>
          <p>Debes iniciar sesión para ver tus ofertas realizadas.</p>
          <button
            className="btn-ir-login"
            onClick={() =>
              navigate(`/login?redirect=${encodeURIComponent(location.pathname)}`)
            }
          >
            Ir al login
          </button>
        </div>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="MisPublicacionesContainer">
        <Navbar />
        <div className="mis-publicaciones-container">
          <h2>Cargando tus publicaciones...</h2>
        </div>
        <Footer />
      </div>
    );
  }

  if (error) {
    return (
      <div className="MisPublicacionesContainer">
        <Navbar />
        <div className="mis-publicaciones-container">
          <h2>{error}</h2>
        </div>
        <Footer />
      </div>
    );
  }

  return (
    <div className="MisPublicacionesContainer">
      <Navbar />
      <div className="mis-publicaciones-container">
        <h2>Mis Publicaciones</h2>

        {publicaciones.length === 0 ? (
          <p>No has realizado publicaciones todavía.</p>
        ) : (
          <table className="tabla-publicaciones">
            <thead>
              <tr>
                <th>Título</th>
                <th>Precio</th>
                <th>Estado</th>
                <th>Fecha</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {publicaciones.map((pub) => (
                <tr key={pub.id}>
                  <td>{pub.titulo}</td>
                  <td>${pub.precio?.toLocaleString()}</td>
                  <td>
                    <span
                      className={`estado ${
                        typeof pub.estado === "string"
                          ? pub.estado.toLowerCase()
                          : pub.estado === 0
                          ? "activo"
                          : pub.estado === 1
                          ? "vendido"
                          : "pausado"
                      }`}
                    >
                      {typeof pub.estado === "string"
                        ? pub.estado
                        : pub.estado === 0
                        ? "Activo"
                        : pub.estado === 1
                        ? "Vendido"
                        : "Pausado"}
                    </span>
                  </td>
                  <td>
                    {pub.fecha
                      ? new Date(pub.fecha).toLocaleDateString()
                      : "Sin fecha"}
                  </td>
                  <td>
                    <button
                      className="btn-ver-publicacion"
                      onClick={() => navigate(`/publicaciones/${pub.id}`)}
                    >
                      Ver Publicacion
                    </button>
                    <button
                      className="btn-ver-ofertas"
                      onClick={() => navigate(`/mis-publicaciones/${pub.id}/ofertas`)}
                    >
                      Ver Ofertas
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
      <Footer />
    </div>
  );
}
