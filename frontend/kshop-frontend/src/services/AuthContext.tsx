import { createContext, useState } from "react";

export class AuthData{
    userId?: string;
    token?:string;
    refreshToken?:string;
}

export const AuthContext = createContext<AuthData>({});
//export const useAuth = () => useContext(AuthContext);
export const useAuth = (authData: AuthData = {}) => useState<AuthData>(authData);