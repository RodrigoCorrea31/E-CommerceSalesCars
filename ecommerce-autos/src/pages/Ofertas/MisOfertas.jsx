import React, { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { obtenerOfertasUsuario } from "../../api/usuarioApi";
import { eliminarOferta } from "../../api/ofertaApi";
import Navbar from "../../components/layout/Navbar";
import Footer from "../../components/layout/Footer";
import { useNavigate, useLocation } from "react-router-dom";
import "../../styles/MisOfertas.css";
import "../../styles/FormPublicacionNoLogueado.css";

export default function MisOfertas() {
  const { usuario } = useAuth();
  const [ofertas, setOfertas] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [modalInfoVisible, setModalInfoVisible] = useState(false);
  const [ofertaSeleccionada, setOfertaSeleccionada] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const cargarOfertas = async () => {
      try {
        if (!usuario) {
          setCargando(false);
          return;
        }

        const response = await obtenerOfertasUsuario(usuario.id);
        setOfertas(response.data);
      } catch (err) {
        console.error("Error obteniendo ofertas:", err);
        setError("No se pudieron cargar las ofertas.");
      } finally {
        setCargando(false);
      }
    };

    cargarOfertas();
  }, [usuario]);

  const abrirModalEliminar = (oferta) => {
    if (oferta.estado.toLowerCase() === "aceptada") {
      setOfertaSeleccionada(oferta);
      setModalInfoVisible(true); 
      return;
    }
    setOfertaSeleccionada(oferta);
    setModalVisible(true);
  };

  const cerrarModal = () => {
    setModalVisible(false);
    setOfertaSeleccionada(null);
  };

  const cerrarModalInfo = () => {
    setModalInfoVisible(false);
    setOfertaSeleccionada(null);
  };

  const confirmarEliminar = async () => {
    try {
      await eliminarOferta(ofertaSeleccionada.id);
      setOfertas((prev) =>
        prev.filter((oferta) => oferta.id !== ofertaSeleccionada.id)
      );
      cerrarModal();
    } catch (err) {
      console.error("Error eliminando la oferta:", err);
      alert("Ocurrió un error al eliminar la oferta.");
      cerrarModal();
    }
  };

  if (cargando) return <p>Cargando tus ofertas...</p>;
  if (error) return <p>{error}</p>;

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

  return (
    <div className="MisOfertasContainer">
      <Navbar />
      <div className="mis-ofertas-container">
        <h2>Mis Ofertas Realizadas</h2>

        {ofertas.length === 0 ? (
          <p>No realizaste ninguna oferta todavía.</p>
        ) : (
          <table className="tabla-ofertas">
            <thead>
              <tr>
                <th>Publicación</th>
                <th>Monto</th>
                <th>Fecha</th>
                <th>Estado</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {ofertas.map((oferta) => (
                <tr key={oferta.id}>
                  <td>{oferta.tituloPublicacion}</td>
                  <td>${oferta.monto}</td>
                  <td>{new Date(oferta.fecha).toLocaleDateString()}</td>
                  <td className={`estado ${oferta.estado.toLowerCase()}`}>
                    {oferta.estado}
                  </td>
                  <td className="acciones">
                    <button
                      className="btn-ver-publicacion"
                      onClick={() =>
                        navigate(`/publicaciones/${oferta.publicacionId}`)
                      }
                    >
                      Ver publicación
                    </button>
                    <button
                      className="btn-eliminar-oferta"
                      onClick={() => abrirModalEliminar(oferta)}
                    >
                      Eliminar oferta
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {modalVisible && (
        <div className="modal-overlay">
          <div className="modal-contenido">
            <h3>Confirmar eliminación</h3>
            <p>
              ¿Estás seguro de que deseas eliminar la oferta por{" "}
              <strong>${ofertaSeleccionada?.monto}</strong> en{" "}
              <strong>{ofertaSeleccionada?.tituloPublicacion}</strong>?
            </p>
            <div className="modal-botones">
              <button className="btn-cancelar" onClick={cerrarModal}>
                Cancelar
              </button>
              <button
                className="btn-confirmar-eliminar"
                onClick={confirmarEliminar}
              >
                Sí, eliminar
              </button>
            </div>
          </div>
        </div>
      )}

      {modalInfoVisible && (
        <div className="modal-overlay">
          <div className="modal-contenido">
            <h3>Acción no permitida</h3>
            <p>
              No puedes eliminar una oferta que ya fue <strong>aceptada</strong>.
            </p>
            <div className="modal-botones">
              <button className="btn-confirmar-eliminar" onClick={cerrarModalInfo}>
                Entendido
              </button>
            </div>
          </div>
        </div>
      )}

      <Footer />
    </div>
  );
}
