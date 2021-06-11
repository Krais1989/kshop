import ProductAttribute from "./ProductAttribute";

export class ProductDetails {
    id: number;
    title: string;
    price: number;
    image?: string;
    description?: string;

    attributes?: ProductAttribute[];

    constructor(
        id:number, 
        title:string, 
        price:number, 
        image?:string, 
        description?:string) 
    {
        this.id = id;
        this.title = title;
        this.price = price;
        this.image = image;
        this.description = description;
    }
}

