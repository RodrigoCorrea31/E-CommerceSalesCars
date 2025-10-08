import {BrowserRouter, Route, Routes} from 'react-router-dom';
import Home from "../pages/Home/Home";
import Login from "../pages/Usuarios/Login";
import Registro from "../pages/Usuarios/Registro";
import CrearPublicacion from '../pages/Publicaciones/CrearPublicacion';

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="/login" element={<Login/>}/>
                <Route path="/registro" element={<Registro/>}/>
                <Route path="/publicaciones/crear" element={<CrearPublicacion />} />
            </Routes>
        </BrowserRouter>
    );
};
