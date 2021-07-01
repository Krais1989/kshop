import { AppSettings } from "components/app/app-settings";
import { ClientUtils } from "services/ClientUtils";
import {
    IProductsClient,
    GetProductsForHomeRequest,
    GetProductsForHomeResponse,
    GetProductDetailsRequest,
    GetProductDetailsResponse,
    GetProductsForOrderRequest,
    GetProductsForOrderResponse,
} from "../abstractions/IProductsClient";
import { HttpClient } from "./HttpClient";

export class ProductsClient implements IProductsClient {
    private http: HttpClient = new HttpClient();

    getProductsForHome = async (data: GetProductsForHomeRequest) => {
        const params = ClientUtils.objToQueryParams(data);
        return await this.http.get<GetProductsForHomeResponse>(
            `${AppSettings.ProductsHost}/api/products/for-home${params}`
        );
    };

    getProductsDetails = async (data: GetProductDetailsRequest) => {
        const params = ClientUtils.objToQueryParams(data);
        return await this.http.get<GetProductDetailsResponse>(
            `${AppSettings.ProductsHost}/api/products/details${params}`
        );
    };

    getProductsForOrder = async (data: GetProductsForOrderRequest) => {
        const params = data.productIDs
            .map((pid, i) => `${i === 0 ? "?" : "&"}ProductIDs=${pid}`)
            .join("");
        //const params = ClientUtils.objToQueryParams();
        return await this.http.get<GetProductsForOrderResponse>(
            `${AppSettings.ProductsHost}/api/products/for-order${params}`
        );
    };
}
