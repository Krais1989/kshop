import React, { createContext, useContext, useState } from "react";
import { toast } from "react-toastify";
import { SignInRequest, SignUpRequest } from "services/clients/dtos/IdentityDtos";
import { IdentityClient } from "services/clients/IdentityClient";

export class AuthData {
    userId: string = "";
    token: string = "";
    refreshToken: string = "";
}

export class AuthContextData {
    auth: AuthData|null = null;
    readonly isAuthenticated: () => boolean = () => false;
    readonly signOut: () => void = () => {};
    readonly signIn: (request: SignInRequest) => Promise<void> = async (request) => {};
    readonly signUp: (request: SignUpRequest) => Promise<void> = async (request) => {};
}

const AuthContext = createContext<AuthContextData>(new AuthContextData());

interface IProps {}

export const AuthProvider: React.FunctionComponent<IProps> = (props) => {

    const getLocal = ():AuthData|null =>{
        const rawAuth = localStorage.getItem("auth");
        if (rawAuth === null) return null;
        const authData: AuthData = Object.assign(new AuthData(), JSON.parse(rawAuth));
        return authData;
    }

    const [auth, setAuth] = useState<AuthData | null>(getLocal());

    const set = (a: AuthData | null) => {
        if (a === null) 
            localStorage.removeItem("auth");
        else 
            localStorage.setItem("auth", JSON.stringify(a));
        setAuth(a);
    };



    const signOut = () => {
        set(null);
    };

    const signIn = async (request: SignInRequest): Promise<void> =>
        await IdentityClient.signIn(request).then((r) => {
            if (r.isSuccess) {
                toast.success("User logged in");
                set({ userId: r.email, token: r.token, refreshToken: r.refreshToken });
            } else {
                set(null);
            }
        });

    const signUp = async (request: SignUpRequest): Promise<void> =>
        await IdentityClient.signUp(request).then((r) => {
            if (r.isSuccess) {
                toast.success("User successfuly registered");
            } else {
                toast.error("User registration error");
            }
        });


    return (
        <AuthContext.Provider
            value={{
                auth: auth,
                isAuthenticated: () => auth !== null,
                signOut: signOut,
                signIn: signIn,
                signUp: signUp,
            }}
        >
            {props.children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
