import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login, register } from "../Servicios/authService";
import "./Login.css";

const Login = () => {
    const [isLogin, setIsLogin] = useState(true);
    const [email, setEmail] = useState("");
    const [contrasena, setContrasena] = useState("");
    const [nombre, setNombre] = useState("");
    const [apellido, setApellido] = useState("");
    const [telefono, setTelefono] = useState("");
    const [fechaNacimiento, setFechaNacimiento] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const validarFormulario = () => {
        if (!email || !contrasena) {
            setError("El email y la contraseña son obligatorios.");
            return false;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            setError("El correo electrónico no es válido.");
            return false;
        }

        if (contrasena.length < 6) {
            setError("La contraseña debe tener al menos 6 caracteres.");
            return false;
        }

        if (!isLogin) {
            if (!nombre || !apellido) {
                setError("Nombre y apellido son obligatorios.");
                return false;
            }

            if (telefono && !/^\d{7,15}$/.test(telefono)) {
                setError("El teléfono debe contener solo números (7 a 15 dígitos).");
                return false;
            }

            if (fechaNacimiento && isNaN(Date.parse(fechaNacimiento))) {
                setError("La fecha de nacimiento no es válida.");
                return false;
            }
        }

        return true;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        if (!validarFormulario()) return;

        try {
            if (isLogin) {
                const user = await login(email, contrasena);
                localStorage.setItem("usuario", JSON.stringify(user));
                navigate("/dashboard");
            } else {
                const usuario = {
                    nombre,
                    apellido,
                    email,
                    telefono,
                    fechaNacimiento,
                    passwordHash: contrasena,
                    esCliente: true
                };
                await register(usuario);
                alert("Registro exitoso. Ahora podés iniciar sesión.");
                setIsLogin(true);
                setEmail("");
                setContrasena("");
                setNombre("");
                setApellido("");
                setTelefono("");
                setFechaNacimiento("");
            }
        } catch (err) {
            setError("Error: " + (err.message || err));
        }
    };

    return (
        <div className="login-wrapper">
            <div className="login-box">
                <div className="toggle-buttons">
                    <button className={isLogin ? "active" : ""} onClick={() => { setIsLogin(true); setError(""); }}>
                        Iniciar sesión
                    </button>
                    <button className={!isLogin ? "active" : ""} onClick={() => { setIsLogin(false); setError(""); }}>
                        Registrarse
                    </button>
                </div>

                <h2>{isLogin ? "Iniciar sesión" : "Registrarse"}</h2>
                {error && <p className="error">{error}</p>}

                <form onSubmit={handleSubmit}>
                    {!isLogin && (
                        <>
                            <input
                                type="text"
                                placeholder="Nombre"
                                value={nombre}
                                onChange={(e) => setNombre(e.target.value)}
                                required
                            />
                            <input
                                type="text"
                                placeholder="Apellido"
                                value={apellido}
                                onChange={(e) => setApellido(e.target.value)}
                                required
                            />
                            <input
                                type="tel"
                                placeholder="Teléfono"
                                value={telefono}
                                onChange={(e) => setTelefono(e.target.value)}
                            />
                            <input
                                type="date"
                                placeholder="Fecha de Nacimiento"
                                value={fechaNacimiento}
                                onChange={(e) => setFechaNacimiento(e.target.value)}
                            />
                        </>
                    )}
                    <input
                        type="email"
                        placeholder="Correo electrónico"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                    <input
                        type="password"
                        placeholder="Contraseña"
                        value={contrasena}
                        onChange={(e) => setContrasena(e.target.value)}
                        required
                    />
                    <button type="submit">{isLogin ? "Ingresar" : "Registrarse"}</button>
                </form>
            </div>
        </div>
    );
};

export default Login;
