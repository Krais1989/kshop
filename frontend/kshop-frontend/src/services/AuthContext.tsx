import { createContext, useContext } from "react";

export class AuthContextType {
    token?: string;
    userId?: string;
}

export const AuthContext = createContext<AuthContextType>({});
export const useAuth = () => useContext(AuthContext);