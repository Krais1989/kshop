import "./w-login-panel.sass";

import React, { useRef, useState } from "react";
import { toast } from "react-toastify";
import { AppServices } from "components/app/app-services";
import { AuthData, useAuth } from "components/contexts/AuthContext";
import {
    SignInRequest,
    SignInResponse,
} from "services/clients/abstractions/IIdentityClient";
import { AppSettings } from "components/app/app-settings";
import { AuthService } from "services/AuthService";

interface IWLoginPanelProps {}

const WLoginPanel: React.FunctionComponent<IWLoginPanelProps> = (props) => {
    const [email, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const { auth, setAuth, isAuthenticated, signOut } = useAuth();

    const submitSignIn = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        AppServices.Clients.Identity.signIn({
            email: email,
            password: password,
        }).then((r) => {
            if (!r.ErrorMessage) {
                const auth: AuthData = {
                    userId: r.email,
                    token: r.token,
                    refreshToken: ""
                };

                localStorage.setItem("auth", JSON.stringify(auth));
                setAuth(auth);
            } else {
                console.log(r.ErrorMessage);
                toast.error(`Logging fail: ${r.ErrorMessage}`);
                setAuth(new AuthData());
            }
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
            {auth.userId} &nbsp;
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
