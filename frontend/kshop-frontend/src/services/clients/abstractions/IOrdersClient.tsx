import { OrderDetails } from "models/Orders";
import { BaseResult } from "services/BaseResult";

export class CreateOrderRequest {}
export class CreateOrderResponse extends BaseResult {
    OrderID: string | undefined;
}

export class GetOrdersRequest {}
export class GetOrdersResponse extends BaseResult {
    Orders: Array<OrderDetails> | undefined;
}

export class CancelOrderRequest {}
export class CancelOrderResponse extends BaseResult{}

export interface IOrdersClient {
    createOrder(request: CreateOrderRequest): Promise<CreateOrderResponse>;
    getOrders(request: GetOrdersRequest): Promise<GetOrdersResponse>;
    cancelOrder(request: CancelOrderRequest): Promise<CancelOrderResponse>;
}
