import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getOfertasPorPublicacion, getPublicacionById } from "../../api/publicacionApi";
import { rechazarOferta } from "../../api/ofertaApi";
import Navbar from "../../components/layout/Navbar";
import Footer from "../../components/layout/Footer";
import "../../styles/OfertasPorPublicacion.css";

export default function OfertasPorPublicacion() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [ofertas, setOfertas] = useState([]);
  const [tituloPublicacion, setTituloPublicacion] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const obtenerNombreEstado = (estado) => {
    switch (estado) {
      case 0:
        return "Pendiente";
      case 1:
        return "Aceptada";
      case 2:
        return "Rechazada";
      case 3:
        return "Cancelada";
      default:
        return "Desconocido";
    }
  };

  const fetchDatos = async () => {
    try {
      setLoading(true);
      const token = localStorage.getItem("token");

      const resOfertas = await getOfertasPorPublicacion(id, token);
      setOfertas(resOfertas.data);

      const resPublicacion = await getPublicacionById(id);
      setTituloPublicacion(resPublicacion.data.titulo);
    } catch (err) {
      console.error(err);
      setError("Error al cargar los datos.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (id) fetchDatos();
  }, [id]);

  const manejarRedireccionAceptar = (ofertaId) => {
    navigate(`/finalizar-transaccion/${ofertaId}`);
  };

  const manejarRechazar = async (ofertaId) => {
    try {
      await rechazarOferta(ofertaId);
      alert("❌ Oferta rechazada correctamente.");
      fetchDatos();
    } catch (err) {
      console.error(err);
      alert("⚠️ Error al rechazar la oferta.");
    }
  };

  if (loading) {
    return (
      <div className="OfertasContainer">
        <Navbar />
        <div className="ofertas-body">
          <h2>Cargando ofertas...</h2>
        </div>
        <Footer />
      </div>
    );
  }

  if (error) {
    return (
      <div className="OfertasContainer">
        <Navbar />
        <div className="ofertas-body">
          <h2>{error}</h2>
        </div>
        <Footer />
      </div>
    );
  }

  return (
    <div className="OfertasContainer">
      <Navbar />
      <div className="ofertas-body">
        <div className="ofertas-header">
          <h2>Ofertas de la publicación "{tituloPublicacion}"</h2>
          <button className="btn-volver" onClick={() => navigate(-1)}>
            ← Volver
          </button>
        </div>

        {ofertas.length === 0 ? (
          <p>No hay ofertas todavía.</p>
        ) : (
          <table className="tabla-ofertas">
            <thead>
              <tr>
                <th>Monto</th>
                <th>Fecha</th>
                <th>Estado</th>
                <th>Comprador</th>
                <th>Email</th>
                <th>Teléfono</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {ofertas.map((oferta, index) => (
                <tr key={index}>
                  <td>${oferta.monto.toLocaleString()}</td>
                  <td>{new Date(oferta.fecha).toLocaleDateString()}</td>
                  <td>
                    <span
                      className={`estado ${
                        oferta.estado === 0
                          ? "pendiente"
                          : oferta.estado === 1
                          ? "aceptada"
                          : oferta.estado === 2
                          ? "rechazada"
                          : ""
                      }`}
                    >
                      {obtenerNombreEstado(oferta.estado)}
                    </span>
                  </td>
                  <td>{oferta.comprador?.nombre}</td>
                  <td>{oferta.comprador?.email}</td>
                  <td>{oferta.comprador?.telefono}</td>
                  <td>
                    {oferta.estado === 0 ? (
                      <div className="acciones-oferta">
                        <button
                          className="btn-aceptar"
                          onClick={() => manejarRedireccionAceptar(oferta.id)}
                        >
                          Aceptar
                        </button>
                        <button
                          className="btn-rechazar"
                          onClick={() => manejarRechazar(oferta.id)}
                        >
                          Rechazar
                        </button>
                      </div>
                    ) : (
                      <span>—</span>
                    )}
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
