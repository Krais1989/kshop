import { AppSettings } from "components/app/AppSettings";
import { AuthService } from "services/AuthService";
import {
    SubmitOrderRequest,
    SubmitOrderResponse,
    GetOrdersResponse,
    GetOrderDetailsRequest,
    GetOrderDetailsResponse,
    CancelOrderRequest,
    CancelOrderResponse,
} from "./dtos/OrdersDtos";
import { HttpClient } from "./HttpClient";

class COrdersClient {
    submitOrder = async (data: SubmitOrderRequest) =>
        await HttpClient.post<SubmitOrderRequest, SubmitOrderResponse>(
            SubmitOrderResponse,
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
