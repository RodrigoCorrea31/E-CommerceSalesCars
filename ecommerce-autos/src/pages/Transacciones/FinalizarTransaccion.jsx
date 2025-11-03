import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Navbar from "../../components/layout/Navbar";
import Footer from "../../components/layout/Footer";
import { aceptarOferta, getOfertaById } from "../../api/ofertaApi";
import { finalizarTransaccion } from "../../api/transaccionApi";
import "../../styles/FinalizarTransaccion.css";

export default function FinalizarTransaccion() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [oferta, setOferta] = useState(null);
  const [loading, setLoading] = useState(true);
  const [mensaje, setMensaje] = useState("");
  const [error, setError] = useState(null);
  const [transaccion, setTransaccion] = useState(null);
  const [procesando, setProcesando] = useState(false);

  useEffect(() => {
    const fetchOferta = async () => {
      try {
        const token = localStorage.getItem("token");
        const response = await getOfertaById(id, token);
        setOferta(response.data);
      } catch (err) {
        console.error(err);
        setError("⚠️ Error al cargar la oferta seleccionada.");
      } finally {
        setLoading(false);
      }
    };

    fetchOferta();
  }, [id]);

  const manejarFinalizarTransaccion = async () => {
    setProcesando(true);
    setMensaje("");
    setError("");

    try {
      await aceptarOferta(id);

      const res = await finalizarTransaccion(id);
      setTransaccion(res.data);

      setMensaje("✅ Transacción finalizada correctamente.");
    } catch (err) {
      console.error(err);
      setError("⚠️ Error al finalizar la transacción. Verificá el estado de la oferta o volvé a intentarlo.");
    } finally {
      setProcesando(false);
    }
  };

  if (loading) {
    return (
      <div className="FinalizarContainer">
        <Navbar />
        <div className="finalizar-body">
          <h2>Cargando datos...</h2>
        </div>
        <Footer />
      </div>
    );
  }

  if (error && !oferta) {
    return (
      <div className="FinalizarContainer">
        <Navbar />
        <div className="finalizar-body">
          <h2>{error}</h2>
          <button onClick={() => navigate(-1)}>← Volver</button>
        </div>
        <Footer />
      </div>
    );
  }

  return (
    <div className="ContainerPrincipal">
        <Navbar />
    <div className="FinalizarContainer">
      <div className="finalizar-body">
        <h2>Finalizar Transacción</h2>

        {oferta && (
          <div className="oferta-detalle">
            {oferta.publicacion?.titulo && (
              <p><strong>Publicación:</strong> {oferta.publicacion.titulo}</p>
            )}
            <p><strong>Monto ofertado:</strong> ${oferta.monto.toLocaleString()}</p>
            <p><strong>Fecha de la oferta:</strong> {new Date(oferta.fecha).toLocaleDateString()}</p>
          </div>
        )}

        <div className="acciones-finalizacion">
          <button
            onClick={manejarFinalizarTransaccion}
            className="btn-finalizar"
            disabled={procesando}
          >
            {procesando ? "Procesando..." : "Finalizar Transacción"}
          </button>
          <button onClick={() => navigate(-1)} className="btn-volver">
            ← Volver
          </button>
        </div>

        {mensaje && <p className="mensaje-exito">{mensaje}</p>}
        {error && <p className="mensaje-error">{error}</p>}

        {transaccion && (
          <div className="transaccion-detalle">
            <h3>Detalles de la transacción</h3>
            <p><strong>Precio de venta:</strong> ${transaccion.precioVenta.toLocaleString()}</p>
            {transaccion.fechaFinalizacion && (
              <p><strong>Finalizada el:</strong> {new Date(transaccion.fecha).toLocaleDateString()}</p>
            )}
            <p><strong>Estado:</strong> {transaccion.estado}</p>
            <p><strong>Método de pago:</strong> {transaccion.metodoDePago ?? "No especificado"}</p>
          </div>
        )}
      </div>
    </div>
    <Footer />
    </div>
  );
}
