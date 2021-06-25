export class Money {
    price: number;
    currency: string;

    constructor(price: number, currency: string = "RUB"){
        this.price = price;
        this.currency = currency;
    }
}