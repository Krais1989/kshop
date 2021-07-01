export class ProductQuantity {
    productID: number = 0;
    quantity: number = 0;

    constructor(productId: number, quantity: number){
        this.productID = productId;
        this.quantity = quantity;
    }
}