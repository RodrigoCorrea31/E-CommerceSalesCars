import {BrowserRouter, Route, Routes} from 'react-router-dom';
import Home from "../pages/Home/Home";
import Publicaciones from "../pages/Publicaciones/Publicaciones";
import DetallePublicacion from "../pages/Publicaciones/DetallePublicacion";
import Login from "../pages/Usuarios/Login";
import Registro from "../pages/Usuarios/Registro";

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="/publicaciones" element={<Publicaciones/>}/>
                <Route path="/publicaciones/:id" element={<DetallePublicacion/>}/>
                <Route path="/login" element={<Login/>}/>
                <Route path="/registro" element={<Registro/>}/>
            </Routes>
        </BrowserRouter>
    );
};
