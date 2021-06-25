import { Money } from "./Money";

export class FavoritProduct {
    productId: number;
    price: Money;
    title: string;
    description?: string;
    image?: string;

    constructor(
        productId: number,
        title: string,
        price: Money,
        description?: string,
        image?: string
    ) {
        this.productId = productId;
        this.title = title;
        this.price = price;
        this.description = description;
        this.image = image;
    }
}