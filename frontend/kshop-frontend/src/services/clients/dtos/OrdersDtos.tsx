import { OrderDetails } from "models/Orders";
import { ProductQuantity } from "models/ProductQuantity";
import { BaseResult } from "services/BaseResult";


export class PaymentDto{
    type: string = "";
}
export class ShipmentDto{
    type: string = "";
}
export class AddressDto {
    data: string = "";
}
export class SubmitOrderRequest {
    address: AddressDto = new AddressDto();
    payment: PaymentDto = {type: ""};
    shipment: ShipmentDto = {type: ""};
    orderContent: Array<ProductQuantity> = [];
}
export class SubmitOrderResponse extends BaseResult {
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