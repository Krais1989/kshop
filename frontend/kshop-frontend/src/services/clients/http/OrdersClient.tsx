import { AppServices } from "components/app/app-services";
import { AppSettings } from "components/app/app-settings";
import {
    IOrdersClient,
    CreateOrderRequest,
    CreateOrderResponse,
    GetOrdersResponse,
    CancelOrderRequest,
    CancelOrderResponse,
    GetOrderDetailsRequest,
    GetOrderDetailsResponse,
} from "../abstractions/IOrdersClient";
import { HttpClient } from "./HttpClient";

export class OrdersClient implements IOrdersClient {
    private readonly http: HttpClient = new HttpClient();

    createOrder = async (data: CreateOrderRequest) =>
        await this.http.post<CreateOrderRequest, CreateOrderResponse>(
            `${AppSettings.OrdersHost}/api/orders`,
            data,
            AppServices.Auth.GetAuthHeader()
        );

    getOrders = async () =>
        await this.http.get<GetOrdersResponse>(
            `${AppSettings.OrdersHost}/api/orders`,
            AppServices.Auth.GetAuthHeader()
        );

    getOrderDetails = async (request: GetOrderDetailsRequest) =>
        await this.http.get<GetOrderDetailsResponse>(
            `${AppSettings.OrdersHost}/api/orders/details/${request.orderID}`,
            AppServices.Auth.GetAuthHeader()
        );

    cancelOrder = async (request: CancelOrderRequest) =>
        await this.http.post<CancelOrderRequest, CancelOrderResponse>(
            `${AppSettings.OrdersHost}/api/orders/cancel`,
            request,
            AppServices.Auth.GetAuthHeader()
        );
}
