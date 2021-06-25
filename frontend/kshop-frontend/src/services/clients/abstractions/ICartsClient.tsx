import { BaseResult, Result } from "services/BaseResult";

export class GetCartResponse extends BaseResult {}

export class SetCartPositionRequest {
    productId!: number;
    quantity!: number;
}
export class SetCartPositionResponse extends BaseResult {}

export class RemoveCartPositionRequest {
    productId!: number;
}
export class RemoveCartPositionResponse extends BaseResult {}

export class ClearCartPositionResponse extends BaseResult {}

export interface ICartsClient {
    get(): Promise<Result<GetCartResponse>>;
    setPosition(request: SetCartPositionRequest): Promise<Result<SetCartPositionResponse>>;
    removePosition(request: RemoveCartPositionRequest): Promise<Result<RemoveCartPositionResponse>>;
    clear(): Promise<Result<ClearCartPositionResponse>>;
}
