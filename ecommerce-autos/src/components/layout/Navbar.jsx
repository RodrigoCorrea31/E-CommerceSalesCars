import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { useState } from "react";
import "../../styles/Navbar.css";
import LogoESC from "../../assets/LogoESC.jpg";

export default function Navbar() {
  const { usuario, logout } = useAuth();
  const navigate = useNavigate();
  const [menuAbierto, setMenuAbierto] = useState(false);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  const toggleMenu = () => {
    setMenuAbierto(!menuAbierto);
  };

  const handleLinkClick = () => {
    setMenuAbierto(false);
  };

  return (
    <nav className="navbar">
      <div className="navbar-logo">
        <Link to="/" onClick={handleLinkClick}>
          <img src={LogoESC} alt="E-Commerce Autos" className="logo-img" />
        </Link>
      </div>

      <button
        className={`hamburger ${menuAbierto ? "active" : ""}`}
        onClick={toggleMenu}
      >
        <span></span>
        <span></span>
        <span></span>
      </button>

      <ul className={`navbar-links ${menuAbierto ? "open" : ""}`}>
        <li>
          <Link to="/" onClick={handleLinkClick}>Inicio</Link>
        </li>
        <li>
          <Link to="/publicaciones/crear" onClick={handleLinkClick}>
            Publicar Vehículo
          </Link>
        </li>
        <li>
          <Link to="/ofertas" onClick={handleLinkClick}>Ofertas</Link>
        </li>
        <li>
          <Link to="/mis-transacciones" onClick={handleLinkClick}>
            Mis Compras/Ventas
          </Link>
        </li>
        <li>
          <Link to="/mis-publicaciones" onClick={handleLinkClick}>
            Mis Publicaciones
          </Link>
        </li>
      </ul>

      <div className="navbar-user">
        {usuario ? (
          <>
            <span>Hola, {usuario.nombre}</span>
            <button onClick={handleLogout} className="navbar-btn">
              Salir
            </button>
          </>
        ) : (
          <button
            onClick={() => navigate("/login")}
            className="navbar-btn"
          >
            Ingresar
          </button>
        )}
      </div>
    </nav>
  );
}
