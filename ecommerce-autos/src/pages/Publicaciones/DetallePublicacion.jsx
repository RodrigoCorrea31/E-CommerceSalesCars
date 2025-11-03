import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { obtenerDetallePublicacion } from "../../services/detallePublicacionService";
import { crearOferta } from "../../api/ofertaApi";
import { useAuth } from "../../context/AuthContext";
import "../../styles/DetallePublicacion.css";
import Navbar from "../../components/layout/Navbar";

const mapCombustible = {
  0: "Nafta",
  1: "Diesel",
  2: "Eléctrico",
};

export default function DetallePublicacion() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { usuario } = useAuth();
  const [publicacion, setPublicacion] = useState(null);
  const [loading, setLoading] = useState(true);
  const [imgIndex, setImgIndex] = useState(0);
  const [mostrarModal, setMostrarModal] = useState(false);
  const [montoOferta, setMontoOferta] = useState("");
  const [mensaje, setMensaje] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await obtenerDetallePublicacion(id);
        setPublicacion(data);
      } catch (err) {
        console.error("Error cargando publicación:", err);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando...</p>;
  if (!publicacion) return <p className="detalle-error">Publicación no encontrada.</p>;

  const imagenes = publicacion.vehiculo.imagenes.length
    ? publicacion.vehiculo.imagenes
    : ["/placeholder-car.png"];

  const siguiente = () => setImgIndex((prev) => (prev + 1) % imagenes.length);
  const anterior = () => setImgIndex((prev) => (prev - 1 + imagenes.length) % imagenes.length);

  const abrirModalOferta = () => {
    if (!usuario) {
      alert("Debes iniciar sesión para realizar una oferta.");
      navigate("/login");
      return;
    }
    setMostrarModal(true);
  };

  const enviarOferta = async () => {
  try {
    const token = localStorage.getItem("token");
    const ofertaData = {
      monto: publicacion.precio,
      publicacionId: publicacion.id,
    };

    await crearOferta(ofertaData, token);
    setMensaje("✅ Oferta enviada correctamente.");
    setTimeout(() => {
      setMostrarModal(false);
      setMensaje("");
    }, 2000);
  } catch (err) {
    if (err.response && err.response.status === 400 && err.response.data?.mensaje) {
      setMensaje(`⚠️ ${err.response.data.mensaje}`);
      err.name = "HandledAxiosError";
    } 
    else if (err.response && err.response.status === 401) {
      setMensaje("❌ Debes iniciar sesión para realizar una oferta.");
      navigate("/login");
    } 
    else {
      setMensaje("❌ Ocurrió un error inesperado al enviar la oferta.");
    }

    if (import.meta.env.MODE === "development" && err.name !== "HandledAxiosError") {
      console.warn("Detalles del error:", err.response?.data || err.message);
    }
  }
};


  return (
    <div className="navbar-detalle-publicacion">
      <Navbar />
      <div className="detalle-publicacion-container">
        <div className="detalle-card">
          <div className="detalle-img-wrapper">
            <img src={imagenes[imgIndex]} alt="Vehículo" className="detalle-img" />
            {imagenes.length > 1 && (
              <>
                <button className="img-nav left" onClick={anterior}>‹</button>
                <button className="img-nav right" onClick={siguiente}>›</button>
              </>
            )}
          </div>

          <div className="detalle-info">
            <h2>{publicacion.titulo}</h2>
            <div className="detalle-info-grid">
              <div><strong>Marca:</strong> {publicacion.vehiculo.marca}</div>
              <div><strong>Modelo:</strong> {publicacion.vehiculo.modelo}</div>
              <div><strong>Año:</strong> {publicacion.vehiculo.anio}</div>
              <div><strong>Kilometraje:</strong> {publicacion.vehiculo.kilometraje.toLocaleString("es-UY")} km</div>
              <div><strong>Combustible:</strong> {mapCombustible[publicacion.vehiculo.combustible]}</div>
              <div><strong>Color:</strong> {publicacion.vehiculo.color}</div>
              <div><strong>Condición:</strong> {publicacion.esUsado ? "Usado" : "Nuevo"}</div>
              <div className="detalle-precio">USD {publicacion.precio.toLocaleString("es-UY")}</div>
            </div>
          </div>

          {publicacion.vendedor && (
            <div className="detalle-vendedor">
              <h3>Vendedor</h3>
              <div className="detalle-info-grid">
                <div><strong>Nombre:</strong> {publicacion.vendedor.nombre}</div>
                <div><strong>Email:</strong> {publicacion.vendedor.email}</div>
                <div><strong>Teléfono:</strong> {publicacion.vendedor.telefono}</div>
                <div><strong>Reputación:</strong> ⭐ {publicacion.vendedor.reputacion}/5</div>
                <div><strong>Ventas realizadas:</strong> {publicacion.vendedor.cantidadVentas}</div>
              </div>
            </div>
          )}

          <div className="detalle-actions">
            <button className="btn-volver" onClick={() => navigate(-1)}>← Volver</button>
            <button className="btn-ofertar" onClick={abrirModalOferta}>Ofertar</button>
          </div>
        </div>
      </div>

      {mostrarModal && (
  <div className="modal-overlay">
    <div className="modal-content">
      <h3>Confirmar Oferta</h3>

      <div className="modal-info">
        <p><strong>Vehículo:</strong> {publicacion.vehiculo.marca} {publicacion.vehiculo.modelo}</p>
        <p><strong>Año:</strong> {publicacion.vehiculo.anio}</p>
        <p><strong>Precio:</strong> USD {publicacion.precio.toLocaleString("es-UY")}</p>
      </div>

      {mensaje && <p className="modal-mensaje">{mensaje}</p>}

      <div className="modal-buttons">
        <button className="btn-cancelar" onClick={() => setMostrarModal(false)}>Cancelar</button>
        <button className="btn-confirmar" onClick={enviarOferta}>Confirmar</button>
      </div>
    </div>
  </div>
)}

    </div>
  );
}
