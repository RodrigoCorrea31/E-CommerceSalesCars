import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { crearPublicacion, uploadImagen } from "../../api/publicacionApi";
import { useAuth } from "../../context/AuthContext";
import "../../styles/FormPublicacion.css";
import "../../styles/FormPublicacionNoLogueado.css";
import Navbar from "../../components/layout/Navbar";

export default function CrearPublicacion() {
  const navigate = useNavigate();
  const { usuario } = useAuth();

  const [formData, setFormData] = useState({
    titulo: "",
    precio: "",
    esUsado: false,
    vehiculo: {
      marca: "",
      modelo: "",
      anio: "",
      kilometraje: "",
      combustible: 0,
      color: "",
      imagenes: [],
    },
  });

  const [archivos, setArchivos] = useState([]); 
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    if (name.startsWith("vehiculo.")) {
      const field = name.split(".")[1];
      setFormData({
        ...formData,
        vehiculo: {
          ...formData.vehiculo,
          [field]: type === "checkbox" ? checked : value,
        },
      });
    } else {
      setFormData({
        ...formData,
        [name]: type === "checkbox" ? checked : value,
      });
    }
  };

  const handleFileChange = (e) => {
  const nuevasImagenes = Array.from(e.target.files);
  setArchivos((prevImagenes) => [...prevImagenes, ...nuevasImagenes]);
};

  const handleSubmit = async (e) => {
  e.preventDefault();
  setError("");
  setLoading(true);

  const token = localStorage.getItem("token");
  if (!token) {
    setError("Debes iniciar sesión para crear una publicación.");
    setLoading(false);
    return;
  }

  try {
    const urls = await Promise.all(
      archivos.map(async (file) => {
        const { data: url } = await uploadImagen(file, token);
        return url;
      })
    );

    await crearPublicacion(
      {
        titulo: formData.titulo,
        precio: parseFloat(formData.precio),
        esUsado: formData.esUsado,
        vehiculo: {
          ...formData.vehiculo,
          imagenes: urls,
        },
      },
      token
    );

    navigate("/");
  } catch (err) {
    console.error(err);
    setError("No se pudo crear la publicación. Revisa los datos.");
  } finally {
    setLoading(false);
  }
};


  return (
    <div className="CrearPublicacionContainer">
      <Navbar />
      {!usuario ? (
      <div className="login-requerido-container">
        <h2>No has iniciado sesión</h2>
        <p>Debes iniciar sesión para crear una nueva publicación.</p>
        <button className="btn-ir-login" onClick={() => navigate("/login?redirect=/publicaciones/crear")}>
            Ir al login
        </button>
      </div>
    ) : (
    <div className="crear-publicacion-container">
      <form onSubmit={handleSubmit} className="form-publicacion">
        <h2>Crear Nueva Publicación</h2>

        {error && <p className="form-error">{error}</p>}

        <div className="form-field">
          <label htmlFor="titulo">Título</label>
          <input
            id="titulo"
            type="text"
            name="titulo"
            value={formData.titulo}
            onChange={handleChange}
            required
            className="form-input"
          />
        </div>

        <div className="form-field">
          <label htmlFor="precio">Precio (USD)</label>
          <input
            id="precio"
            type="number"
            name="precio"
            step="0.01"
            value={formData.precio}
            onChange={handleChange}
            required
            className="form-input"
          />
        </div>

        <div className="form-field form-checkbox">
          <label htmlFor="esUsado">
            <input
              id="esUsado"
              type="checkbox"
              name="esUsado"
              checked={formData.esUsado}
              onChange={handleChange}
            />
            ¿Es usado?
          </label>
        </div>

        <h3>Datos del Vehículo</h3>
        <div className="form-field">
          <label htmlFor="marca">Marca</label>
          <input
            id="marca"
            type="text"
            name="vehiculo.marca"
            value={formData.vehiculo.marca}
            onChange={handleChange}
            required
            className="form-input"
          />
        </div>

        <div className="form-field">
          <label htmlFor="modelo">Modelo</label>
          <input
            id="modelo"
            type="text"
            name="vehiculo.modelo"
            value={formData.vehiculo.modelo}
            onChange={handleChange}
            required
            className="form-input"
          />
        </div>

        <div className="form-field">
          <label htmlFor="anio">Año</label>
          <input
            id="anio"
            type="number"
            name="vehiculo.anio"
            value={formData.vehiculo.anio}
            onChange={handleChange}
            required
            className="form-input"
          />
        </div>

        <div className="form-field">
          <label htmlFor="kilometraje">Kilometraje</label>
          <input
            id="kilometraje"
            type="number"
            name="vehiculo.kilometraje"
            value={formData.vehiculo.kilometraje}
            onChange={handleChange}
            required
            className="form-input"
          />
        </div>

        <div className="form-field">
          <label htmlFor="combustible">Combustible</label>
          <select
            id="combustible"
            name="vehiculo.combustible"
            value={formData.vehiculo.combustible}
            onChange={handleChange}
            className="form-input"
          >
            <option value={0}>Nafta</option>
            <option value={1}>Diesel</option>
            <option value={2}>Eléctrico</option>
          </select>
        </div>

        <div className="form-field">
          <label htmlFor="color">Color</label>
          <input
            id="color"
            type="text"
            name="vehiculo.color"
            value={formData.vehiculo.color}
            onChange={handleChange}
            required
            className="form-input"
          />
        </div>

        <div className="form-field">
          <label htmlFor="imagenes">Imágenes del vehículo</label>
          <input
            id="imagenes"
            type="file"
            multiple
            onChange={handleFileChange}
            className="form-input"
          />
        </div>

        <div className="form-button-container">
          <button type="submit" disabled={loading} className="form-button">
            {loading ? "Creando..." : "Publicar"}
          </button>
        </div>
      </form>
    </div>
)}
    </div>
  );
}
