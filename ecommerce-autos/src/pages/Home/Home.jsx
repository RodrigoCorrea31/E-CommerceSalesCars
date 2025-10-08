import Navbar from "../../components/layout/Navbar";
import Sidebar from "../../components/layout/Sidebar";
import Footer from "../../components/layout/Footer";
import CardAuto from "../../components/common/CardAuto";
import { usePublicaciones } from "../../hooks/usePublicaciones";
import "../../styles/Home.css";

const Home = () => {
  const { publicaciones, loading } = usePublicaciones();

  if (loading) return <p>Cargando publicaciones...</p>;

  return (
    <div className="home-container">
      <Navbar />

      <div className="content-wrapper">
        <Sidebar />
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
