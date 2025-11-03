import { useNavigate } from "react-router-dom";
import "../../styles/CardAuto.css";

const mapCombustible = {
  0: "Nafta",
  1: "Diesel",
  2: "Eléctrico",
};

export default function CardAuto({ publicacion }) {
  const navigate = useNavigate();
  const imagenPrincipal =
    publicacion?.imagenes?.length > 0
      ? publicacion.imagenes[0]
      : "/placeholder-car.png";

  const verDetalles = () => navigate(`/publicaciones/${publicacion.id}`);

  return (
    <div className="card-auto">
      <div className="card-auto-img-wrapper">
        <img
          src={imagenPrincipal}
          alt={`${publicacion?.marca || ""} ${publicacion?.modelo || ""}`}
          className="card-auto-img"
        />
      </div>

      <div className="card-auto-body">
        <h3 className="card-auto-titulo">{publicacion.titulo}</h3>
        <p className="card-auto-sub">
          {publicacion.marca} {publicacion.modelo} · Año: {publicacion.anio}
        </p>
        <p className="card-auto-detalle">
          KMs: {publicacion.kilometraje?.toLocaleString("es-UY")} ·{" "}
          Combustible: {mapCombustible[publicacion.combustible]}
        </p>
        <p className="card-auto-precio">
          USD {publicacion.precio?.toLocaleString("es-UY")}
        </p>
      </div>

      <div className="card-auto-actions">
        <button className="btn-detalles" onClick={verDetalles}>
          Ver Detalles
        </button>
      </div>
    </div>
  );
}
