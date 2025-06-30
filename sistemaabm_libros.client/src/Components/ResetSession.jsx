// src/Components/ResetSession.jsx
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const ResetSession = () => {
    const navigate = useNavigate();

    useEffect(() => {
        localStorage.removeItem("usuario");
        navigate("/login", { replace: true });
    }, [navigate]);

    return null;
};

export default ResetSession;
