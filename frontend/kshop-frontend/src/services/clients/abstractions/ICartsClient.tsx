import { Cart, CartPosition } from "models/Cart";
import { BaseResult } from "services/BaseResult";

export class GetCartResponse extends BaseResult {
    data: Cart = new Cart();
}

export class SetCartPositionsRequest {
    positions: Array<CartPosition> = [];
}
export class SetCartPositionResponse extends BaseResult {}

export class RemoveCartPositionRequest {
    productIDs: Array<number> = [];
}
export class RemoveCartPositionResponse extends BaseResult {}

export class ClearCartPositionResponse extends BaseResult {}

export interface ICartsClient {
    current(): Promise<GetCartResponse>;
    setPositions(request: SetCartPositionsRequest): Promise<SetCartPositionResponse>;
    removePosition(request: RemoveCartPositionRequest): Promise<RemoveCartPositionResponse>;
    clear(): Promise<ClearCartPositionResponse>;
}
