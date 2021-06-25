import { ProductDetails } from "models/ProductDetails";
import { ProductShort } from "models/ProductShort";
import { BaseResult, Result } from "../../BaseResult";


export class GetProductsForHomeRequest {
    page!: number;
}
export class GetProductsForHomeResponse extends BaseResult {
    Data!: Array<ProductShort>;
}

export class GetProductDetailsRequest {
    productId!:number;
}
export class GetProductDetailsResponse extends BaseResult {
    Data!: ProductDetails;
}


export interface IProductsClient {
    getProductsForHome(data: GetProductsForHomeRequest): Promise<GetProductsForHomeResponse>;
    getProductDetails(request: GetProductDetailsRequest): Promise<GetProductDetailsResponse>;
}
