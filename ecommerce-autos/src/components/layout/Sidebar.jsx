import { useState, useEffect } from "react";
import "../../styles/Sidebar.css";

export default function Sidebar() {
  const [abierto, setAbierto] = useState(false);
  const [touchStartX, setTouchStartX] = useState(0);
  const [touchEndX, setTouchEndX] = useState(0);

  useEffect(() => {
    const handleTouchStart = (e) => {
      setTouchStartX(e.touches[0].clientX);
    };

    const handleTouchMove = (e) => {
      setTouchEndX(e.touches[0].clientX);
    };

    const handleTouchEnd = () => {
      if (touchStartX - touchEndX > 100) {
        setAbierto(false);
      }
      if (touchEndX - touchStartX > 100) {
        setAbierto(true);
      }
    };

    window.addEventListener("touchstart", handleTouchStart);
    window.addEventListener("touchmove", handleTouchMove);
    window.addEventListener("touchend", handleTouchEnd);

    return () => {
      window.removeEventListener("touchstart", handleTouchStart);
      window.removeEventListener("touchmove", handleTouchMove);
      window.removeEventListener("touchend", handleTouchEnd);
    };
  }, [touchStartX, touchEndX]);

  return (
    <aside className={`sidebar ${abierto ? "open" : ""}`}>
      <h3>Filtros</h3>
      <div className="filter-group">
        <label>Marca</label>
        <select>
          <option>Todas</option>
          <option>Toyota</option>
          <option>Ford</option>
          <option>Chevrolet</option>
          <option>BMW</option>
        </select>
      </div>

      <div className="filter-group">
        <label>Precio</label>
        <input type="number" placeholder="Mínimo" />
        <input type="number" placeholder="Máximo" />
      </div>

      <div className="filter-group">
        <label>Año</label>
        <input type="number" placeholder="Desde" />
        <input type="number" placeholder="Hasta" />
      </div>

      <button className="filter-btn">Aplicar Filtros</button>

      <hr />

      <h3>Mi Panel</h3>
      <ul className="sidebar-panel">
        <li>Mis Ofertas</li>
        <li>Mis Compras</li>
        <li>Mis Ventas</li>
      </ul>
    </aside>
  );
}
