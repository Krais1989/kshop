import { AppSettings } from "components/app/AppSettings";
import { AuthService } from "services/AuthService";
import {
    CreateOrderRequest,
    CreateOrderResponse,
    GetOrdersResponse,
    GetOrderDetailsRequest,
    GetOrderDetailsResponse,
    CancelOrderRequest,
    CancelOrderResponse,
} from "./dtos/OrdersDtos";
import { HttpClient } from "./HttpClient";

class COrdersClient {
    createOrder = async (data: CreateOrderRequest) =>
        await HttpClient.post<CreateOrderRequest, CreateOrderResponse>(
            CreateOrderResponse,
            `${AppSettings.OrdersHost}/api/orders`,
            data,
            AuthService.getAuthHeader()
        );

    getOrders = async () => 
        await HttpClient.get<GetOrdersResponse>(
            GetOrdersResponse,
            `${AppSettings.OrdersHost}/api/orders`,
            AuthService.getAuthHeader()
        );

    getOrderDetails = async (request: GetOrderDetailsRequest) =>
        await HttpClient.get<GetOrderDetailsResponse>(
            GetOrderDetailsResponse,
            `${AppSettings.OrdersHost}/api/orders/details/${request.orderID}`,
            AuthService.getAuthHeader()
        );

    cancelOrder = async (request: CancelOrderRequest) =>
        await HttpClient.post<CancelOrderRequest, CancelOrderResponse>(
            CancelOrderResponse,
            `${AppSettings.OrdersHost}/api/orders/cancel`,
            request,
            AuthService.getAuthHeader()
        );
}

export const OrdersClient: COrdersClient = new COrdersClient();
