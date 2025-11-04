import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Navbar from "../../components/layout/Navbar";
import Footer from "../../components/layout/Footer";
import {
  getPublicacionById,
  editarPublicacion,
} from "../../api/publicacionApi";
import "../../styles/EditarPublicacion.css";

export default function EditarPublicacion() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [publicacion, setPublicacion] = useState(null);
  const [formData, setFormData] = useState({
    titulo: "",
    precio: "",
    esUsado: false,
    estado: "Activo",
    marca: "",
    modelo: "",
    anio: "",
    kilometraje: "",
    combustible: "Nafta",
    color: "",
    nuevasImagenesBase64: [],
  });
  const [loading, setLoading] = useState(true);
  const [mensaje, setMensaje] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await getPublicacionById(id, token);
        const data = res.data;

        setPublicacion(data);
        setFormData({
          titulo: data.titulo,
          precio: data.precio,
          esUsado: data.esUsado,
          estado: data.estado,
          marca: data.vehiculo.marca,
          modelo: data.vehiculo.modelo,
          anio: data.vehiculo.anio,
          kilometraje: data.vehiculo.kilometraje,
          combustible: data.vehiculo.combustible,
          color: data.vehiculo.color,
          nuevasImagenesBase64: [],
        });
      } catch (err) {
        console.error(err);
        setError("⚠️ Error al cargar la publicación.");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleFileChange = (e) => {
    const files = Array.from(e.target.files);
    const readerPromises = files.map(
      (file) =>
        new Promise((resolve, reject) => {
          const reader = new FileReader();
          reader.onload = () => resolve(reader.result.split(",")[1]);
          reader.onerror = reject;
          reader.readAsDataURL(file);
        })
    );

    Promise.all(readerPromises)
      .then((base64Images) => {
        setFormData((prev) => ({
          ...prev,
          nuevasImagenesBase64: [...prev.nuevasImagenesBase64, ...base64Images],
        }));
      })
      .catch((err) => console.error("Error al leer imágenes:", err));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMensaje("");
    setError("");

    try {
      const token = localStorage.getItem("token");

      const payload = { ...formData, id: parseInt(id) };

      await editarPublicacion(id, payload, token);

      setMensaje("✅ Publicación editada correctamente.");
      setTimeout(() => navigate("/mis-publicaciones"), 1500);
    } catch (err) {
      console.error(err);
      setError("⚠️ Error al guardar los cambios. Intente nuevamente.");
    }
  };

  if (loading) {
    return (
      <div className="EditarPublicacionContainer">
        <Navbar />
        <div className="editar-body">
          <h2>Cargando datos de la publicación...</h2>
        </div>
        <Footer />
      </div>
    );
  }

  if (error && !publicacion) {
    return (
      <div className="EditarPublicacionContainer">
        <Navbar />
        <div className="editar-body">
          <h2>{error}</h2>
          <button onClick={() => navigate(-1)}>← Volver</button>
        </div>
        <Footer />
      </div>
    );
  }

  return (
    <div className="ContainerPrincipal">
      <Navbar />
      <div className="EditarPublicacionContainer">
        <div className="editar-body">
          <h2>Editar Publicación</h2>

          <form onSubmit={handleSubmit} className="form-editar-publicacion">
            <div className="form-group">
              <label>Título</label>
              <input
                type="text"
                name="titulo"
                value={formData.titulo}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label>Precio</label>
              <input
                type="number"
                name="precio"
                value={formData.precio}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group checkbox">
              <label htmlFor="esUsado" className="toggle-label">
                ¿Es usado?
              </label>
              <label className="switch">
                <input
                  id="esUsado"
                  type="checkbox"
                  name="esUsado"
                  checked={formData.esUsado}
                  onChange={handleChange}
                />
                <span className="slider"></span>
              </label>
            </div>

            <div className="form-group">
              <label>Estado de la publicación</label>
              <select
                name="estado"
                value={formData.estado}
                onChange={handleChange}
              >
                <option value="Activo">Activo</option>
                <option value="Vendido">Vendido</option>
                <option value="Pausado">Pausado</option>
              </select>
            </div>

            <h3>Datos del vehículo</h3>

            <div className="form-group">
              <label>Marca</label>
              <input
                type="text"
                name="marca"
                value={formData.marca}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label>Modelo</label>
              <input
                type="text"
                name="modelo"
                value={formData.modelo}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label>Año</label>
              <input
                type="number"
                name="anio"
                value={formData.anio}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label>Kilometraje</label>
              <input
                type="number"
                name="kilometraje"
                value={formData.kilometraje}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label>Combustible</label>
              <select
                name="combustible"
                value={formData.combustible}
                onChange={handleChange}
              >
                <option value="Nafta">Nafta</option>
                <option value="Diesel">Diesel</option>
                <option value="Electrico">Eléctrico</option>
              </select>
            </div>

            <div className="form-group">
              <label>Color</label>
              <input
                type="text"
                name="color"
                value={formData.color}
                onChange={handleChange}
                required
              />
            </div>

            <div
              className="upload-area"
              onDragOver={(e) => e.preventDefault()}
              onDrop={(e) => {
                e.preventDefault();
                const files = Array.from(e.dataTransfer.files);
                const readerPromises = files.map(
                  (file) =>
                    new Promise((resolve, reject) => {
                      const reader = new FileReader();
                      reader.onload = () =>
                        resolve(reader.result.split(",")[1]);
                      reader.onerror = reject;
                      reader.readAsDataURL(file);
                    })
                );

                Promise.all(readerPromises)
                  .then((base64Images) => {
                    setFormData((prev) => ({
                      ...prev,
                      nuevasImagenesBase64: [
                        ...prev.nuevasImagenesBase64,
                        ...base64Images,
                      ],
                    }));
                  })
                  .catch((err) =>
                    console.error("Error al leer imágenes:", err)
                  );
              }}
            >
              <p>Arrastra tus imágenes aquí o haz clic para seleccionar</p>
              <input
                id="imagenes"
                type="file"
                multiple
                accept="image/*"
                onChange={handleFileChange}
                className="upload-input"
              />
            </div>

            <div className="botones-acciones">
              <button type="submit" className="btn-guardar">
                Guardar cambios
              </button>
              <button
                type="button"
                className="btn-cancelar"
                onClick={() => navigate(-1)}
              >
                Cancelar
              </button>
            </div>

            {mensaje && <p className="mensaje-exito">{mensaje}</p>}
            {error && <p className="mensaje-error">{error}</p>}
          </form>
        </div>
      </div>
      <Footer />
    </div>
  );
}
