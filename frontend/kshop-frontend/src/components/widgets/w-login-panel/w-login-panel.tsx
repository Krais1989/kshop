import "./w-login-panel.sass";

import React, { useState } from "react";
import { toast } from "react-toastify";
import { AppServices } from "components/app/app-services";
import { HttpClient } from "services/clients/http/HttpClient";
import { AppSettings } from "components/app/app-settings";

interface IWLoginPanelProps {}

const WLoginPanel: React.FunctionComponent<IWLoginPanelProps> = (props) => {
    const [email, setLogin] = useState("");
    const [password, setPassword] = useState("");
    
    const http = new HttpClient();
    
    // const res:any = http.request<undefined, any>("GET", `${AppSettings.IdentityHost}/api/test/200`)
    //     .then(r => {
    //         if (r.IsSuccess()){
    //             toast.success(r.Data);
    //         } else{
    //             toast.error(r.ErrorMessage);
    //         }
    //     })
    //     .catch(err=>{
    //     });

    const submitCallback = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        console.log("Submit");

        AppServices.Clients.Identity.signIn({
            email: email,
            password: password,
        }).then((r) => {

            if (r.IsSuccess()) {
                toast.success(`Logged in: ${r.Data?.email}`);
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
