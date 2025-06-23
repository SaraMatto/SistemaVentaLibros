import React, { useEffect, useState } from "react";
import "./Perfil.css";

const Perfil = () => {
    const usuarioGuardado = JSON.parse(localStorage.getItem("usuario"));
    const [formData, setFormData] = useState({
        id: usuarioGuardado?.id || 0,
        nombre: "",
        apellido: "",
        email: "",
        telefono: "",
        fechaNacimiento: "",
    });

    const [mensaje, setMensaje] = useState("");
    const [error, setError] = useState("");

    useEffect(() => {
        // Cargar datos del backend con el ID
        if (usuarioGuardado?.id) {
            fetch(`https://localhost:44359/api/Usuario/${usuarioGuardado.id}`)
                .then(res => res.json())
                .then(data => {
                    setFormData({
                        id: data.id,
                        nombre: data.nombre || "",
                        apellido: data.apellido || "",
                        email: data.email || "",
                        telefono: data.telefono || "",
                        fechaNacimiento: data.fechaNacimiento?.split("T")[0] || "",
                    });
                })
                .catch(() => setError("No se pudo cargar el perfil"));
        }
    }, [usuarioGuardado?.id]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMensaje("");
        setError("");

        try {
            const res = await fetch("https://localhost:44359/api/Usuario", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(formData)
            });

            const resultado = await res.json();
            if (!res.ok) {
                throw new Error(resultado.mensaje || "Error al actualizar");
            }

            setMensaje("Perfil actualizado correctamente");
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="perfil-container">
            <h2>Mi Perfil</h2>
            {mensaje && <p className="perfil-mensaje">{mensaje}</p>}
            {error && <p className="perfil-error">{error}</p>}

            <form className="perfil-form" onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="nombre"
                    placeholder="Nombre"
                    value={formData.nombre}
                    onChange={handleChange}
                    required
                />
                <input
                    type="text"
                    name="apellido"
                    placeholder="Apellido"
                    value={formData.apellido}
                    onChange={handleChange}
                    required
                />
                <input
                    type="email"
                    name="email"
                    placeholder="Correo electrónico"
                    value={formData.email}
                    onChange={handleChange}
                    required
                    disabled // no editable
                />
                <input
                    type="tel"
                    name="telefono"
                    placeholder="Teléfono"
                    value={formData.telefono}
                    onChange={handleChange}
                />
                <input
                    type="date"
                    name="fechaNacimiento"
                    value={formData.fechaNacimiento}
                    onChange={handleChange}
                />

                <button type="submit">Guardar cambios</button>
            </form>
        </div>
    );
};

export default Perfil;
