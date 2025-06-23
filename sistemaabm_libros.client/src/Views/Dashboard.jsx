import React from "react";
import { NavLink, Outlet } from "react-router-dom";
import "./Dashboard.css";

const Dashboard = () => {
    const menuItems = [
        { id: "inicio", label: "Inicio", href: "/dashboard" },
        { id: "libros", label: "Libros", href: "/dashboard/libros" },
        { id: "pedidos", label: "Pedidos", href: "/dashboard/pedidos" },
        { id: "perfil", label: "Perfil", href: "/dashboard/perfil" },
        { id: "logout", label: "Cerrar sesión", href: "/logout" },
    ];

    return (
        <div className="dashboard-wrapper">
            <nav className="dashboard-nav">
                <h2 className="dashboard-logo">Sistema Libros</h2>
                <ul className="dashboard-menu">
                    {menuItems.map(({ id, label, href }) => (
                        <li key={id}>
                            <NavLink
                                to={href}
                                className={({ isActive }) => (isActive ? "active" : "")}
                            >
                                {label}
                            </NavLink>
                        </li>
                    ))}
                </ul>
            </nav>

            <main className="dashboard-main">
                {/* Aquí se renderiza la ruta hija activa */}
                <Outlet />
            </main>

            <footer className="dashboard-footer">
                <p>© 2025 Sistema Libros. Todos los derechos reservados.</p>
            </footer>
        </div>
    );
};

export default Dashboard;
