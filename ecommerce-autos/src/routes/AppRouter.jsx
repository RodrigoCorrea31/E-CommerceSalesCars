import {BrowserRouter, Route, Routes} from 'react-router-dom';
import Home from "../pages/Home/Home";
import Login from "../pages/Usuarios/Login";
import Registro from "../pages/Usuarios/Registro";
import CrearPublicacion from '../pages/Publicaciones/CrearPublicacion';
import DetallePublicacion from '../pages/Publicaciones/DetallePublicacion';
import MisOfertas from '../pages/Ofertas/MisOfertas';
import MisPublicaciones from '../pages/Publicaciones/MisPublicaciones';
import OfertasPorPublicacion from '../pages/Publicaciones/OfertasPorPublicacion';
import FinalizarTransaccion from '../pages/Transacciones/FinalizarTransaccion';
import MisTransacciones from '../pages/Usuarios/MisTransacciones';
import EditarPublicacion from '../pages/Publicaciones/EditarPublicacion';

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="/login" element={<Login/>}/>
                <Route path="/registro" element={<Registro/>}/>
                <Route path="/publicaciones/crear" element={<CrearPublicacion />} />
                <Route path="/publicaciones/:id" element={<DetallePublicacion />} />
                <Route path="/ofertas" element={<MisOfertas />} />
                <Route path="/mis-publicaciones" element={<MisPublicaciones />} />
                <Route path="/mis-publicaciones/:id/ofertas" element={<OfertasPorPublicacion />} />
                <Route path="/finalizar-transaccion/:id" element={<FinalizarTransaccion />} />
                <Route path="/mis-transacciones" element={<MisTransacciones />} />
                <Route path="/editar-publicacion/:id" element={<EditarPublicacion />} />
            </Routes>
        </BrowserRouter>
    );
};
