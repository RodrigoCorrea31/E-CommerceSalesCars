import "../../styles/CardAuto.css";

const mapCombustible = {
  0: "Nafta",
  1: "Diesel",
  2: "Eléctrico",
};

export default function CardAuto({ publicacion }) {
  const imagenPrincipal =
    publicacion?.imagenes?.length > 0
      ? publicacion.imagenes[0]
      : "/placeholder-car.png";

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
        <h3
          className="card-auto-titulo"
          title={publicacion.titulo}
        >
          {publicacion.titulo}
        </h3>

        <p className="card-auto-sub">
          {publicacion?.marca} {publicacion?.modelo} · Año: {publicacion?.anio}
        </p>

        <p className="card-auto-detalle">
          <span>
            KMs: {publicacion?.kilometraje?.toLocaleString("es-UY") ?? "N/D"}
          </span>
          <span>
            {" "}· Combustible: {mapCombustible[publicacion?.combustible] ?? "N/D"}
          </span>
        </p>

        <p className="card-auto-detalle">
          Color: {publicacion?.color || "No especificado"}
        </p>

        <p className="card-auto-precio">
          USD {publicacion.precio?.toLocaleString("es-UY") ?? "N/D"}
        </p>
      </div>

      <div className="card-auto-actions">
        <button className="btn-detalles">Ver Detalles</button>
        <button className="btn-ofertar">Ofertar</button>
      </div>
    </div>
  );
}
