import { Money } from "./Money";

export class FavoritProduct {
    productID: number;
    price: Money;
    title: string;
    description?: string;
    image?: string;

    constructor(
        productID: number,
        title: string,
        price: Money,
        description?: string,
        image?: string
    ) {
        this.productID = productID;
        this.title = title;
        this.price = price;
        this.description = description;
        this.image = image;
    }
}