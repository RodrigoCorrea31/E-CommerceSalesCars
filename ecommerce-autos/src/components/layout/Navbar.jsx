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

  return (
    <nav className="navbar">
      <div className="navbar-logo">
        <Link to="/">
          <img src={LogoESC} alt="E-Commerce Autos" className="logo-img" />
        </Link>
      </div>

      <button className={`hamburger ${menuAbierto ? "active" : ""}`} onClick={toggleMenu}>
        <span></span>
        <span></span>
        <span></span>
      </button>

      <ul className={`navbar-links ${menuAbierto ? "open" : ""}`}>
        <li><Link to="/" onClick={() => setMenuAbierto(false)}>Inicio</Link></li>
        <li><Link to="/publicaciones/crear" onClick={() => setMenuAbierto(false)}>Publicar Vehículo</Link></li>
        <li><Link to="/ofertas" onClick={() => setMenuAbierto(false)}>Ofertas</Link></li>
        <li><Link to="/transacciones" onClick={() => setMenuAbierto(false)}>Mis Compras/Ventas</Link></li>
      </ul>

      <div className="navbar-user">
        {usuario ? (
          <>
            <span>Hola, {usuario.nombre}</span>
            <button onClick={handleLogout} className="navbar-btn">Salir</button>
          </>
        ) : (
          <button onClick={() => navigate("/login")} className="navbar-btn">
            Ingresar
          </button>
        )}
      </div>
    </nav>
  );
}
