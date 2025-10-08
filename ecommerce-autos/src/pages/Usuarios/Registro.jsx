import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { registrarUsuario } from "../../api/usuarioApi";
import "../../styles/Registro.css";

export default function Registro() {
  const navigate = useNavigate();

  const [tipoUsuario, setTipoUsuario] = useState("persona");
  const [nombre, setNombre] = useState("");
  const [email, setEmail] = useState("");
  const [telefono, setTelefono] = useState("");
  const [contrasena, setContrasena] = useState("");
  const [campoExtra1, setCampoExtra1] = useState("");
  const [campoExtra2, setCampoExtra2] = useState("");

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [exito, setExito] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");
    setExito("");

    if (!nombre || !email || !contrasena || !telefono) {
      setError("Por favor completa todos los campos obligatorios.");
      setLoading(false);
      return;
    }

    if (!campoExtra1 || !campoExtra2) {
      setError(
        tipoUsuario === "persona"
          ? "Cédula y dirección son requeridas."
          : "RUT y razón social son requeridos."
      );
      setLoading(false);
      return;
    }

    try {
      await registrarUsuario({
        tipoUsuario,
        nombre,
        email,
        telefono,
        contrasena,
        datoExtra1: campoExtra1,
        datoExtra2: campoExtra2,
      });

      setExito("Usuario registrado con éxito. Redirigiendo...");
      setTimeout(() => navigate("/login"), 2000);
    } catch (err) {
      setError("No se pudo registrar el usuario. Verifica los datos.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="registro-container">
      <form onSubmit={handleSubmit} className="registro-form">
        <h2 className="registro-title">Registro de Usuario</h2>
        <p className="registro-subtitle">
          Completa el formulario para crear tu cuenta
        </p>

        {error && <p className="registro-error">{error}</p>}
        {exito && <p className="registro-success">{exito}</p>}

        <div className="registro-grid">
          <div className="registro-field">
            <label htmlFor="tipoUsuario">Tipo de usuario</label>
            <select
              id="tipoUsuario"
              value={tipoUsuario}
              onChange={(e) => setTipoUsuario(e.target.value)}
              className="registro-input"
            >
              <option value="persona">Persona</option>
              <option value="empresa">Empresa</option>
            </select>
          </div>

          <div className="registro-field">
            <label htmlFor="nombre">Nombre completo</label>
            <input
              id="nombre"
              type="text"
              placeholder="Nombre Completo"
              value={nombre}
              onChange={(e) => setNombre(e.target.value)}
              className="registro-input"
              required
            />
          </div>

          <div className="registro-field">
            <label htmlFor="email">Correo electrónico</label>
            <input
              id="email"
              type="email"
              placeholder="Correo electrónico"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="registro-input"
              required
            />
          </div>

          <div className="registro-field">
            <label htmlFor="telefono">Teléfono</label>
            <input
              id="telefono"
              type="text"
              placeholder="Teléfono de contacto"
              value={telefono}
              onChange={(e) => setTelefono(e.target.value)}
              className="registro-input"
              required
            />
          </div>

          <div className="registro-field">
            <label htmlFor="contrasena">Contraseña</label>
            <input
              id="contrasena"
              type="password"
              placeholder="Contraseña"
              value={contrasena}
              onChange={(e) => setContrasena(e.target.value)}
              className="registro-input"
              required
            />
          </div>

          {tipoUsuario === "persona" ? (
            <>
              <div className="registro-field">
                <label htmlFor="cedula">Cédula</label>
                <input
                  id="cedula"
                  type="text"
                  placeholder="Cédula de identidad"
                  value={campoExtra1}
                  onChange={(e) => setCampoExtra1(e.target.value)}
                  className="registro-input"
                  required
                />
              </div>
              <div className="registro-field">
                <label htmlFor="direccion">Dirección</label>
                <input
                  id="direccion"
                  type="text"
                  placeholder="Dirección completa"
                  value={campoExtra2}
                  onChange={(e) => setCampoExtra2(e.target.value)}
                  className="registro-input"
                  required
                />
              </div>
            </>
          ) : (
            <>
              <div className="registro-field">
                <label htmlFor="rut">RUT</label>
                <input
                  id="rut"
                  type="text"
                  placeholder="RUT de la empresa"
                  value={campoExtra1}
                  onChange={(e) => setCampoExtra1(e.target.value)}
                  className="registro-input"
                  required
                />
              </div>
              <div className="registro-field">
                <label htmlFor="razonSocial">Razón Social</label>
                <input
                  id="razonSocial"
                  type="text"
                  placeholder="Razón social"
                  value={campoExtra2}
                  onChange={(e) => setCampoExtra2(e.target.value)}
                  className="registro-input"
                  required
                />
              </div>
            </>
          )}
        </div>

        <div className="registro-button-container">
          <button type="submit" disabled={loading} className="registro-button">
            {loading ? "Registrando..." : "Registrarse"}
          </button>
        </div>

        <p className="registro-footer">
          ¿Ya tienes una cuenta?{" "}
          <span onClick={() => navigate("/login")} className="registro-link">
            Inicia sesión aquí
          </span>
        </p>
      </form>
    </div>
  );
}
