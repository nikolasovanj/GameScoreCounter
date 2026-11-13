import { useNavigate } from "react-router-dom";
import { LoginForm } from "../../components/auth/LoginForm";
import { useAuth } from "../../hooks/auth/useAuthHook";
import type { AuthResponse } from "../../types/auth/AuthResponse";

export default function LoginPage() {
    const { login } = useAuth();
    const navigate = useNavigate();

    return (
        <div className="grid min-h-screen place-items-center bg-gray-50 p-4">
            <LoginForm
            registerHref="/register"
            onSuccess={(t: AuthResponse) => {
                login(t);
                navigate("/");
            }}
            />
        </div>
    );
}