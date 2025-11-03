import { useState, useEffect } from "react";
import Navbar from "../../components/layout/Navbar";
import Sidebar from "../../components/layout/Sidebar";
import Footer from "../../components/layout/Footer";
import CardAuto from "../../components/common/CardAuto";
import { usePublicaciones } from "../../hooks/usePublicaciones";
import "../../styles/Home.css";

const Home = () => {
  const { publicaciones, loading, aplicarFiltros } = usePublicaciones();
  const [sidebarAbierto, setSidebarAbierto] = useState(false);

  // Variables para detectar swipe
  let touchStartX = 0;
  let touchEndX = 0;

  const handleTouchStart = (e) => {
    touchStartX = e.changedTouches[0].screenX;
  };

  const handleTouchEnd = (e) => {
    touchEndX = e.changedTouches[0].screenX;
    if (touchEndX - touchStartX > 50) {
      // swipe a la derecha
      setSidebarAbierto(true);
    } else if (touchStartX - touchEndX > 50) {
      // swipe a la izquierda
      setSidebarAbierto(false);
    }
  };

  useEffect(() => {
    document.addEventListener("touchstart", handleTouchStart);
    document.addEventListener("touchend", handleTouchEnd);

    return () => {
      document.removeEventListener("touchstart", handleTouchStart);
      document.removeEventListener("touchend", handleTouchEnd);
    };
  }, []);

  if (loading) return <p>Cargando publicaciones...</p>;

  return (
    <div className="home-container">
      <Navbar />

      <div className="content-wrapper">
        <Sidebar 
          abierto={sidebarAbierto} 
          setAbierto={setSidebarAbierto} 
          onAplicarFiltros={aplicarFiltros} 
        />

        <main className="publicacion-grid">
          {publicaciones.map((pub) => (
            <CardAuto key={pub.id} publicacion={pub} />
          ))}
        </main>
      </div>

      <Footer />
    </div>
  );
};

export default Home;
