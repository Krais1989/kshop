import { Money } from "./Money";

export class ProductDetails {
    id?: number;
    title?: string;
    price?: Money;
    image?: string;
    description?: string;

    attributes?: ProductAttribute[];

    constructor(
        id?: number,
        title?: string,
        price?: Money,
        image?: string,
        description?: string
    ) {
        this.id = id;
        this.title = title;
        this.price = price;
        this.image = image;
        this.description = description;
    }
}

export class ProductAttribute {
    id?: number;
    title?: string;
    value?: string;
}
