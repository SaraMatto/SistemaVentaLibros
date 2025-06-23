import React, { useEffect, useState } from "react";
import "./Pedidos.css";

const Pedidos = () => {
    const [pedidos, setPedidos] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const usuario = JSON.parse(localStorage.getItem("usuario"));

    useEffect(() => {
        const fetchPedidos = async () => {
            try {
                const res = await fetch(`https://localhost:44359/api/Pedido/usuario/${usuario.id}`);
                if (!res.ok) throw new Error("Error al obtener pedidos.");
                const data = await res.json();
                setPedidos(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        if (usuario?.id) {
            fetchPedidos();
        } else {
            setError("Usuario no autenticado.");
            setLoading(false);
        }
    }, [usuario]);

    if (loading) return <p className="pedidos-loading">Cargando pedidos...</p>;
    if (error) return <p className="pedidos-error">{error}</p>;
    if (pedidos.length === 0) return <p className="pedidos-empty">No tienes pedidos registrados.</p>;

    return (
        <div className="pedidos-tabla-wrapper">
            <h2 className="pedidos-title">Mis Pedidos</h2>
            <div className="tabla-responsive">
                <table className="pedidos-tabla">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Fecha</th>
                            <th>Estado</th>
                            <th>Total</th>
                            <th>Dirección de Envío</th>
                        </tr>
                    </thead>
                    <tbody>
                        {pedidos.map(pedido => (
                            <tr key={pedido.pedidoID}>
                                <td>{pedido.pedidoID}</td>
                                <td>{new Date(pedido.fechaPedido).toLocaleDateString()}</td>
                                <td>{pedido.estadoPedido}</td>
                                <td>${pedido.totalPedido.toFixed(2)}</td>
                                <td>{pedido.direccionEnvio}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default Pedidos;
