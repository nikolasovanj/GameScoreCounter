import { authApi } from "../../api/services/AuthApiService";
import RegisterForm from "../../components/auth/RegisterForm";


export default function RegisterPage() {

    return (
        <div className="">
           <RegisterForm authApi={authApi}/>
        </div>
    );
}