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

export class Cart implements Iterable<CartPosition> {
    [Symbol.iterator](): Iterator<CartPosition, any, undefined> {
        let pointer = 0;
        let positions = this.positions;

        return {
            next(): IteratorResult<CartPosition> {
                if (pointer < positions.length) {
                    return {
                        done: false,
                        value: positions[pointer++],
                    };
                } else {
                    return {
                        done: true,
                        value: null,
                    };
                }
            },
        };
    }

    public getFullPrice() {
        return (
            this.positions
                .map((e) => e.price.price * e.quantity)
                .reduce((res, cur) => res + cur, 0) ?? 0
        );
    }

    public getCheckedPrice() {
        return (
            this.positions
                .filter((e) => e.checked)
                .map((e) => e.price.price * e.quantity)
                .reduce((res, cur) => res + cur, 0) ?? 0
        );
    }

    positions: Array<CartPosition> = [];

    public add(pos: CartPosition): number {
        let findPos = this.positions.find((e) => e.productID === pos.productID);
        if (findPos) {
            findPos.quantity = pos.quantity;
        } else {
            this.positions.push(pos);
            findPos = pos;
        }
        return this.positions.indexOf(findPos);
    }

    public addRange(posRange: Array<CartPosition>) {
        posRange.forEach((pos) => {
            this.add(pos);
        });
    }

    public remove(productID: number): void {
        if (!this.positions || this.positions.length === 0) return;
        let findPos = this.positions.findIndex((e) => e.productID === productID);
        this.positions.splice(findPos, 1);
    }

    public removeRange(ids: Array<number>) {
        ids.forEach((pos) => {
            this.remove(pos);
        });
    }

    public clear() {
        this.positions = [];
    }
}
