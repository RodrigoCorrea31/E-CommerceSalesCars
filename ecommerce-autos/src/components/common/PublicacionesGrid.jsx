import CardAuto from "./CardAuto";
import "../../styles/PublicacionesGrid.css";

export default function PublicacionesGrid({ publicaciones }) {
  return (
    <div className="publicaciones-grid">
      {publicaciones.length > 0 ? (
        publicaciones.map((publi) => (
          <CardAuto key={publi.id} publicacion={publi} />
        ))
      ) : (
        <p>No hay publicaciones disponibles</p>
      )}
    </div>
  );
}
