import { Money } from "./Money";

export class CartPosition {
    productID: number = 0;
    quantity: number = 0;
    checked: boolean = false;
    title: string = "";
    price: Money = new Money(0);
    description?: string = "";
    image?: string = "";
}

export class Cart {
    positions: Array<CartPosition> = [];

    add: (pos: CartPosition) => number = (pos) => {
        let findPos = this.positions.find((e) => e.productID === pos.productID);
        if (findPos) {
            findPos.quantity = pos.quantity;
        } else {
            this.positions.push(pos);
            findPos = pos;
        }
        return this.positions.indexOf(findPos);
    };

    addRange: (posRange: Array<CartPosition>) => void = (posRange) => {
        posRange.forEach((pos) => {
            this.add(pos);
        });
    };

    remove: (productID: number) => void = (productID) => {
        if (!this.positions || this.positions.length === 0) return -1;
        let findPos = this.positions.findIndex(
            (e) => e.productID === productID
        );
        this.positions.splice(findPos, 1);
    };

    removeRange: (ids: Array<number>) => void = (ids) => {
        ids.forEach((pos) => {
            this.remove(pos);
        });
    };

    clear: () => void = () => {
        this.positions = [];
    };
}
