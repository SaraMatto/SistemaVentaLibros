const API_BASE_URL = "http://localhost:5125/api/Libro";
const PEDIDO_BASE_URL = "http://localhost:5125/api/Pedido"; // Cambiado a HTTP y puerto correcto

export async function getLibros() {
    const response = await fetch(API_BASE_URL);
    if (!response.ok) throw new Error("Error al obtener libros");
    return await response.json();
}

export async function actualizarLibro(formData) {
    const response = await fetch(API_BASE_URL, {
        method: "PUT",
        body: formData,
    });

    if (!response.ok) throw new Error("Error al actualizar libro");
    return await response.json();
}

export async function eliminarLibro(id) {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: "DELETE",
    });

    if (!response.ok) {
        let errorMsg = "Error al eliminar libro";
        try {
            const errorData = await response.json();
            if (errorData?.mensaje) errorMsg = errorData.mensaje;
        } catch {
            // No action needed: si no hay cuerpo JSON, dejamos el mensaje por defecto
        }
        throw new Error(errorMsg);
    }
    return await response.json();
}
export async function crearLibro(nuevoLibro) {
    const formData = new FormData();

    formData.append("titulo", nuevoLibro.titulo);
    formData.append("autor", nuevoLibro.autor);
    formData.append("isbn", nuevoLibro.isbn || "");
    formData.append("tipoLibro", nuevoLibro.tipoLibro || "");
    formData.append("precio", String(nuevoLibro.precio));
    formData.append("stock", String(nuevoLibro.stock));
    formData.append("idioma", nuevoLibro.idioma || "");
    formData.append("editorial", nuevoLibro.editorial || "");
    formData.append("anioPublicacion", nuevoLibro.anioPublicacion || "");
    formData.append("descripcion", nuevoLibro.descripcion || "");
    formData.append("estadoLibro", nuevoLibro.estadoLibro || "");
    formData.append("categoriaId", nuevoLibro.categoriaId || "");
    formData.append("subcategoriaId", nuevoLibro.subcategoriaId || 0);

    if (nuevoLibro.imagen) {
        formData.append("ImagenFile", nuevoLibro.imagen);
    }

    const response = await fetch(API_BASE_URL, {
        method: "POST",
        body: formData,
    });

    if (!response.ok) {
        let errorMsg = "Error al crear libro";
        try {
            const errorData = await response.json();
            if (errorData?.mensaje) errorMsg = errorData.mensaje;
        } catch {
           // No action needed: si no hay cuerpo JSON, dejamos el mensaje por defecto
        }
        throw new Error(errorMsg);
    }

    return await response.json();
}

export async function getCategorias() {
    const response = await fetch(`${API_BASE_URL}/categorias`);
    if (!response.ok) throw new Error("Error al obtener categorías");
    return await response.json();
}

export async function getSubcategorias() {
    const response = await fetch(`${API_BASE_URL}/subcategorias`);
    if (!response.ok) throw new Error("Error al obtener subcategorías");
    return await response.json();
}

export async function crearPedido(pedido) {
    const response = await fetch(PEDIDO_BASE_URL, { // Usar HTTP y puerto 5125
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(pedido),
    });

    if (!response.ok) {
        let errorMsg = "Error al registrar el pedido";
        try {
            const errorData = await response.json();
            if (errorData?.mensaje) errorMsg = errorData.mensaje;
        } catch {
            // No action needed: si no hay cuerpo JSON, dejamos el mensaje por defecto
        }
        throw new Error(errorMsg);
    }

    return await response.json();
}