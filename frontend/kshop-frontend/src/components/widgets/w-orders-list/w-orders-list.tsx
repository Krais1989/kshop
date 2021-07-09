import { useAuth } from "components/providers/AuthProvider";
import { Money } from "models/Money";
import { OrderDetails, OrderLog, OrderPosition } from "models/Orders";
import * as React from "react";
import Moment from "react-moment";
import { Link } from "react-router-dom";
import { OrdersClient } from "services/clients/OrdersClient";
import { ProductsClient } from "services/clients/ProductsClient";
import "./w-orders-list.sass";

interface IWOrdersListProps {}

class OrderDetailsLogView {
    status: string = "";
    date: string = "";
    constructor(status: string = "", date: string = "") {
        this.status = status;
        this.date = date;
    }
}

class OrderDetailsPositionView {
    productID: number = 0;
    title: string = "";
    image: string = "";
    quantity: number = 0;
    price: Money;
    constructor(
        id: number = 0,
        quantity: number,
        title: string = "",
        image: string = "",
        price: Money = new Money(0)
    ) {
        this.productID = id;
        this.quantity = quantity;
        this.title = title;
        this.image = image;
        this.price = price;
    }
}

class OrderDetailsView {
    id: string = "";
    createDate: string = "";
    price: Money = new Money(0);

    history: OrderDetailsLogView[] = [];
    positions: OrderDetailsPositionView[] = [];
}

const WOrdersList: React.FunctionComponent<IWOrdersListProps> = (props) => {
    const [data, setData] = React.useState<OrderDetailsView[]>([]);
    const { auth } = useAuth();

    React.useEffect(() => {
        async function Prepare() {
            const orders = (await OrdersClient.getOrders()).orders;
            if (orders.length === 0) return;

            const orders_prods_ids = orders.flatMap((e) => e.positions.map((p) => p.productID));
            const prod_ids = [...new Set(orders_prods_ids)];
            const products = (
                await ProductsClient.getProductsForOrder({
                    productsIDs: prod_ids,
                })
            ).data;
            const products_map = new Map(products.map((p) => [p.id, p]));

            const views = orders.map((o) => {
                const ov = new OrderDetailsView();
                ov.id = o.id;
                ov.createDate = o.createDate;
                ov.price = o.price;
                ov.history = o.logs.map((h) => new OrderDetailsLogView(h.status, h.date));
                ov.positions =
                    o.positions.map(
                        (pos, i) =>
                            new OrderDetailsPositionView(
                                pos.productID,
                                pos.quantity,
                                products_map.get(pos.productID)?.title,
                                products_map.get(pos.productID)?.image
                            )
                    ) ?? [];
                return ov;
            });
            setData(views);
        }
        Prepare();
    }, [auth]);

    if (data.length === 0) return <h2>No orders</h2>;

    const jsxOrderDetailsFunc = (order_details: OrderDetailsView) => {
        const jsxPositionsFunc = (op: OrderDetailsPositionView) => (
            <Link
                className="kshop-w-orders-list-row-body-positions-product"
                to={`catalog/products/${op.productID}`}
                key={op.productID}
            >
                <img src={op.image} alt={op.title} />
            </Link>
        );

        const jsxHistoryFunc = (hist: OrderDetailsLogView) => (
            <li key={hist.status}>
                <span>
                    <span>
                        <Moment format="DD.MM.YYYY hh:mm:ss">{hist.date}</Moment>
                    </span>
                    <span>{hist.status}</span>
                </span>
            </li>
        );

        const jsxPositions = order_details.positions?.map((od, i) => jsxPositionsFunc(od));

        const jsxHistory = order_details.history?.map((hist, i) => jsxHistoryFunc(hist));

        // const order_price: number = orderDet.positions
        //     .map((e) => e.price.price)
        //     .reduce((prev, cur) => prev + cur);

        return (
            <div key={order_details.id} className="kshop-w-orders-list-row">
                <div className="kshop-w-orders-list-row-header">
                    <div className="kshop-w-orders-list-row-header-date">
                        <Moment format="DD.MM.YYYY">{order_details.createDate}</Moment>
                    </div>
                    <div className="kshop-w-orders-list-row-header-price">
                        Price {order_details.price.price}
                    </div>
                </div>
                <div className="kshop-w-orders-list-row-body">
                    <div className="kshop-w-orders-list-row-body-history">
                        <ul>{jsxHistory}</ul>
                    </div>
                    <div className="kshop-w-orders-list-row-body-positions">{jsxPositions}</div>
                </div>
            </div>
        );
    };

    const jsxOrdersRows = data.map((o, i) => jsxOrderDetailsFunc(o));

    return <div className="kshop-w-orders-list">{jsxOrdersRows}</div>;
};

export default WOrdersList;
