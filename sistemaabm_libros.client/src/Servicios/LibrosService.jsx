export async function getLibros() {
    const res = await fetch("https://localhost:44359/api/Libro"); // Cambia la URL según tu API
    if (!res.ok) throw new Error("Error al obtener libros");
    return await res.json();
}