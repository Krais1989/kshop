import "./w-login-panel.sass";

import React, { useState } from "react";
import { toast } from "react-toastify";

interface IWLoginPanelProps {}

const WLoginPanel: React.FunctionComponent<IWLoginPanelProps> = (props) => {
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const submitCallback = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        console.log("Submit");

        if (login !== "" && password !== "") {
            toast.success("Loggin Submited!", {
                hideProgressBar: true,
                autoClose: 2000,
                pauseOnHover: false
            });
        } else {
            toast.warning("Email or password is empty!", {
                hideProgressBar: true,
                autoClose: 3000,
                pauseOnHover: false,
            });
        }
    };
    return (
        <form onSubmit={submitCallback} className="kshop-w-login-panel">
            <input
                type="text"
                placeholder="Email address"
                value={login}
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
