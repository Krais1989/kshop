import { AuthData } from "components/contexts/AuthContext";

/* Использовать вне React-компонентов, для чтения */
export interface IAuthService {
    GetToken: () => string | undefined;
    GetAuthHeader: () => RequestInit;
}

export class AuthService implements IAuthService {
    GetToken: () => string | undefined = () => {
        const authRaw = localStorage.getItem("auth");
        if (!authRaw || authRaw === "") return undefined;
        const auth = JSON.parse(authRaw) as AuthData;
        return auth.token;
    };

    GetAuthHeader: () => RequestInit = () => {
        return {
            headers: {
                Authorization: `Bearer ${this.GetToken()}`,
            },
        };
    };
}