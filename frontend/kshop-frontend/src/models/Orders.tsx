import { Money } from "./Money";

export class OrderDetails {
    id: string;
    createDate: string;
    price: Money;

    history?: OrderLog[];
    positions?: OrderPosition[];

    constructor(
        id: string,
        createDate: string,
        price: Money,
        history?: OrderLog[],
        positions?: OrderPosition[]
    ) {
        this.id = id;
        this.createDate = createDate;
        this.price = price;
        this.history = history;
        this.positions = positions;
    }
}

export class OrderPosition {
    productID: number;
    quantity: number;
    price: Money;
    image: string;

    constructor(
        productID: number,
        quantity: number,
        price: Money,
        image: string
    ) {
        this.productID = productID;
        this.quantity = quantity;
        this.price = price;
        this.image = image;
    }
}

export class OrderLog {
    status: string;
    date: string;

    constructor(status: string, date: string) {
        this.status = status;
        this.date = date;
    }
}
