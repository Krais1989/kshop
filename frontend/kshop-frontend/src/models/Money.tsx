export class Money {
    currency: string;
    value: number;

    constructor(value: number, currency: string = "RUB"){
        this.value = value;
        this.currency = currency;
    }
}