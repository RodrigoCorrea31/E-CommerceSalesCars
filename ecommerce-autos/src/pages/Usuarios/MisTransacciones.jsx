import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { obtenerComprasUsuario, obtenerVentasUsuario } from "../../api/usuarioApi";
import "../../styles/MisTransacciones.css";
import "../../styles/FormPublicacionNoLogueado.css"; 
import Navbar from "../../components/layout/Navbar";


export default function MisTransacciones() {
  const navigate = useNavigate();
  const { usuario } = useAuth();
  const [compras, setCompras] = useState([]);
  const [ventas, setVentas] = useState([]);
  const [tabActiva, setTabActiva] = useState("compras");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!usuario) return;

    const fetchTransacciones = async () => {
      setLoading(true);
      try {
        const [comprasRes, ventasRes] = await Promise.all([
          obtenerComprasUsuario(usuario.id),
          obtenerVentasUsuario(usuario.id),
        ]);

        setCompras(comprasRes.data);
        setVentas(ventasRes.data);
      } catch (err) {
        console.error("Error al obtener transacciones:", err);
        setError("Error al cargar las transacciones.");
      } finally {
        setLoading(false);
      }
    };

    fetchTransacciones();
  }, [usuario]);

  if (!usuario) {
    return (
    <div className="NoLogueadoContainerPrincipal">
        <Navbar />
      <div className="login-requerido-container">
        <h2>No has iniciado sesión</h2>
        <p>Debes iniciar sesión para ver tus transacciones.</p>
        <button
          className="btn-ir-login"
          onClick={() => navigate("/login?redirect=/mis-transacciones")}
        >
          Ir al login
        </button>
      </div>
    </div>
    );
  }

  if (loading) return <p>Cargando transacciones...</p>;
  if (error) return <p>{error}</p>;

  const renderTabla = (data, tipo) => (
    <table className="tabla-transacciones">
      <thead>
        <tr>
          <th>Publicación</th>
          <th>Fecha</th>
          <th>Precio Venta</th>
          <th>Estado</th>
          <th>Método de Pago</th>
          <th>{tipo === "compras" ? "Vendedor" : "Comprador"}</th>
        </tr>
      </thead>
      <tbody>
        {data.length === 0 ? (
          <tr>
            <td colSpan="6" className="sin-datos">
              No hay transacciones registradas.
            </td>
          </tr>
        ) : (
          data.map((t) => (
            <tr key={t.id}>
              <td>{t.publicacionTitulo || "—"}</td>
              <td>{new Date(t.fecha).toLocaleDateString()}</td>
              <td>${t.precioVenta.toLocaleString()}</td>
              <td>
                <span className={`estado ${t.estado.toLowerCase()}`}>{t.estado}</span>
              </td>
              <td>{t.metodoDePago || "—"}</td>
              <td>
                {tipo === "compras"
                  ? t.vendedorNombre || "—"
                  : t.compradorNombre || "—"}
              </td>
            </tr>
          ))
        )}
      </tbody>
    </table>
  );

  return (
    <div className="ContainerPrincipal">
        <Navbar />
    <div className="mis-transacciones-container">
      <h2>Mis Transacciones</h2>

      <div className="tabs">
        <button
          className={tabActiva === "compras" ? "active" : ""}
          onClick={() => setTabActiva("compras")}
        >
          Mis Compras
        </button>
        <button
          className={tabActiva === "ventas" ? "active" : ""}
          onClick={() => setTabActiva("ventas")}
        >
          Mis Ventas
        </button>
      </div>

      <div className="tabla-container">
        {tabActiva === "compras" && renderTabla(compras, "compras")}
        {tabActiva === "ventas" && renderTabla(ventas, "ventas")}
      </div>
    </div>
    </div>
  );
}
