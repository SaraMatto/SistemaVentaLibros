import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Dashboard from "./Views/Dashboard";
import Libros from "./Views/Libros";
import Perfil from "./Views/Perfil";
import Logout from "./Views/Logout";
import Login from "./Components/Login";
import ProtectedRoute from "./Components/ProtectedRoute"; 
import Pedidos from "./Views/Pedidos";

const Inicio = () => (
    <>
        <h1>Bienvenido a Sistema Libros</h1>
        <p>Tu plataforma para gestionar la venta de libros de manera sencilla y eficiente.</p>
    </>
);

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<Login />} />

                <Route
                    path="/dashboard"
                    element={
                        <ProtectedRoute>
                            <Dashboard />
                        </ProtectedRoute>
                    }
                >
                    <Route index element={<Inicio />} />
                    <Route path="libros" element={<Libros />} />
                    <Route path="perfil" element={<Perfil />} />
                    <Route path="pedidos" element={<Pedidos />} />

                </Route>

                <Route path="/logout" element={<Logout />} />
                <Route path="*" element={<Navigate to="/dashboard" replace />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;
