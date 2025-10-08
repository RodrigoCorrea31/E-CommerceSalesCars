import { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { loginUsuario } from "../../api/usuarioApi";
import "../../styles/Login.css";

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const queryParams = new URLSearchParams(location.search);
  const redirectPath = queryParams.get("redirect") || "/";

  const [email, setEmail] = useState("");
  const [contrasena, setContrasena] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      const data = await loginUsuario({ email, contrasena });
      console.log("Data recibida del backend:", data); 
      login(data);
      navigate(redirectPath); 
    } catch (error) {
      setError("Error en el inicio de sesión. Verifique los datos ingresados.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form onSubmit={handleSubmit} className="login-form">
        <h2 className="login-title">Iniciar Sesión</h2>
        <p className="login-subtitle">Bienvenido de nuevo</p>

        {error && <p className="login-error">{error}</p>}

        <div className="login-field">
          <label>Correo electrónico</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="login-input"
            required
          />
        </div>

        <div className="login-field">
          <label>Contraseña</label>
          <input
            type="password"
            value={contrasena}
            onChange={(e) => setContrasena(e.target.value)}
            className="login-input"
            required
          />
        </div>

        <div className="login-button-container">
          <button type="submit" disabled={loading} className="login-button">
            {loading ? "Ingresando..." : "Ingresar"}
          </button>
        </div>

        <p className="login-footer">
          ¿No tienes cuenta?{" "}
          <span onClick={() => navigate("/registro")} className="login-link">
            Regístrate aquí
          </span>
        </p>
      </form>
    </div>
  );
}
