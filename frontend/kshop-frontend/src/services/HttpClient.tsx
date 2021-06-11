import { BaseResult } from "./BaseResult";

export interface IHttpClient {

    request<TData, TResult>(
        url: string,
        method: string,
        data: TData | undefined,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

    get<TResult>(url: string, add_cfg: RequestInit | undefined): Promise<TResult>;

    post<TData, TResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

    put<TData, TResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

    delete<TData, TResult>(
        url: string,
        data: TData | undefined,
        add_cfg: RequestInit | undefined
    ): Promise<TResult>;

}

export class HttpClient implements IHttpClient {
    async request<TData, TResult>(
        method: string,
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        let req_cfg: RequestInit = {
            method: method,
            mode: "no-cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                //"Authorization": "Bearer <token>"
            },
            redirect: "follow",
            referrerPolicy: "no-referrer",
        };

        if (data) {
            req_cfg.body = JSON.stringify(data);
        }

        if (add_cfg) {
            req_cfg = { ...req_cfg, ...add_cfg }; //Object.assign(req_cfg, add_cfg);
            req_cfg.headers = { ...req_cfg.headers, ...add_cfg?.headers };
        }

        const response = await fetch(url, req_cfg);
        if (response.ok){

        }
        const result = (await response.json()) as TResult;
        return result;
    }

    async get<TResult>(
        url: string,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request("GET", url, undefined, add_cfg);
    }

    async post<TData, TResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request("POST", url, data, add_cfg);
    }

    async put<TData, TResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request("PUT", url, data, add_cfg);
    }

    async delete<TData, TResult>(
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request("DELETE", url, data, add_cfg);
    }
}
