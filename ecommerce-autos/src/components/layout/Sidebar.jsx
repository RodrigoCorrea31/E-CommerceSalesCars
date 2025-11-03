import { useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import "../../styles/Sidebar.css";

export default function Sidebar({ abierto, onAplicarFiltros }) {
  const { usuario } = useAuth();
  const [marca, setMarca] = useState("");
  const [precioMin, setPrecioMin] = useState("");
  const [precioMax, setPrecioMax] = useState("");
  const [anioDesde, setAnioDesde] = useState("");
  const [anioHasta, setAnioHasta] = useState("");

  const marcas = [
    "Alfa Romeo", "Aston Martin", "Audi", "BMW", "Cadillac", "Chery", "Chevrolet", "Chevy",
    "Citroën", "Dodge", "Fiat", "Ford", "Foton", "GMC", "Great Wall", "Honda", "Hyundai",
    "Jeep", "Kia", "Land Rover", "Lexus", "Lifan", "Mazda", "Mercedes-Benz", "Mini",
    "Mitsubishi", "Nissan", "Opel", "Peugeot", "Porsche", "Ram", "Renault", "Rolls-Royce",
    "Seat", "Subaru", "Suzuki", "Toyota", "Volkswagen", "Volvo"
  ].sort();

  const aplicarFiltros = () => {
    const filtros = {
      marca: marca || null,
      precioDesde: precioMin || null,
      precioHasta: precioMax || null,
      anioDesde: anioDesde || null,
      anioHasta: anioHasta || null,
    };

    if (onAplicarFiltros) {
      onAplicarFiltros(filtros);
    } else {
      console.warn("Sidebar: onAplicarFiltros no fue pasado como prop");
    }
  };

  return (
    <aside className={`sidebar ${abierto ? "open" : ""}`}>
      <h3>Filtros</h3>

      <div className="filter-group">
        <label>Marca</label>
        <select value={marca} onChange={(e) => setMarca(e.target.value)}>
          <option value="">Todas</option>
          {marcas.map((m) => (
            <option key={m} value={m}>
              {m}
            </option>
          ))}
        </select>
      </div>

      <div className="filter-group">
        <label>Precio</label>
        <input
          type="number"
          placeholder="Mínimo"
          value={precioMin}
          onChange={(e) => setPrecioMin(e.target.value)}
        />
        <input
          type="number"
          placeholder="Máximo"
          value={precioMax}
          onChange={(e) => setPrecioMax(e.target.value)}
        />
      </div>

      <div className="filter-group">
        <label>Año</label>
        <input
          type="number"
          placeholder="Desde"
          value={anioDesde}
          onChange={(e) => setAnioDesde(e.target.value)}
        />
        <input
          type="number"
          placeholder="Hasta"
          value={anioHasta}
          onChange={(e) => setAnioHasta(e.target.value)}
        />
      </div>

      <button className="filter-btn" onClick={aplicarFiltros}>
        Aplicar Filtros
      </button>

      <hr />
      
          <h3>Mi Panel</h3>
          <ul className="sidebar-panel">
            <li><Link to="/mis-publicaciones">Mis Publicaciones</Link></li>
            <li><Link to="/ofertas">Mis Ofertas</Link></li>
            <li><Link to="/mis-transacciones">Mis Compras/Ventas</Link></li>
          </ul>
    </aside>
  );
}
