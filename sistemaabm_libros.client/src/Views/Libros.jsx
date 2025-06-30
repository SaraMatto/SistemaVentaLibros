import React, { useEffect, useState } from "react";
import {
    getLibros,
    actualizarLibro,
    eliminarLibro,
    crearLibro,
    getCategorias,
    getSubcategorias,
    crearPedido
} from "../Servicios/LibrosService";
import "./Libros.css";

const inputStyle = {
    width: "100%",
    padding: "0.5rem",
    marginTop: "0.25rem",
    marginBottom: "1rem",
    borderRadius: "6px",
    border: "1px solid #ccc",
    textAlign: "left",
};

const Libros = () => {
    const [modalComprar, setModalComprar] = useState(null);
    const [pedidoCantidad, setPedidoCantidad] = useState(1);
    const [pedidoDireccion, setPedidoDireccion] = useState("");

    const [libros, setLibros] = useState([]);
    const [categorias, setCategorias] = useState([]);
    const [subcategorias, setSubcategorias] = useState([]);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const [modalCrear, setModalCrear] = useState(false);
    const [modalEditar, setModalEditar] = useState(null);
    const [modalEliminar, setModalEliminar] = useState(null);

    const [editarDatos, setEditarDatos] = useState({});
    const [nuevoLibro, setNuevoLibro] = useState({
        titulo: "",
        autor: "",
        isbn: "",
        tipoLibro: "",
        precio: 0,
        stock: 0,
        idioma: "",
        editorial: "",
        anioPublicacion: "",
        descripcion: "",
        estadoLibro: "",
        categoriaId: "",
        subcategoriaId: "",
        imagen: null,
    });

    const usuario = JSON.parse(localStorage.getItem("usuario"));
    const esCliente = usuario?.esCliente != true;
    console.log(esCliente);
    console.log(usuario);

    useEffect(() => {
        const fetchAll = async () => {
            setLoading(true);
            try {
                const [librosData, cats, subs] = await Promise.all([
                    getLibros(),
                    getCategorias(),
                    getSubcategorias(),
                ]);
                setLibros(librosData);
                setCategorias(cats);
                setSubcategorias(subs);
            } catch (e) {
                setError(e.message);
            } finally {
                setLoading(false);
            }
        };
        fetchAll();
    }, []);

    const handleInputChange = (e, target = "nuevo") => {
        const { name, value, files, type } = e.target;
        const setter = target === "nuevo" ? setNuevoLibro : setEditarDatos;
        const base = target === "nuevo" ? nuevoLibro : editarDatos;

        let newVal = type === "file" ? files[0] || null : value;

        setter({ ...base, [name]: newVal });
    };

    const handleComprar = (libro) => {
        setModalComprar(libro);
        setPedidoCantidad(1);
        setPedidoDireccion("");
    };

    const handleConfirmarCompra = async (e) => {
        e.preventDefault();

        const usuario = JSON.parse(localStorage.getItem("usuario"));
        if (!usuario?.id) {
            alert("Debes iniciar sesión para comprar.");
            return;
        }

        const pedido = {
            usuarioId: usuario.id,
            fechaPedido: new Date().toISOString(),
            estadoPedido: "Pendiente",
            totalPedido: modalComprar.precio * pedidoCantidad,
            direccionEnvio: pedidoDireccion,
            detalles: [
                {
                    libroId: modalComprar.id,
                    cantidad: parseInt(pedidoCantidad),
                    precioUnitario: modalComprar.precio,
                    tituloLibro: modalComprar.titulo,
                },
            ],
        };

        try {
            await crearPedido(pedido); // ✅ usamos el nuevo servicio
            alert("¡Pedido realizado con éxito!");
            setModalComprar(null);
        } catch (err) {
            alert("Error al procesar la compra: " + err.message);
        }
    };



    const handleCrearSubmit = async (e) => {
        e.preventDefault();
        try {
            const creado = await crearLibro(nuevoLibro); // solo pasa el objeto
            setLibros([...libros, creado]);
            setModalCrear(false);
            setNuevoLibro({
                titulo: "",
                autor: "",
                isbn: "",
                tipoLibro: "",
                precio: 0,
                stock: 0,
                idioma: "",
                editorial: "",
                anioPublicacion: "",
                descripcion: "",
                estadoLibro: "",
                categoriaId: "",
                subcategoriaId: "",
                imagen: null,
            });
            window.location.reload(); 
        } catch (e) {
            alert("Error al crear libro: " + e.message);
        }
    };

    const handleAbrirEditar = (libro) => {
        // Obtener categoriaId desde libro o buscar por subcategoria si no viene directo
        let categoriaIdStr = "";
        if (libro.categoriaId) {
            categoriaIdStr = String(libro.categoriaId);
        } else if (libro.subcategoriaId) {
            const sub = subcategorias.find(
                (s) =>
                    s.id === libro.subcategoriaId ||
                    String(s.id) === String(libro.subcategoriaId)
            );
            if (sub) categoriaIdStr = String(sub.categoriaId);
        }

        setEditarDatos({
            ...libro,
            categoriaId: categoriaIdStr,
            subcategoriaId: libro.subcategoriaId ? String(libro.subcategoriaId) : "",
        });
        setModalEditar(libro);
    };

    const handleGuardarEdicion = async () => {
        const formData = new FormData();

        formData.append("id", editarDatos.id);
        formData.append("titulo", editarDatos.titulo);
        formData.append("autor", editarDatos.autor);
        formData.append("isbn", editarDatos.isbn || "");
        formData.append("tipoLibro", editarDatos.tipoLibro || "");
        formData.append("precio", editarDatos.precio);
        formData.append("stock", editarDatos.stock);
        formData.append("idioma", editarDatos.idioma || "");
        formData.append("editorial", editarDatos.editorial || "");
        formData.append("anioPublicacion", editarDatos.anioPublicacion || "");
        formData.append("descripcion", editarDatos.descripcion || "");
        formData.append("estadoLibro", editarDatos.estadoLibro || "");
        formData.append("subcategoriaId", editarDatos.subcategoriaId);

        if (editarDatos.imagen instanceof File) {
            formData.append("ImagenFile", editarDatos.imagen); // <- nombre debe coincidir con el parámetro del backend
        }

        try {
            await actualizarLibro(formData);
            setModalEditar(null);
            window.location.reload(); // fuerza recarga completa para actualizar la vista
        } catch (e) {
            alert("Error al actualizar: " + e.message);
        }

    };


    const handleEliminar = (libro) => {
        setModalEliminar(libro);
    };

    const confirmarEliminar = async () => {
        try {
            await eliminarLibro(modalEliminar.id);
            setLibros(libros.filter((l) => l.id !== modalEliminar.id));
            setModalEliminar(null);
            alert(`Libro "${modalEliminar.titulo}" eliminado correctamente.`);
        } catch (e) {
            alert("Error al eliminar libro: " + e.message);
        }
    };


    return (
        <div className="libros-wrapper">
            {!esCliente && (
                <div style={{ textAlign: "right", marginBottom: "1.5rem" }}>
                    <button className="btn-crear" onClick={() => setModalCrear(true)}>
                        Crear Libro
                    </button>
                </div>
            )}
            <h1 className="libros-title">Lista de Libros</h1>
            {loading && <p className="loading">Cargando...</p>}
            {error && <p className="error">{error}</p>}
            <div className="libros-grid">
                {libros.map((libro) => (
                    <div key={libro.id} className="libro-card">
                        {libro.imagen && (
                            <div className="libro-imagen">
                                <img src={`https://localhost:44359/img/${libro.imagen}`} alt={libro.titulo} />
                            </div>
                        )}
                        <div className="libro-info">
                            <h3>{libro.titulo}</h3>
                            <p>
                                <strong>Autor:</strong> {libro.autor}
                            </p>
                            <p>
                                <strong>Precio:</strong> ${libro.precio}
                            </p>
                        </div>

                        {/* Botones según tipo de usuario */}
                        <div className="libro-actions">
                            {esCliente ? (
                                <button className="btn-comprar" onClick={() => handleComprar(libro)}>
                                    Comprar
                                </button>
                            ) : (
                                <>
                                    <button className="btn-editar" onClick={() => handleAbrirEditar(libro)}>
                                        Editar
                                    </button>
                                    <button className="btn-eliminar" onClick={() => handleEliminar(libro)}>
                                        Eliminar
                                    </button>
                                </>
                            )}
                        </div>
                    </div>
                ))}
            </div>

            {/* Modal COMPRAR */}
            {modalComprar && (
                <div className="modal-overlay" onClick={() => setModalComprar(null)}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()} style={{ maxWidth: "500px" }}>
                        <button className="close-btn" onClick={() => setModalComprar(null)}>
                            &times;
                        </button>
                        <h3 className="modal-title">Comprar Libro: {modalComprar.titulo}</h3>
                        <form onSubmit={handleConfirmarCompra}>
                            <label style={{ textAlign: "left" }}>
                                Cantidad:
                                <input
                                    type="number"
                                    name="cantidad"
                                    min="1"
                                    max={modalComprar.stock}
                                    value={pedidoCantidad}
                                    onChange={(e) => setPedidoCantidad(e.target.value)}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Dirección de Envío:
                                <input
                                    type="text"
                                    name="direccionEnvio"
                                    value={pedidoDireccion}
                                    onChange={(e) => setPedidoDireccion(e.target.value)}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            <div style={{ display: "flex", justifyContent: "center", gap: "1rem", marginTop: "1rem" }}>
                                <button type="submit" className="btn-comprar">
                                    Confirmar Compra
                                </button>
                                <button type="button" className="btn-cancelar" onClick={() => setModalComprar(null)}>
                                    Cancelar
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}


            {/* Modal CREAR */}
            {modalCrear && (
                <div className="modal-overlay" onClick={() => setModalCrear(false)}>
                    <div
                        className="modal-content"
                        onClick={(e) => e.stopPropagation()}
                        style={{ maxWidth: "600px", position: "relative" }}
                    >
                        <button
                            className="close-btn"
                            onClick={() => setModalCrear(false)}
                            style={{
                                position: "absolute",
                                top: "10px",
                                right: "15px",
                                background: "none",
                                border: "none",
                                fontSize: "1.5rem",
                                cursor: "pointer",
                                color: "#999",
                            }}
                            title="Cerrar"
                        >
                            &times;
                        </button>
                        <h3 className="modal-title">Crear Nuevo Libro</h3>
                        <form onSubmit={handleCrearSubmit}>
                            <label style={{ textAlign: "left" }}>
                                Título:
                                <input
                                    type="text"
                                    name="titulo"
                                    value={nuevoLibro.titulo || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Autor:
                                <input
                                    type="text"
                                    name="autor"
                                    value={nuevoLibro.autor || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                ISBN:
                                <input
                                    type="text"
                                    name="isbn"
                                    value={nuevoLibro.isbn || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Precio:
                                <input
                                    type="number"
                                    name="precio"
                                    min="0"
                                    step="0.01"
                                    value={nuevoLibro.precio || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Stock:
                                <input
                                    type="number"
                                    name="stock"
                                    min="0"
                                    value={nuevoLibro.stock || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Idioma:
                                <input
                                    type="text"
                                    name="idioma"
                                    value={nuevoLibro.idioma || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Editorial:
                                <input
                                    type="text"
                                    name="editorial"
                                    value={nuevoLibro.editorial || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Año de Publicación:
                                <input
                                    type="text"
                                    name="anioPublicacion"
                                    maxLength={4}
                                    pattern="\d{4}"
                                    title="Ingrese un año válido de 4 dígitos"
                                    value={nuevoLibro.anioPublicacion || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    placeholder="Ej: 2021"
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Descripción:
                                <textarea
                                    name="descripcion"
                                    rows={3}
                                    value={nuevoLibro.descripcion || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Estado Libro:
                                <input
                                    type="text"
                                    name="estadoLibro"
                                    value={nuevoLibro.estadoLibro || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                />
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Tipo:
                                <select
                                    name="tipoLibro"
                                    value={nuevoLibro.tipoLibro || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    required
                                >
                                    <option value="">Seleccione tipo</option>
                                    <option value="Nuevo">Nuevo</option>
                                    <option value="Usado">Usado</option>
                                </select>
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Categoría:
                                <select
                                    name="categoriaId"
                                    value={nuevoLibro.categoriaId || ""}
                                    onChange={(e) => {
                                        const newCategoriaId = e.target.value;
                                        handleInputChange(e); // para actualizar nuevoLibro.categoriaId
                                        setNuevoLibro((prev) => ({
                                            ...prev,
                                            categoriaId: newCategoriaId,
                                            subcategoriaId: "",
                                        }));
                                    }}
                                    style={inputStyle}
                                    required
                                >
                                    <option value="">Seleccione categoría</option>
                                    {categorias.map((c) => (
                                        <option key={c.id} value={String(c.id)}>
                                            {c.nombreCategoria || c.nombre}
                                        </option>
                                    ))}
                                </select>
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Subcategoría:
                                <select
                                    name="subcategoriaId"
                                    value={nuevoLibro.subcategoriaId || ""}
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    required
                                    disabled={!nuevoLibro.categoriaId}
                                >
                                    <option value="">Seleccione subcategoría</option>
                                    {subcategorias
                                        .filter((s) => String(s.categoriaId) === String(nuevoLibro.categoriaId))
                                        .map((s) => (
                                            <option key={s.id} value={String(s.id)}>
                                                {s.nombreSubcategoria || s.nombre}
                                            </option>
                                        ))}
                                </select>
                            </label>

                            <label style={{ textAlign: "left" }}>
                                Imagen:
                                <input
                                    type="file"
                                    name="imagen"
                                    onChange={handleInputChange}
                                    style={inputStyle}
                                    accept="image/*"
                                />
                            </label>

                            <div style={{ display: "flex", justifyContent: "center", gap: "1rem", marginTop: "1rem" }}>
                                <button type="submit" className="btn-editar">
                                    Crear
                                </button>
                                <button type="button" className="btn-cancelar" onClick={() => setModalCrear(false)}>
                                    Cancelar
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}


            {/* Modal EDITAR */}
            {modalEditar && (
                <div className="modal-overlay" onClick={() => setModalEditar(null)}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()} style={{ maxWidth: "600px" }}>
                        <button
                            className="close-btn"
                            onClick={() => setModalEditar(null)}
                            style={{
                                position: "absolute",
                                top: "10px",
                                right: "15px",
                                background: "none",
                                border: "none",
                                fontSize: "1.5rem",
                                cursor: "pointer",
                                color: "#999",
                            }}
                            title="Cerrar"
                        >
                            &times;
                        </button>
                        <h3 className="modal-title">Editar Libro: {modalEditar.titulo}</h3>
                        <form
                            onSubmit={(e) => {
                                e.preventDefault();
                                handleGuardarEdicion();
                            }}
                        >
                            {/* Título */}
                            <label style={{ textAlign: "left" }}>
                                Título:
                                <input
                                    type="text"
                                    name="titulo"
                                    value={editarDatos.titulo || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            {/* Autor */}
                            <label style={{ textAlign: "left" }}>
                                Autor:
                                <input
                                    type="text"
                                    name="autor"
                                    value={editarDatos.autor || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            {/* ISBN */}
                            <label style={{ textAlign: "left" }}>
                                ISBN:
                                <input
                                    type="text"
                                    name="isbn"
                                    value={editarDatos.isbn || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                />
                            </label>

                            {/* Precio */}
                            <label style={{ textAlign: "left" }}>
                                Precio:
                                <input
                                    type="number"
                                    name="precio"
                                    min="0"
                                    step="0.01"
                                    value={editarDatos.precio || 0}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            {/* Stock */}
                            <label style={{ textAlign: "left" }}>
                                Stock:
                                <input
                                    type="number"
                                    name="stock"
                                    min="0"
                                    value={editarDatos.stock || 0}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    required
                                />
                            </label>

                            {/* Idioma */}
                            <label style={{ textAlign: "left" }}>
                                Idioma:
                                <input
                                    type="text"
                                    name="idioma"
                                    value={editarDatos.idioma || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                />
                            </label>

                            {/* Editorial */}
                            <label style={{ textAlign: "left" }}>
                                Editorial:
                                <input
                                    type="text"
                                    name="editorial"
                                    value={editarDatos.editorial || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                />
                            </label>

                            {/* Año de Publicación (texto, solo año) */}
                            <label style={{ textAlign: "left" }}>
                                Año de Publicación:
                                <input
                                    type="text"
                                    name="anioPublicacion"
                                    maxLength={4}
                                    pattern="\d{4}"
                                    title="Ingrese un año válido de 4 dígitos"
                                    value={editarDatos.anioPublicacion || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    placeholder="Ej: 2021"
                                />
                            </label>

                            {/* Descripción */}
                            <label style={{ textAlign: "left" }}>
                                Descripción:
                                <textarea
                                    name="descripcion"
                                    rows={3}
                                    value={editarDatos.descripcion || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                />
                            </label>

                            {/* Estado Libro (input texto) */}
                            <label style={{ textAlign: "left" }}>
                                Estado Libro:
                                <input
                                    type="text"
                                    name="estadoLibro"
                                    value={editarDatos.estadoLibro || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    
                                />
                            </label>
                            {/* Tipo (input texto) */}
                            <label style={{ textAlign: "left" }}>
                                Tipo:
                                <select
                                    name="tipoLibro"
                                    value={editarDatos.tipoLibro || ""}
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    required
                                >
                                    <option value="">Seleccione tipo</option>
                                    <option value="Nuevo">Nuevo</option>
                                    <option value="Usado">Usado</option>
                                </select>
                            </label>

                            {/* Categoría */}
                            <label style={{ textAlign: "left" }}>
                                Categoría:
                                <select
                                    name="categoriaId"
                                    value={editarDatos.categoriaId || ""}
                                    onChange={(e) => {
                                        const newCategoriaId = e.target.value;
                                        setEditarDatos((prev) => ({
                                            ...prev,
                                            categoriaId: newCategoriaId,
                                            subcategoriaId: "", // Limpiar subcategoría al cambiar categoría
                                        }));
                                    }}
                                    style={inputStyle}
                                    required
                                >
                                    <option value="">Seleccione categoría</option>
                                    {categorias.map((c) => (
                                        <option key={c.id} value={String(c.id)}>
                                            {c.nombreCategoria}
                                        </option>
                                    ))}
                                </select>
                            </label>

                            {/* Subcategoría */}
                            <label style={{ textAlign: "left" }}>
                                Subcategoría:
                                <select
                                    name="subcategoriaId"
                                    value={editarDatos.subcategoriaId || ""}
                                    onChange={(e) =>
                                        setEditarDatos((prev) => ({ ...prev, subcategoriaId: e.target.value }))
                                    }
                                    style={inputStyle}
                                    required
                                    disabled={!editarDatos.categoriaId}
                                >
                                    <option value="">Seleccione subcategoría</option>
                                    {subcategorias
                                        .filter((s) => String(s.categoriaId) === String(editarDatos.categoriaId))
                                        .map((s) => (
                                            <option key={s.id} value={String(s.id)}>
                                                {s.nombreSubcategoria}
                                            </option>
                                        ))}
                                </select>
                            </label>


                            {/* Imagen */}
                            <label style={{ textAlign: "left" }}>
                                Imagen:
                                <input
                                    type="file"
                                    name="imagen"
                                    onChange={(e) => handleInputChange(e, "editar")}
                                    style={inputStyle}
                                    accept="image/*"
                                />
                            </label>

                            {/* Botones */}
                            <div style={{ display: "flex", justifyContent: "center", gap: "1rem", marginTop: "1rem" }}>
                                <button type="submit" className="btn-editar">
                                    Guardar
                                </button>
                                <button type="button" className="btn-cancelar" onClick={() => setModalEditar(null)}>
                                    Cancelar
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}


            {/* Modal ELIMINAR */}
            {modalEliminar && (
                <div className="modal-overlay" onClick={() => setModalEliminar(null)}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                        <h3 className="modal-title">Confirmar Eliminación</h3>
                        <p>
                            ¿Estás seguro de eliminar el libro <strong>"{modalEliminar.titulo}"</strong>?
                        </p>
                        <div style={{ marginTop: "1.5rem", display: "flex", gap: "1rem", justifyContent: "center" }}>
                            <button className="btn-eliminar" onClick={confirmarEliminar}>
                                Eliminar
                            </button>
                            <button className="btn-cancelar" onClick={() => setModalEliminar(null)}>
                                Cancelar
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Libros;
