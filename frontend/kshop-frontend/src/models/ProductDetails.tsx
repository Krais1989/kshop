import { Money } from "./Money";

export class ProductDetails {
    id: number = 0;
    title: string = "";
    price: Money = new Money(100);
    image: string = "";
    description: string = "";

    attributes: ProductAttribute[] = [];

}

export class ProductAttribute {
    id: number = 0;
    title: string = "";
    value: string = "";
}
