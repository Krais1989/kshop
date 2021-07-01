import { OrderDetails } from "models/Orders";
import { ProductQuantity } from "models/ProductQuantity";
import { BaseResult } from "services/BaseResult";


export class CreateOrderRequest {
    address: string = "";
    paymentProvider: number = -1;
    shippingMethod: number = -1;
    orderContent: Array<ProductQuantity> = [];
}
export class CreateOrderResponse extends BaseResult {
    orderID: string | undefined;
}

export class GetOrderDetailsRequest {
    orderID: string = "";
}
export class GetOrderDetailsResponse extends BaseResult {
    details: OrderDetails | undefined;
}

export class GetOrdersResponse extends BaseResult {
    orders: Array<OrderDetails> = [];
}

export class CancelOrderRequest {
    orderID: string = "";
}
export class CancelOrderResponse extends BaseResult {}

export interface IOrdersClient {
    createOrder(request: CreateOrderRequest): Promise<CreateOrderResponse>;
    getOrders(): Promise<GetOrdersResponse>;
    getOrderDetails(
        request: GetOrderDetailsRequest
    ): Promise<GetOrderDetailsResponse>;
    cancelOrder(request: CancelOrderRequest): Promise<CancelOrderResponse>;
}
