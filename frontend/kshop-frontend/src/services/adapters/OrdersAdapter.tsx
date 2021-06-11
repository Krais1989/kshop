import { IHttpClient } from "../HttpClient";
import {
    CancelOrderRequest,
    CancelOrderResult,
    CreateOrderRequest,
    CreateOrderResult,
    GetOrdersRequest,
    GetOrdersResult,
    IOrdersAdapter,
} from "./IOrdersAdapter";

export class OrdersAdapter implements IOrdersAdapter {
    private readonly _host: string;
    private readonly _http: IHttpClient;

    constructor(host: string, http: IHttpClient) {
        this._host = host;
        this._http = http;
    }

    async createOrder(request: CreateOrderRequest): Promise<CreateOrderResult> {
        return new CreateOrderResult();
    }

    async getOrders(request: GetOrdersRequest): Promise<GetOrdersResult> {
        return new GetOrdersResult();
    }

    async cancelOrder(request: CancelOrderRequest): Promise<CancelOrderResult> {
        return new CancelOrderResult();
    }
}
