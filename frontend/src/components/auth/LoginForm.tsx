import { useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/auth/useAuthHook";
import { useId, useState } from "react";
import type { IAuthAPIService } from "../../api/interfaces/IAuthApiService";

export default function LoginForm({authApi}: {authApi: IAuthAPIService}) {
    const navigate = useNavigate();
    const { login } = useAuth();
    
    const usernameKey = useId();
    const passwordKey = useId();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [formError, setFormError] = useState("");
    const [loading, setLoading] = useState(false);

    const onSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
        e.preventDefault(); setFormError(""); setLoading(true);

        //TODO: validate input
        var res = await authApi.login({username, password})
        setLoading(false);
        res.success ? login(res.data!) : setFormError(res.message ?? "Invalid creditentials.");

        navigate("/");
    };

    return (
        <div className="text-center bg-gray-800 shadow-md shadow-indigo-500/40 rounded-md px-2 py-4">
            <div className="">
                <header className=" font-semibold text-3xl text-blue-400/50">Login</header>
                <form onSubmit={onSubmit} className="flex flex-col gap-0.5">
                    <div className="px-2 m-2">
                        <div className="shadow-xl">
                            <label className="block text-left text-sm text-cyan-700 font-semibold px-2" 
                                htmlFor={usernameKey}>Username</label>
                            <input className="w-full text-green-300 border-2 border-blue-400/40 rounded-md focus:outline-0 focus:border-indigo-500 transition-colors py-1 px-2"
                                key={usernameKey}
                                type="text"
                                value={username}
                                onChange={e => setUsername(e.target.value)}/>
                        </div>
                    </div>
                    <div className="px-2 m-2">
                        <div className="shadow-xl">
                            <label className="block text-left text-sm text-cyan-700 font-semibold px-2" 
                                htmlFor={passwordKey}>Password</label>
                            <input className="w-full text-green-300 border-2 border-blue-400/40 rounded-md focus:outline-0 focus:border-indigo-500 transition-colors py-1 px-2"
                                key={passwordKey}
                                type="password"
                                value={password}
                                onChange={e => setPassword(e.target.value)}/>
                        </div>
                    </div>
                    {formError && 
                        <div className="">
                            {formError}
                        </div>
                    }
                    <div className="shadow-xl">
                        <button className="w-full font-semibold text-sm border-0 text-cyan-600 shrink-0 bg-blue-400/50 hover:bg-blue-300/50 disabled:opacity-50 disabled:cursor-not-allowed rounded-2xl px-2 py-1 mt-2 transition-colors"
                            type="submit" 
                            aria-busy={loading}
                            disabled={loading}>{loading ? "Singing in..." : "Sing in"}</button>
                    </div>
                </form>
            </div>
        </div>
    );
}