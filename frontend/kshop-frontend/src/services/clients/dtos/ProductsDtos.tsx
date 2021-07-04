import { ProductDetails } from "models/ProductDetails";
import { ProductPresentation } from "models/ProductPresentation";
import { BaseResult } from "../../BaseResult";


export class GetProductsForHomeRequest {
    pageIndex: number = 0;
}
export class GetProductsForHomeResponse extends BaseResult {
    data: Array<ProductPresentation> = [];
}

export class GetProductDetailsRequest {
    productID:Array<number> = [];
}
export class GetProductDetailsResponse extends BaseResult {
    data: Array<ProductDetails> = [];
}

export class GetProductsForOrderRequest {
    productIDs:Array<number> = [];
}
export class GetProductsForOrderResponse extends BaseResult {
    data: Array<ProductPresentation> = [];
}