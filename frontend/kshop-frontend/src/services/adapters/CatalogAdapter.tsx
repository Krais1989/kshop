import { IHttpClient } from "../HttpClient";
import {
    GetProductDetailsRequest,
    GetProductDetailsResult,
    GetProductsRequest,
    GetProductsResult,
    ICatalogAdapter,
    SearchProductsRequest,
    SearchProductsResult
} from "./ICatalogAdapter";

export class CatalogAdapter implements ICatalogAdapter {
    private readonly _host: string;
    private readonly _http: IHttpClient;

    constructor(host: string, http: IHttpClient) {
        this._host = host;
        this._http = http;
    }
    async searchProducts(
        request: SearchProductsRequest
    ): Promise<SearchProductsResult> {
        return new SearchProductsResult();
    }
    async getProducts(request: GetProductsRequest): Promise<GetProductsResult> {
        return new GetProductsResult();
    }
    async getProductDetails(
        request: GetProductDetailsRequest
    ): Promise<GetProductDetailsResult> {
        return new GetProductDetailsResult();
    }
}
