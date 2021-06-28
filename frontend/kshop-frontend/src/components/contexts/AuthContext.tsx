import React, { createContext, useContext, useState } from "react";

export class AuthData {
    userId?: string;
    token?: string;
    refreshToken?: string;
}

export class AuthContextData {
    auth!: AuthData;
    setAuth!: React.Dispatch<React.SetStateAction<AuthData>>;
    isAuthenticated!: () => boolean;
    signOut!: () => void;
}

const AuthContext = createContext<AuthContextData>({
    auth: {},
    setAuth: () => {},
    isAuthenticated: () => false,
    signOut: () =>{}
});

interface IProps {}

export const AuthProvider: React.FunctionComponent<IProps> = (props) => {
    const rawAuth = localStorage.getItem("auth");
    const authData: AuthData =
        rawAuth !== null
            ? Object.assign(new AuthData(), JSON.parse(rawAuth))
            : new AuthData();

    const [auth, setAuth] = useState<AuthData>(authData);

    const loadAuth = () => {
        const rawAuth = localStorage.getItem("auth");
        const newAuth: AuthData =
        rawAuth !== null
            ? Object.assign(new AuthData(), JSON.parse(rawAuth))
            : new AuthData();
        setAuth(newAuth);
    };

    const signOut = () => {
        localStorage.removeItem("auth");
        loadAuth();
    }

    return (
        <AuthContext.Provider
            value={{
                auth: auth,
                setAuth: setAuth,
                isAuthenticated: () => auth !== null && auth.token !== null&& auth.token !== undefined,
                signOut: () => signOut()
            }}
        >
            {props.children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
