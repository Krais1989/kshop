import {
    IOrdersClient,
    CreateOrderRequest,
    CreateOrderResponse,
    GetOrdersRequest,
    GetOrdersResponse,
    CancelOrderRequest,
    CancelOrderResponse,
} from "../abstractions/IOrdersClient";
import { HttpClient } from "./HttpClient";

export class OrdersClient implements IOrdersClient {
    private readonly http: HttpClient = new HttpClient();

    // constructor(host: string, http: IHttpClient) {
    //     this._host = host;
    //     this._http = http;
    // }

    authData = () => {
        const token = "";
        let cfg: RequestInit = { headers: { "Authorization": `Bearer ${token}` } };
        return cfg;
    };

    createOrder = async (data: CreateOrderRequest) =>
        await this.http.post<CreateOrderRequest, CreateOrderResponse>(``, data, this.authData());

    getOrders = async (request: GetOrdersRequest) =>
        await this.http.post<GetOrdersRequest, GetOrdersResponse>(``, this.authData());

    cancelOrder = async (request: CancelOrderRequest) =>
        await this.http.post<CancelOrderRequest, CancelOrderResponse>(``, this.authData());
}
