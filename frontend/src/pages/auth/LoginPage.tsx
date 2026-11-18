import { authApi } from "../../api/services/AuthApiService";
import LoginForm from "../../components/auth/LoginForm";

export default function LoginPage() {
    
    return (
        <main className="flex flex-col items-center bg-gray-600 w-full h-screen">
            <div className="px-4 py-4">
                <LoginForm authApi={authApi}/>
            </div>
        </main>
    );
}