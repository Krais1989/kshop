export class ProductDetails {
    id: number;
    title: string;
    price: number;
    image?: string;
    description?: string;

    attributes?: ProductAttribute[];

    constructor(
        id: number,
        title: string,
        price: number,
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
    name: string;
    value: string;

    constructor(name: string, value: string) {
        this.name = name;
        this.value = value;
    }
}
