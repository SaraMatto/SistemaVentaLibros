// LOGIN: Solo envía email y password
export async function login(email, password) {
    const res = await fetch("https://localhost:44359/api/Usuario/login", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password })
    });

    if (!res.ok) throw new Error("Credenciales inválidas");
    return await res.json();
}

// REGISTER: Envía todo el objeto usuario
export async function register(usuario) {
    const res = await fetch("https://localhost:44359/api/Usuario/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(usuario)
    });

    if (!res.ok) throw new Error("Error al registrar");
    return await res.json();
}

