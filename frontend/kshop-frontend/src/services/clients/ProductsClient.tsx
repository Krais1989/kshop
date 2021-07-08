import { AppSettings } from "components/app/AppSettings";
import { AuthService } from "services/AuthService";
import { ClientUtils } from "./ClientUtils";
import {
    GetProductsForHomeRequest,
    GetProductsForHomeResponse,
    GetProductDetailsRequest,
    GetProductDetailsResponse,
    GetProductsForOrderRequest,
    GetProductsForOrderResponse,
    GetProductsBookmarkedResponse,
    AddBookmarksRequest,
    DeleteBookmarksRequest,
    AddBookmarksResponse,
    DeleteBookmarksResponse,
    GetBookmarksResponse,
} from "./dtos/ProductsDtos";
import { HttpClient } from "./HttpClient";

class CProductsClient {

    //getProductsBookmarked = async()

    getBookmarks = async() => {
        return await HttpClient.get<GetBookmarksResponse>(
            GetBookmarksResponse,
            `${AppSettings.ProductsHost}/api/bookmarks`,
            AuthService.getAuthHeader()
        );
    }

    addBookmarks = async(data: AddBookmarksRequest) => {
        return await HttpClient.post<AddBookmarksRequest, AddBookmarksResponse>(
            AddBookmarksResponse,
            `${AppSettings.ProductsHost}/api/bookmarks`,
            data,
            AuthService.getAuthHeader()
        );
    }

    delBookmarks = async(data: DeleteBookmarksRequest) => {
        return await HttpClient.delete<DeleteBookmarksRequest, DeleteBookmarksResponse>(
            DeleteBookmarksResponse,
            `${AppSettings.ProductsHost}/api/bookmarks`,
            data,
            AuthService.getAuthHeader()
        );
    }

    getProductsBookmarked = async () =>{
        return await HttpClient.get<GetProductsBookmarkedResponse>(
            GetProductsForHomeResponse,
            `${AppSettings.ProductsHost}/api/products/bookmarked`,
            AuthService.getAuthHeader()
        );
    }

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
        const params = data.productsIDs
            .map((pid, i) => `${i === 0 ? "?" : "&"}ProductsIDs=${pid}`)
            .join("");
        //const params = ClientUtils.objToQueryParams();
        return await HttpClient.get<GetProductsForOrderResponse>(
            GetProductsForOrderResponse,
            `${AppSettings.ProductsHost}/api/products/for-order${params}`
        );
    };
}

export const ProductsClient: CProductsClient = new CProductsClient();