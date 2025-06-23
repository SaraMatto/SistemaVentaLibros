import React, { useEffect, useState } from "react";
import { getLibros } from "../Servicios/LibrosService";
import "./Libros.css";

const API_BASE_URL = "https://localhost:44359";

const Libros = () => {
    const [libros, setLibros] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchLibros = async () => {
            setLoading(true);
            setError("");
            try {
                const data = await getLibros();
                setLibros(data);
            } catch (err) {
                setError(err.message || "Error al cargar libros");
            } finally {
                setLoading(false);
            }
        };

        fetchLibros();
    }, []);

    const handleComprarClick = (libroId) => {
        alert(`¡Compraste el libro con ID: ${libroId}! (Esta es una acción simulada)`);
        // Here you would typically add logic to handle the purchase,
        // e.g., add to a shopping cart, redirect to a checkout page, etc.
    };

    return (
        <div className="libros-wrapper">
            <h1 className="libros-title">Lista de Libros</h1>
            {loading && <p className="loading">Cargando libros...</p>}
            {error && <p className="error">{error}</p>}

            {!loading && !error && libros.length === 0 && (
                <p className="no-libros">No hay libros disponibles.</p>
            )}

            <div className="libros-grid">
                {libros.map((libro) => (
                    <div key={libro.id} className="libro-card">
                        {libro.imagen && (
                            <div className="libro-imagen">
                                <img
                                    src={`${API_BASE_URL}/img/${libro.imagen}`}
                                    alt={libro.titulo}
                                    style={{ width: "150px", height: "220px", objectFit: "cover" }}
                                />
                            </div>
                        )}
                        <div className="libro-info">
                            <h3 title={libro.titulo}>{libro.titulo}</h3>
                            <p><strong>Autor:</strong> {libro.autor}</p>
                            <p><strong>Precio:</strong> ${libro.precio}</p>
                            <p><strong>Editorial:</strong> {libro.editorial}</p>
                        </div>
                        <div className="libro-actions">
                            <button className="btn-comprar" onClick={() => handleComprarClick(libro.id)}>Comprar</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Libros;