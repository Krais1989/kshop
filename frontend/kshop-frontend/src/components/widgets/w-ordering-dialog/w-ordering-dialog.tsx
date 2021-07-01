import { AppServices } from "components/app/app-services";
import { useCart } from "components/contexts/CartContext";
import { useRedirect } from "components/contexts/RedirectContext";
import { ProductQuantity } from "models/ProductQuantity";
import * as React from "react";
import { useState } from "react";
import { CreateOrderRequest } from "services/clients/abstractions/IOrdersClient";
import "./w-ordering-dialog.sass";

interface IWOrderingDialogProps {}

const WOrderingDialog: React.FunctionComponent<IWOrderingDialogProps> = (
    props
) => {
    const cart = useCart();
    const redirect = useRedirect();
    const [isLoad, setIsLoad] = useState(false);
    const [data, setData] = useState<CreateOrderRequest>({
        address: "",
        paymentProvider: 0,
        shippingMethod: 0,
        orderContent: cart.cart.positions.map(
            (e, i) => new ProductQuantity(e.productID, e.quantity)
        ), // [{ productID: 3, quantity: 1 }],
    });

    const jsxProducts = cart.cart.positions.map((e, i) => (
        <img
            key={e.productID}
            alt={e.title}
            onClick={() => redirect.toProductDetails(e.productID)}
            src={e.image}
        />
    ));

    const submit = () => {
        if (isLoad) return;
        AppServices.Clients.Orders.createOrder(data).then((r) => {
            setIsLoad(false);
            redirect.toMyOrders();
        });
        setIsLoad(true);
    };

    return (
        <div className="ks-ordering">
            <div className="ks-ordering-info">
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">
                        Payment method
                    </span>
                    <span>
                        <select>
                            <option value="">Credit card</option>
                            <option value="">Yookass</option>
                            <option value="">Qiwi</option>
                        </select>
                    </span>
                </div>
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">
                        Receive method
                    </span>
                    <span>
                        <select>
                            <option value="">Pickup</option>
                            <option value="">Delivery</option>
                        </select>
                    </span>
                </div>
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">
                        Receiver info
                    </span>
                    Address
                    <textarea />
                </div>
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">
                        Products
                    </span>
                    <div className="ks-ordering-info-panel-products">
                        {jsxProducts}
                    </div>
                </div>
            </div>

            <div className="ks-ordering-сtrl">
                <button
                    className="ks-ordering-submit kshop-button-blue"
                    onClick={(e) => submit()}
                    disabled={isLoad ? true : false}
                >
                    Submit
                </button>
                <div className="ks-ordering-сtrl-element">
                    <span>Price</span> <span>1000</span>
                </div>
                <div className="ks-ordering-сtrl-element">
                    <span>Products</span> <span>1000</span>
                </div>

                <div className="ks-ordering-сtrl-element">
                    <span>Shipment</span> <span>1000</span>
                </div>

                <div className="ks-ordering-сtrl-element">
                    <span>Overall</span> <span>1000</span>
                </div>
            </div>
        </div>
    );
};

export default WOrderingDialog;
