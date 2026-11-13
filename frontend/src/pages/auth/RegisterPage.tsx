import { useNavigate } from "react-router-dom";
import { RegisterForm } from "../../components/auth/RegisterForm";
import { useAuth } from "../../hooks/auth/useAuthHook";
import type { AuthResponse } from "../../types/auth/AuthResponse";

export default function RegisterPage() {
    const navigate = useNavigate();
    const { login } = useAuth();

    return (
        <div className="grid min-h-screen place-items-center bg-gray-50 p-4">
            <RegisterForm
            loginHref="/login"
            onSuccess={(t: AuthResponse) => {
                login(t);
                navigate("/");
            }}
            />
        </div>
    );
}