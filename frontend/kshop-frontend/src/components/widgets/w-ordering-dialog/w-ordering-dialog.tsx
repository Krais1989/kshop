import Select, { SelectOption } from "components/controls/select/select";
import { useCart } from "components/providers/CartProvider";
import { useRedirect } from "components/providers/RedirectProvider";
import * as React from "react";
import { useState } from "react";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import { SubmitOrderRequest } from "services/clients/dtos/OrdersDtos";
import { OrdersClient } from "services/clients/OrdersClient";
import "./w-ordering-dialog.sass";

interface IWOrderingDialogProps {}

const WOrderingDialog: React.FunctionComponent<IWOrderingDialogProps> = (props) => {
    const { cart, removeFromCart } = useCart();
    const redirect = useRedirect();
    const [isLoad, setIsLoad] = useState(false);
    // const [data, setData] = useState<CreateOrderRequest>({
    //     address: "",
    //     paymentProvider: 0,
    //     shippingMethod: 0,
    //     orderContent: cartData.cart.positions.map(
    //         (e, i) => new ProductQuantity(e.productID, e.quantity)
    //     ), // [{ productID: 3, quantity: 1 }],
    // });

    const [paymentType, setPaymentType] = useState("Mock");
    const [shipmentType, setShipmentType] = useState("Default");
    const [address, setAddress] = useState("");

    const jsxProducts = cart.positions
        .filter((e, i) => e.checked)
        .map((e, i) => (
            <Link to="/" key={e.productID}>
                <img
                    alt={e.title}
                    onClick={() => redirect.toProductDetails(e.productID)}
                    src={e.image}
                />
            </Link>
        ));

    const submit = () => {
        if (isLoad) return;
        const submitOrderRequest: SubmitOrderRequest = {
            address: { data: address },
            orderContent: cart.positions.filter((e, i) => e.checked),
            payment: { type: paymentType },
            shipment: { type: shipmentType },
        };

        OrdersClient.submitOrder(submitOrderRequest).then((r) => {
            if (r.isSuccess) {
                removeFromCart(cart.positions.filter((e, i) => e.checked).map((e) => e.productID));
                setIsLoad(false);
                redirect.toMyOrders();
            } else {
                console.log(r.errorMessage);
            }
        });
        setIsLoad(true);
    };

    const payment_opts: Array<SelectOption> = [
        { value: "Mock", label: "Mock" },
        { value: "Yookassa", label: "Yookassa" },
    ];

    const shipment_opts: Array<SelectOption> = [
        { value: "Default", label: "Default" },
        { value: "Pickup", label: "Pickup" },
    ];

    //const order_price = 111;

    const base_price = cart.getCheckedPrice();
    const discount_price = base_price;
    const shipment_price = 100;
    const overall_price = discount_price + shipment_price;

    return (
        <div className="ks-ordering">
            <div className="ks-ordering-info">
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">Payment method</span>
                    <span>
                        <Select
                            disabled={true}
                            selected={paymentType}
                            data={payment_opts}
                            onChange={(e) => setPaymentType(e)}
                        ></Select>
                    </span>
                </div>
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">Receive method</span>
                    <span>
                        <Select
                            disabled={true}
                            selected={shipmentType}
                            data={shipment_opts}
                            onChange={(e) => setShipmentType(e)}
                        ></Select>
                    </span>
                </div>
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">Receiver info</span>
                    Address
                    <textarea onChange={(e) => setAddress(e.target.value)} value={address} />
                </div>
                <div className="ks-ordering-info-panel">
                    <span className="ks-ordering-info-panel-title">Products</span>
                    <div className="ks-ordering-info-panel-products">{jsxProducts}</div>
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
                    <span>Price</span> <span>{base_price}</span>
                </div>
                <div className="ks-ordering-сtrl-element">
                    <span>With discount</span> <span>{discount_price}</span>
                </div>

                <div className="ks-ordering-сtrl-element">
                    <span>Shipment</span> <span>{shipment_price}</span>
                </div>

                <div className="ks-ordering-сtrl-element">
                    <span>Overall</span> <span>{overall_price}</span>
                </div>
            </div>
        </div>
    );
};

export default WOrderingDialog;
