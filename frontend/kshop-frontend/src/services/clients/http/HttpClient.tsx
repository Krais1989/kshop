import { AppSettings, useAppSettings } from "components/app/app-settings";
import { toast } from "react-toastify";
import { BaseResult, Result } from "../../BaseResult";

export class HttpClient {
    async request<TData, TResult>(
        method: string,
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<Result<TResult>> {
        let req_cfg: RequestInit = {
            method: method,
            mode: "cors",
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

        // if (add_cfg) {
        //     req_cfg = { ...req_cfg, ...add_cfg }; //Object.assign(req_cfg, add_cfg);
        //     req_cfg.headers = { ...req_cfg.headers, ...add_cfg?.headers };
        // }

        console.log(`HTTP Request ${method}: ${url}\n${data}`);

        const resp = await fetch(url, req_cfg);

        if (resp.ok || resp.status === 400)
        {
            return new Result<TResult>((await resp.json()) as TResult);
        } else {
            return new Result<TResult>(undefined, await resp.text());
        }

        //return result;
        // .then(async (response) => {

        //     if (response.ok)
        //         toast.success("Http response success");
        //     return await response.json() as TResult);
        // })
        // .catch((err) => {
        //     console.error("Http response error: ");
        //     console.error(err);
        //     //toast.error(`Http response error: ${err.text()}`);
        // });
    }

    async get<TResult>(
        url: string,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<Result<TResult>> {
        return await this.request<undefined, TResult>(
            "GET",
            url,
            undefined,
            add_cfg
        );
    }

    async post<TData, TResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<Result<TResult>> {
        return await this.request<TData, TResult>("POST", url, data, add_cfg);
    }

    async put<TData, TResult>(
        url: string,
        data: TData,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<Result<TResult>> {
        return await this.request<TData, TResult>("PUT", url, data, add_cfg);
    }

    async delete<TData, TResult>(
        url: string,
        data: TData | undefined = undefined,
        add_cfg: RequestInit | undefined = undefined
    ): Promise<Result<TResult>> {
        return await this.request<TData, TResult>("DELETE", url, data, add_cfg);
    }
}
