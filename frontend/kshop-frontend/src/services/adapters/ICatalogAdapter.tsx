import { BaseResult } from "../BaseResult";

export class SearchProductsRequest {}
export class SearchProductsResult extends BaseResult {}

export class GetProductsRequest {}
export class GetProductsResult extends BaseResult {}

export class GetProductDetailsRequest {}
export class GetProductDetailsResult extends BaseResult {}

export interface ICatalogAdapter {
    searchProducts(request: SearchProductsRequest): Promise<SearchProductsResult>;
    getProducts(request: GetProductsRequest): Promise<GetProductsResult>;
    getProductDetails(request: GetProductDetailsRequest): Promise<GetProductDetailsResult>;
}
