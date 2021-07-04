import { AppSettings } from "components/app/AppSettings";
import { ClientUtils } from "./ClientUtils";
import {
    GetProductsForHomeRequest,
    GetProductsForHomeResponse,
    GetProductDetailsRequest,
    GetProductDetailsResponse,
    GetProductsForOrderRequest,
    GetProductsForOrderResponse,
} from "./dtos/ProductsDtos";
import { HttpClient } from "./HttpClient";

class CProductsClient {

    getProductsForHome = async (data: GetProductsForHomeRequest) => {
        const params = ClientUtils.objToQueryParams(data);
        return await HttpClient.get<GetProductsForHomeResponse>(
            GetProductsForHomeResponse,
            `${AppSettings.ProductsHost}/api/products/for-home${params}`
        );
    };

    getProductsDetails = async (data: GetProductDetailsRequest) => {
        const params = ClientUtils.objToQueryParams(data);
        return await HttpClient.get<GetProductDetailsResponse>(
            GetProductDetailsResponse,
            `${AppSettings.ProductsHost}/api/products/details${params}`
        );
    };

    getProductsForOrder = async (data: GetProductsForOrderRequest) => {
        const params = data.productIDs
            .map((pid, i) => `${i === 0 ? "?" : "&"}ProductIDs=${pid}`)
            .join("");
        //const params = ClientUtils.objToQueryParams();
        return await HttpClient.get<GetProductsForOrderResponse>(
            GetProductsForOrderResponse,
            `${AppSettings.ProductsHost}/api/products/for-order${params}`
        );
    };
}

export const ProductsClient: CProductsClient = new CProductsClient();