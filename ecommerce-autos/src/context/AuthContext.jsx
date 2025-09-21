import { createContext, useState } from "react";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [usuario, setUsuario] = useState(null);

    const login = (data) => {
        localStorage.setItem("token", data.token);
        setUsuario(data.usuario);
    };

    const logout = () => {
        localStorage.removeItem("token");
        setUsuario(null);
    };

    return (
        <AuthContext.Provider value={{ usuario, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};