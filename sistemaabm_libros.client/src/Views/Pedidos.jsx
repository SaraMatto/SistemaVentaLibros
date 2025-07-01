import React, { useEffect, useRef, useState } from "react";
import "./Pedidos.css";

const API_PEDIDO_URL = "http://localhost:5125/api/Pedido";

const Pedidos = () => {
    const [pedidos, setPedidos] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [modalPedido, setModalPedido] = useState(null);
    const [modalEstado, setModalEstado] = useState({ abierto: false, pedidoId: null });

    const abrirModalEstado = (pedidoId) => {
        setModalEstado({ abierto: true, pedidoId });
    };

    const cerrarModalEstado = () => {
        setModalEstado({ abierto: false, pedidoId: null });
    };

    const usuarioRef = useRef(JSON.parse(localStorage.getItem("usuario")));

    const handleEliminarPedido = async (pedidoId) => {
        if (!window.confirm("¿Estás seguro de eliminar este pedido?")) return;

        try {
            const res = await fetch(`${API_PEDIDO_URL}/${pedidoId}`, {
                method: "DELETE",
            });
            if (!res.ok) throw new Error("Error al eliminar el pedido.");
            setPedidos(pedidos.filter((p) => p.pedidoID !== pedidoId));
            alert("Pedido eliminado correctamente.");
        } catch (err) {
            alert("Error al eliminar: " + err.message);
        }
    };

    const handleCambiarEstado = async (nuevoEstado) => {
        if (!nuevoEstado) return;

        try {
            const res = await fetch(
                `${API_PEDIDO_URL}/CambiarEstado/${modalEstado.pedidoId}`,
                {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(nuevoEstado),
                }
            );
            if (!res.ok) throw new Error("Error al cambiar estado.");

            setPedidos((prev) =>
                prev.map((p) =>
                    p.pedidoID === modalEstado.pedidoId ? { ...p, estadoPedido: nuevoEstado } : p
                )
            );
            alert("Estado actualizado correctamente.");
            cerrarModalEstado();
        } catch (err) {
            alert("Error al cambiar estado: " + err.message);
        }
    };

    useEffect(() => {
        const fetchPedidos = async () => {
            try {
                const res = await fetch(
                    `${API_PEDIDO_URL}/usuario/${usuarioRef.current.id}`
                );
                if (!res.ok) throw new Error("Error al obtener pedidos.");
                const data = await res.json();
                setPedidos(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        if (usuarioRef.current?.id) {
            fetchPedidos();
        } else {
            setError("Usuario no autenticado.");
            setLoading(false);
        }
    }, []);

    if (loading) return <p className="pedidos-loading">Cargando pedidos...</p>;
    if (error) return <p className="pedidos-error">{error}</p>;
    if (pedidos.length === 0)
        return <p className="pedidos-empty">No tienes pedidos registrados.</p>;

    return (
        <div className="pedidos-tabla-wrapper">
            <h2 className="pedidos-title">Mis Pedidos</h2>
            <table className="pedidos-tabla">
                <thead>
                    <tr>
                        <th>Fecha</th>
                        <th>Estado</th>
                        <th>Total</th>
                        <th>Dirección</th>
                        <th>Acción</th>
                    </tr>
                </thead>
                <tbody>
                    {pedidos.map((pedido) => (
                        <tr key={pedido.pedidoID} className="pedido-row">
                            <td>{new Date(pedido.fechaPedido).toLocaleDateString()}</td>
                            <td>{pedido.estadoPedido}</td>
                            <td>${pedido.totalPedido.toFixed(2)}</td>
                            <td>{pedido.direccionEnvio}</td>
                            <td className="acciones-pedido">
                                <div className="acciones-admin">
                                    <button
                                        className="boton-accion btn-detalle"
                                        onClick={() => setModalPedido(pedido)}
                                    >
                                        Ver detalle
                                    </button>

                                    {usuarioRef.current.esCliente && (
                                        <>
                                            {pedido.estadoPedido !== "Entregado" && pedido.estadoPedido !== "Cancelado" && (
                                                <button
                                                    className="boton-accion btn-estado"
                                                    onClick={() => abrirModalEstado(pedido.pedidoID)}
                                                >
                                                    Cambiar estado
                                                </button>
                                            )}
                                            <button
                                                className="boton-accion btn-eliminar"
                                                onClick={() => handleEliminarPedido(pedido.pedidoID)}
                                            >
                                                Eliminar
                                            </button>
                                        </>
                                    )}
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalPedido && modalPedido.detalles?.length > 0 && (
                <div className="modal-overlay" onClick={() => setModalPedido(null)}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                        <h3 className="modal-title">Detalles del Pedido</h3>

                        <div className="modal-info">
                            <p><strong>Fecha:</strong> {new Date(modalPedido.fechaPedido).toLocaleDateString()}</p>
                            <p><strong>Estado:</strong> {modalPedido.estadoPedido}</p>
                            <p><strong>Total Pedido:</strong> ${modalPedido.totalPedido.toFixed(2)}</p>
                            <p><strong>Dirección:</strong> {modalPedido.direccionEnvio}</p>
                        </div>

                        <h4 className="modal-subtitle">Libros:</h4>
                        <div className="detalle-contenedor">
                            {modalPedido.detalles.map((detalle) => (
                                <div key={detalle.id} className="detalle-item">
                                    <span className="titulo-libro">{detalle.tituloLibro}</span>
                                    <span>Cantidad: {detalle.cantidad}</span>
                                    <span>Precio Unitario: ${detalle.precioUnitario.toFixed(2)}</span>
                                    <span>Subtotal: ${(detalle.cantidad * detalle.precioUnitario).toFixed(2)}</span>
                                </div>
                            ))}
                        </div>

                        <div className="modal-subtotal">
                            <strong>
                                Subtotal (detalles): $
                                {modalPedido.detalles.reduce(
                                    (acc, item) => acc + item.cantidad * item.precioUnitario,
                                    0
                                ).toFixed(2)}
                            </strong>
                        </div>

                        <button className="btn-cerrar" onClick={() => setModalPedido(null)}>
                            Cerrar
                        </button>
                    </div>
                </div>
            )}

            {modalEstado.abierto && (
                <div className="modal-overlay" onClick={cerrarModalEstado}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                        <h3 className="modal-title">Cambiar estado del pedido</h3>
                        <select
                            className="modal-select"
                            defaultValue=""
                            onChange={(e) => handleCambiarEstado(e.target.value)}
                        >
                            <option value="" disabled>
                                Selecciona un estado
                            </option>
                            <option value="Cancelado">Cancelado</option>
                            <option value="Entregado">Entregado</option>
                        </select>
                        <button className="btn-cerrar" onClick={cerrarModalEstado}>
                            Cancelar
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Pedidos;