import React, { useEffect, useState } from "react";
import Navbar from "../../components/layout/Navbar";
import Footer from "../../components/layout/Footer";
import { useAuth } from "../../context/AuthContext";
import { getMisPublicaciones, eliminarPublicacion } from "../../api/publicacionApi";
import { useNavigate } from "react-router-dom";
import "../../styles/MisPublicaciones.css";

export default function MisPublicaciones() {
  const { usuario } = useAuth();
  const [publicaciones, setPublicaciones] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [mostrarModal, setMostrarModal] = useState(false);
  const [publicacionSeleccionada, setPublicacionSeleccionada] = useState(null);
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

  const abrirModalEliminar = (publicacion) => {
    setPublicacionSeleccionada(publicacion);
    setMostrarModal(true);
  };

  const cerrarModal = () => {
    setMostrarModal(false);
    setPublicacionSeleccionada(null);
  };

  const confirmarEliminacion = async () => {
    try {
      const token = localStorage.getItem("token");
      await eliminarPublicacion(publicacionSeleccionada.id, token);
      setPublicaciones((prev) =>
        prev.filter((p) => p.id !== publicacionSeleccionada.id)
      );
      cerrarModal();
    } catch (err) {
      console.error(err);
      alert("Error al eliminar la publicación.");
    }
  };

  if (!usuario) {
    return (
      <div className="MisOfertasContainer">
        <Navbar />
        <div className="login-requerido-container">
          <h2>No has iniciado sesión</h2>
          <p>Debes iniciar sesión para ver tus publicaciones.</p>
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
                  <td className="acciones-publicacion">
                    <button
                      className="btn-ver-publicacion"
                      onClick={() => navigate(`/publicaciones/${pub.id}`)}
                    >
                      Ver
                    </button>
                    <button
                      className="btn-ver-ofertas"
                      onClick={() =>
                        navigate(`/mis-publicaciones/${pub.id}/ofertas`)
                      }
                    >
                      Ofertas
                    </button>
                    <button
                      className="btn-editar"
                      onClick={() => navigate(`/editar-publicacion/${pub.id}`)}
                    >
                      Editar
                    </button>
                    <button
                      className="btn-eliminar"
                      onClick={() => abrirModalEliminar(pub)}
                    >
                      Eliminar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {mostrarModal && (
        <div className="modal-overlay">
          <div className="modal-contenido">
            <h3>¿Eliminar publicación?</h3>
            <p>
              ¿Seguro que deseas eliminar la publicación "
              <strong>{publicacionSeleccionada?.titulo}</strong>"?
            </p>
            <div className="modal-botones">
              <button className="btn-cancelar" onClick={cerrarModal}>
                Cancelar
              </button>
              <button className="btn-confirmar" onClick={confirmarEliminacion}>
                Sí, eliminar
              </button>
            </div>
          </div>
        </div>
      )}

      <Footer />
    </div>
  );
}
