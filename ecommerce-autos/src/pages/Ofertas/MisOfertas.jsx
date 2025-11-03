import React, { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { obtenerOfertasUsuario } from "../../api/usuarioApi";
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
                <th>Acción</th>
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
                  <td>
                    <button
                      className="btn-ver-publicacion"
                      onClick={() => navigate(`/publicaciones/${oferta.publicacionId}`)}
                    >
                      Ver publicación
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
