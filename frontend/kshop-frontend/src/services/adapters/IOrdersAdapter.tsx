import { Order } from "../../models/Order";
import { BaseResult } from "../BaseResult";

export class CreateOrderRequest {}
export class CreateOrderResult extends BaseResult {
    OrderID: string | undefined;
}

export class GetOrdersRequest {}
export class GetOrdersResult extends BaseResult {
    Orders: Array<Order> | undefined;
}

export class CancelOrderRequest {}
export class CancelOrderResult extends BaseResult{}

export interface IOrdersAdapter {
    createOrder(request: CreateOrderRequest): Promise<CreateOrderResult>;
    getOrders(request: GetOrdersRequest): Promise<GetOrdersResult>;
    cancelOrder(request: CancelOrderRequest): Promise<CancelOrderResult>;
}
