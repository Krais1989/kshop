import { ProductDetails } from "models/ProductDetails";
import { ProductShort } from "models/ProductShort";
import { BaseResult } from "../../BaseResult";


export class GetProductsForHomeRequest {
    page!: number;
}
export class GetProductsForHomeResponse extends BaseResult {
    data: Array<ProductShort> = [];
}

export class GetProductDetailsRequest {
    productID!:number;
}
export class GetProductDetailsResponse extends BaseResult {
    data!: ProductDetails;
}


export interface IProductsClient {
    getProductsForHome(data: GetProductsForHomeRequest): Promise<GetProductsForHomeResponse>;
    getProductDetails(request: GetProductDetailsRequest): Promise<GetProductDetailsResponse>;
}
