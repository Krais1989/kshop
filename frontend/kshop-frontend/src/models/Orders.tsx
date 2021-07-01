import { Money } from "./Money";

export class OrderDetails {
    id: string = "";
    createDate: string = "";
    price: Money = new Money(0);

    logs: OrderLog[] = [];
    positions: OrderPosition[] = [];
}

export class OrderPosition {
    productID: number = 0;
    quantity: number = 0;
    price: Money = new Money(0);
    image: string = "";
}

export class OrderLog {
    status: string = "";
    date: string = "";
}
