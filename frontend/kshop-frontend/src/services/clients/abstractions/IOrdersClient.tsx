import { OrderDetails } from "models/Orders";
import { BaseResult, Result } from "services/BaseResult";

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
    createOrder(request: CreateOrderRequest): Promise<Result<CreateOrderResponse>>;
    getOrders(request: GetOrdersRequest): Promise<Result<GetOrdersResponse>>;
    cancelOrder(request: CancelOrderRequest): Promise<Result<CancelOrderResponse>>;
}
