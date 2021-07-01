import { AppServices } from "components/app/app-services";
import { Money } from "models/Money";
import { OrderDetails, OrderLog, OrderPosition } from "models/Orders";
import * as React from "react";
import { Link } from "react-router-dom";
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

    React.useEffect(() => {
        // const genPositions = (count: number, offset: number = 0) =>
        //     Array.from(Array(count).keys()).map(
        //         (n, i) =>
        //             new OrderPosition(
        //                 n + offset,
        //                 1,
        //                 new Money(100),
        //                 "https://cdn1.ozone.ru/multimedia/wc100/1024295937.jpg"
        //             )
        //     );

        // const genHistory = (count: number, offset: number = 0) =>
        //     Array.from(Array(count).keys()).map(
        //         (n, i) => new OrderLog(`Status ${n}`, new Date().toDateString())
        //     );

        // const genOrders = (count: number, offset: number = 0) =>
        //     Array.from(Array(count).keys()).map(
        //         (n, i) =>
        //             new OrderDetails(
        //                 `AA-3432-B-${i}`,
        //                 new Date().toDateString(),
        //                 new Money(100),
        //                 genHistory(5),
        //                 genPositions(4)
        //             )
        //     );

        async function Prepare() {
            const orders = (await AppServices.Clients.Orders.getOrders()).orders;
            const prod_ids = orders.flatMap((e) => e.positions.map((p) => p.productID));
            const products = (
                await AppServices.Clients.Products.getProductsForOrder({
                    productIDs: prod_ids,
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
    }, []);

    if (data.length === 0) return <h2>No orders</h2>;

    const jsxOrderDetailsFunc = (orderDet: OrderDetailsView) => {
        const jsxPositionsFunc = (op: OrderDetailsPositionView) => (
            <Link to={`catalog/products/${op.productID}`} key={op.productID}>
                <span>
                    <img src={op.image} alt={op.title} />
                </span>
            </Link>
        );

        const jsxHistoryFunc = (hist: OrderDetailsLogView) => (
            <li key={hist.status}>
                <span>
                    <span>{hist.date}</span>
                    <span>{hist.status}</span>
                </span>
            </li>
        );

        const jsxPositions = orderDet.positions?.map((od, i) => jsxPositionsFunc(od));

        const jsxHistory = orderDet.history?.map((hist, i) => jsxHistoryFunc(hist));

        const order_price: number = orderDet.positions
            .map((e) => e.price.price)
            .reduce((prev, cur) => prev + cur);

        return (
            <div key={orderDet.id} className="kshop-w-orders-list-row">
                <div className="kshop-w-orders-list-row-header">
                    <div className="kshop-w-orders-list-row-header-date">{orderDet.createDate}</div>
                    <div className="kshop-w-orders-list-row-header-price">Price {order_price}</div>
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
