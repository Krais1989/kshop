import { Cart, CartPosition } from "models/Cart";
import { BaseResult } from "services/BaseResult";

export class GetCartResponse extends BaseResult {
    data: Cart = new Cart();
}

export class SetCartPositionsRequest {
    positions: Array<CartPosition> = [];
    useMerge: boolean = false;
}
export class SetCartPositionResponse extends BaseResult {}

export class RemoveCartPositionRequest {
    productsIDs: Array<number> = [];
}
export class RemoveCartPositionResponse extends BaseResult {}

export class ClearCartPositionResponse extends BaseResult {}