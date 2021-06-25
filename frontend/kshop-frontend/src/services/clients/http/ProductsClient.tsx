import { AppSettings } from "components/app/app-settings";
import {
    IProductsClient,
    GetProductsForHomeRequest,
    GetProductsForHomeResponse,
    GetProductDetailsRequest,
    GetProductDetailsResponse,
} from "../abstractions/IProductsClient";
import { HttpClient } from "./HttpClient";

export class ProductsClient implements IProductsClient {
    private http: HttpClient = new HttpClient();

    getProductsForHome = async (data: GetProductsForHomeRequest) =>
        await this.http.get<GetProductsForHomeResponse>(`${AppSettings.ProductsHost}/api/products/home/${data.page}`);
        

    getProductDetails = async (data: GetProductDetailsRequest) =>
        await this.http.get<GetProductDetailsResponse>(`${AppSettings.ProductsHost}/api/products/details/${data.productId}`);

        // for (let i = 0; i < 60; i++) {
        //     prods.push(
        //         new ProductShort(
        //             i,
        //             `Product ${i}`,
        //             new Money(100),
        //             undefined,
        //             `Description for product ${i}`
        //         )
        //     );
        // }
}
