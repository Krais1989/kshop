import React, { createContext, useContext } from "react";
import { useHistory } from "react-router-dom";
import { toast } from "react-toastify";
import { BaseResult } from "services/BaseResult";
import { useRedirect } from "./RedirectProvider";

interface IHttpProviderService {

    request<TData, TResult extends BaseResult>(
        method: string,
        url: string,
        data: TData | undefined,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

    get<TResult extends BaseResult>(
        url: string,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

    post<TData, TResult extends BaseResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

    put<TData, TResult extends BaseResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

    delete<TData, TResult extends BaseResult>(
        url: string,
        data: TData | undefined,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;
}

class HttpContextData {
    async request<TData, TResult extends BaseResult>(
        method: string,
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        let req_cfg: RequestInit = {
            method: method,
            // mode: "cors",
            // cache: "no-cache",
            // credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                //"Authorization": "Bearer <token>"
            },
            // redirect: "follow",
            // referrerPolicy: "no-referrer",
        };

        if (data) {
            req_cfg.body = JSON.stringify(data);
        }

        if (add_cfg) {
            const hhh = Object.assign({}, req_cfg.headers);

            req_cfg = Object.assign(req_cfg, add_cfg); //Object.assign(req_cfg, add_cfg);
            req_cfg.headers = Object.assign(hhh, add_cfg?.headers);
        }

        console.log(`HTTP Request ${method}: ${url}\n${data}`);

        try {
            const resp = await fetch(url, req_cfg);
            if (resp.ok || resp.status === 400) {
                return (await resp.json()) as TResult;
            } else if (resp.status === 400) {
                return { errorMessage: await resp.text() } as TResult;
            } else if (resp.status === 401) {
                return { errorMessage: await resp.text() } as TResult;
            } else {
                //TODO: asdasd
                toast.error(`Internal Server Error: ${await resp.text()}`);

                return (await resp.json()) as TResult;
            }
        } catch (err) {
            toast.error(`HttpClient request error:\n${err.message}`);
            //toast.error(e.Message);
            console.error(`HttpClient request error:`);
            console.error(err);
            throw err;
        }
    }

    async get<TResult extends BaseResult>(
        url: string,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<undefined, TResult>("GET", url, undefined, add_cfg);
    }

    async post<TData, TResult extends BaseResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<TData, TResult>("POST", url, data, add_cfg);
    }

    async put<TData, TResult extends BaseResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<TData, TResult>("PUT", url, data, add_cfg);
    }

    async delete<TData, TResult extends BaseResult>(
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<TData, TResult>("DELETE", url, data, add_cfg);
    }
}

export const HttpContext = createContext<IHttpProviderService | null>(null);
export const useHttp = () => useContext(HttpContext);

export const HttpProvider: React.FC = (props) => {
    const default_data = new HttpContextData();

    return <HttpContext.Provider value={default_data}>{props.children}</HttpContext.Provider>;
};
