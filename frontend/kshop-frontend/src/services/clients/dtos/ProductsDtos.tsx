import { Category } from "models/Category";
import { ProductDetails } from "models/ProductDetails";
import { ProductPresentation } from "models/ProductPresentation";
import { BaseResult } from "services/BaseResult";

export class GetCategoriesResponse extends BaseResult {
    categories: Array<Category> = [];
}

export class GetBookmarksResponse extends BaseResult {
    productsIDs: Array<number> = [];
}

export class AddBookmarksRequest {
    productsIDs: Array<number> = [];
}
export class AddBookmarksResponse extends BaseResult {}

export class DeleteBookmarksRequest {
    productsIDs: Array<number> = [];
}
export class DeleteBookmarksResponse extends BaseResult {}

export class GetProductsBookmarkedResponse extends BaseResult {
    data: Array<ProductPresentation> = [];
}

export class GetProductsForHomeRequest {
    pageIndex: number = 0;
    categoryID: number = 0;
}
export class GetProductsForHomeResponse extends BaseResult {
    data: Array<ProductPresentation> = [];
}

export class GetProductDetailsRequest {
    productIDs: Array<number> = [];
}
export class GetProductDetailsResponse extends BaseResult {
    data: Array<ProductDetails> = [];
}

export class GetProductsForOrderRequest {
    productsIDs: Array<number> = [];
}
export class GetProductsForOrderResponse extends BaseResult {
    data: Array<ProductPresentation> = [];
}
