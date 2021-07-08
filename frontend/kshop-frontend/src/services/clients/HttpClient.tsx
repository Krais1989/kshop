import { toast } from "react-toastify";
import { BaseResult } from "services/BaseResult";

class CHttpClient {
    async request<TData, TResult extends BaseResult>(
        resp_type: { new (): TResult },
        method: string,
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
        //c: new()=>TResult
    ): Promise<TResult> {
        let req_cfg: RequestInit = {
            method: method,
            mode: "cors",
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
                const data = await resp.json();
                return Object.assign(new resp_type(), data) as TResult;
            } else if (resp.status === 400) {
                const result = new resp_type();
                return Object.assign(new resp_type(), {
                    isSuccess: false,
                    errorMessage: await resp.text(),
                }) as TResult;
            } else if (resp.status === 401) {
                localStorage.setItem("auth_err", "");
                return Object.assign(new resp_type(), {
                    isSuccess: false,
                    errorMessage: "Authorization request error",
                }) as TResult;
            } else {
                const err_msg = await resp.text();
                //toast.error(`Internal Server Error: ${err_msg}`);
                console.error(">>> Internal Server Error <<<");
                console.error(err_msg);
                return Object.assign(new resp_type(), {isSuccess: false, errorMessage: err_msg }) as TResult;
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
        resp_type: { new (): TResult },
        url: string,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<undefined, TResult>(resp_type, "GET", url, undefined, add_cfg);
    }

    async post<TData, TResult extends BaseResult>(
        resp_type: { new (): TResult },
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<TData, TResult>(resp_type, "POST", url, data, add_cfg);
    }

    async put<TData, TResult extends BaseResult>(
        resp_type: { new (): TResult },
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<TData, TResult>(resp_type, "PUT", url, data, add_cfg);
    }

    async delete<TData, TResult extends BaseResult>(
        resp_type: { new (): TResult },
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<TResult> {
        return await this.request<TData, TResult>(resp_type, "DELETE", url, data, add_cfg);
    }
}

export const HttpClient: CHttpClient = new CHttpClient();
