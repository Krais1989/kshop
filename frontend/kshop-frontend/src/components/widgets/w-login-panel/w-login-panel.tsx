import "./w-login-panel.sass";

import React, { useState } from "react";
import { toast } from "react-toastify";
import { AuthData, useAuth } from "components/providers/AuthProvider";
import Submitter from "components/controls/submitter/Submitter";

interface IWLoginPanelProps {}

const WLoginPanel: React.FunctionComponent<IWLoginPanelProps> = (props) => {
    const [email, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const { auth, isAuthenticated, signOut, signIn } = useAuth();

    const submitSignIn = () => {
        return signIn({
            email: email,
            password: password,
        });
    };

    const signOutCallback = () => {
        console.log("SignOut");
        signOut();
    };

    const jsxSignIn = (
        <div className="kshop-w-login-panel">
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
            <Submitter submit={() => submitSignIn()}>
                <button className="kshop-button-blue" type="submit">
                    Login
                </button>
            </Submitter>
        </div>
    );

    const jsxSignOut = (
        <div>
            {auth?.userId} &nbsp;
            <button className={"kshop-button-yellow"} onClick={(e) => signOutCallback()}>
                SignOut
            </button>
        </div>
    );

    return !isAuthenticated() ? jsxSignIn : jsxSignOut;
};

export default WLoginPanel;
