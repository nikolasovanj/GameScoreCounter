import { useNavigate } from "react-router-dom";
import type { IAuthAPIService } from "../../api/interfaces/IAuthApiService";
import { useAuth } from "../../hooks/auth/useAuthHook";
import { useId, useState } from "react";

export default function RegisterForm({authApi}: {authApi: IAuthAPIService}) {
    const navigate = useNavigate();
    const { login } = useAuth();

    const usernameKey = useId();
    const passwordKey = useId();
    const passwordRepeatKey = useId();
    const emailKey = useId();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [passwordRepeat, setPasswordRepeat] = useState("");
    const [email, setEmail] = useState("");

    const [formError, setFormError] = useState("");
    const [loading, setLoading] = useState(false);

    const onSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
        e.preventDefault(); setFormError(""); setLoading(true);

        //TODO: validate input
        var res = await authApi.register({username, password, passwordRepeat});
        setLoading(false);
        (res.success && res.data) ? login(res.data) : setFormError(res.message ?? "Invalid creditentials.");

        navigate("/");
    };

    return (
        <div>
            <div>
                <header>Register</header>
                <form onSubmit={onSubmit}>
                    <div>
                        <div>
                            <label>Username</label>
                            <input 
                                key={usernameKey}
                                required
                                type="text"
                                value={username}
                                onChange={e => setUsername(e.target.value)}/>
                        </div>
                    </div>
                    <div>
                        <div>
                            <label>Email</label>
                            <input 
                                key={emailKey}
                                type="text"
                                value={email}
                                onChange={e => setUsername(e.target.value)}/>
                        </div>
                    </div>
                    <div>
                        <div>
                            <label>Password</label>
                            <input 
                                key={passwordKey}
                                required
                                type="password"
                                value={password}
                                onChange={e => setPassword(e.target.value)}/>
                        </div>
                    </div>
                    <div>
                        <div>
                            <label>Repeat Password</label>
                            <input 
                                key={passwordRepeatKey}
                                required
                                type="password"
                                value={passwordRepeat}
                                onChange={e => setPasswordRepeat(e.target.value)}/>
                        </div>
                    </div>
                    {formError && 
                        <div>
                            {formError}
                        </div>
                    }
                    <div>
                        <button 
                            type="submit"
                            aria-busy={loading}
                            disabled={loading}>{loading ? "Working...": "Register"}</button>
                    </div>
                </form>
            </div>
        </div>
    );
};