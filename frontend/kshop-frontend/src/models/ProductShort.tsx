

export class ProductShort {
    id: number;
    title: string;
    price: number;
    image?: string;
    description?: string;

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