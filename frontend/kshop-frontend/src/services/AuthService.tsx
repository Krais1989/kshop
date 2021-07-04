import { AuthData } from "../components/providers/AuthProvider";


class CAuthService {
    public getToken() {
        const authRaw = localStorage.getItem("auth");
        if (!authRaw || authRaw === "")
            return undefined;
        const auth = JSON.parse(authRaw) as AuthData;
        return auth.token;
    }

    public getAuthHeader() {
        return {
            headers: {
                Authorization: `Bearer ${this.getToken()}`,
            },
        };
    }
}
// Выведен в отдельный класс, тк впихивать в IdentityClient не вариант - это для работы с Api, 
// а в AuthProvider нельзя, тк используется не компонентными классами
export const AuthService: CAuthService = new CAuthService();
