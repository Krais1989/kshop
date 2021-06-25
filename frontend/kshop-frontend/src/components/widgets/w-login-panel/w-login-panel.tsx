import "./w-login-panel.sass";

import React, { useState } from "react";
import { toast } from "react-toastify";
import { AppServices } from "components/app/app-services";
import { HttpClient } from "services/clients/http/HttpClient";
import { AppSettings } from "components/app/app-settings";
import { useAuth } from "services/AuthContext";

interface IWLoginPanelProps {}

const WLoginPanel: React.FunctionComponent<IWLoginPanelProps> = (props) => {
    const [email, setLogin] = useState("");
    const [password, setPassword] = useState("");
        
    const submitCallback = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        console.log("Submit");

        AppServices.Clients.Identity.signIn({
            email: email,
            password: password,
        }).then((r) => {

            if (!r.ErrorMessage) {
                toast.success(`Logged in: ${r.email}`);

            } else {
                console.log(r.ErrorMessage);
                toast.error(`Logging fail: ${r.ErrorMessage}`);
            }
        });
    };

    return (
        <form onSubmit={submitCallback} className="kshop-w-login-panel">
            <input
                type="text"
                placeholder="Email address"
                value={email}
                onChange={(e) => setLogin(e.target.value)}
            />
            <input
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
};

export default WLoginPanel;
