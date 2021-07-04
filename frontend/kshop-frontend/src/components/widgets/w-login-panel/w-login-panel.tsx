import "./w-login-panel.sass";

import React, { useState } from "react";
import { toast } from "react-toastify";
import { AuthData, useAuth } from "components/providers/AuthProvider";

interface IWLoginPanelProps {}

const WLoginPanel: React.FunctionComponent<IWLoginPanelProps> = (props) => {
    const [email, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const { auth, isAuthenticated, signOut, signIn } = useAuth();

    const submitSignIn = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        signIn({
            email: email,
            password: password,
        });
    };

    const signOutCallback = () => {
        console.log("SignOut");
        signOut();
    };

    const jsxSignIn = (
        <form onSubmit={(e) => submitSignIn(e)} className="kshop-w-login-panel">
            <input
                name="email"
                type="text"
                placeholder="Email address"
                value={email}
                onChange={(e) => setLogin(e.target.value)}
            />
            <input
                name="password"
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button className="kshop-button-blue" type="submit">
                Login
            </button>
        </form>
    );

    const jsxSignOut = (
        <div>
            {auth?.userId} &nbsp;
            <button
                className={"kshop-button-yellow"}
                onClick={(e) => signOutCallback()}
            >
                SignOut
            </button>
        </div>
    );

    const render = !isAuthenticated() ? jsxSignIn : jsxSignOut;

    return render;
};

export default WLoginPanel;
