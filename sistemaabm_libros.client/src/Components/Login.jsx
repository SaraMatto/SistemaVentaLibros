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
    const [tipoUsuario, setTipoUsuario] = useState("cliente"); // "cliente" o "admin"
    const navigate = useNavigate();

    const validarFormulario = () => {
        let isValid = true;
        let mensajeError = "";

        if (!email || !contrasena) {
            isValid = false;
            mensajeError = "El email y la contraseña son obligatorios.";
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (isValid && !emailRegex.test(email)) {
            isValid = false;
            mensajeError = "El correo electrónico no es válido.";
        }

        if (isValid && contrasena.length < 6) {
            isValid = false;
            mensajeError = "La contraseña debe tener al menos 6 caracteres.";
        }

        if (isValid && !isLogin) {
            if (!nombre || !apellido) {
                isValid = false;
                mensajeError = "Nombre y apellido son obligatorios.";
            }

            if (isValid && telefono && !/^\d{7,15}$/.test(telefono)) {
                isValid = false;
                mensajeError = "El teléfono debe contener solo números (7 a 15 dígitos).";
            }

            if (isValid && fechaNacimiento && isNaN(Date.parse(fechaNacimiento))) {
                isValid = false;
                mensajeError = "La fecha de nacimiento no es válida.";
            }
        }

        setError(mensajeError);
        return isValid;
    };

    const handleLogin = async () => {
        let errorMsg = "";
        try {
            const user = await login(email, contrasena);
            if (user && typeof user.esCliente !== "undefined") {
                if (tipoUsuario === "admin" && user.esCliente === false) {
                    localStorage.setItem("usuario", JSON.stringify(user));
                    navigate("/dashboard");
                } else if (tipoUsuario === "cliente" && user.esCliente === true) {
                    localStorage.setItem("usuario", JSON.stringify(user));
                    navigate("/dashboard");
                } else {
                    errorMsg = tipoUsuario === "admin"
                        ? "No tienes permisos de administrador."
                        : "No tienes permisos de cliente.";
                }
            } else {
                errorMsg = "Usuario o contraseña incorrectos.";
            }
        } catch (err) {
            setError("Error: " + (err.message || err));
        }
        setError(errorMsg);
    };

    const handleRegister = async () => {
        let errorMsg = "";
        try {
            const usuario = {
                Nombre: nombre,
                Apellido: apellido,
                Email: email,
                Telefono: telefono,
                FechaNacimiento: fechaNacimiento ? fechaNacimiento : null,
                PasswordHash: contrasena,
                EsCliente: tipoUsuario === "cliente"
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
            setTipoUsuario("cliente");
        } catch (error) {
            errorMsg = "Error en el registro: " + (error.message || error);
            setError(errorMsg);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        if (validarFormulario()) {
            if (isLogin) {
                await handleLogin();
            } else {
                await handleRegister();
            }
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
                            <div style={{ margin: "1rem 0" }}>
                                <label>
                                    Tipo de usuario:&nbsp;
                                    <select
                                        value={tipoUsuario}
                                        onChange={e => setTipoUsuario(e.target.value)}
                                        style={{ padding: "0.25rem", borderRadius: "5px" }}
                                    >
                                        <option value="cliente">Cliente</option>
                                        <option value="admin">Administrador</option>
                                    </select>
                                </label>
                            </div>
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
                    {isLogin && (
                        <div style={{ margin: "1rem 0" }}>
                            <label>
                                Tipo de usuario:&nbsp;
                                <select
                                    value={tipoUsuario}
                                    onChange={e => setTipoUsuario(e.target.value)}
                                    style={{ padding: "0.25rem", borderRadius: "5px" }}
                                >
                                    <option value="cliente">Cliente</option>
                                    <option value="admin">Administrador</option>
                                </select>
                            </label>
                        </div>
                    )}
                    <button type="submit">{isLogin ? "Ingresar" : "Registrarse"}</button>
                </form>
            </div>
        </div>
    );
};

export default Login;