import { Money } from "models/Money";
import { OrderDetails, OrderLog, OrderPosition } from "models/Orders";
import * as React from "react";
import "./w-orders-list.sass";

interface IWOrdersListProps {}

const WOrdersList: React.FunctionComponent<IWOrdersListProps> = (props) => {
    const [orders, setOrder] = React.useState<OrderDetails[]>([]);

    React.useEffect(() => {
        const genPositions = (count: number, offset: number = 0) =>
            Array.from(Array(count).keys()).map(
                (n, i) =>
                    new OrderPosition(
                        n + offset,
                        1,
                        new Money(100),
                        "https://cdn1.ozone.ru/multimedia/wc100/1024295937.jpg"
                    )
            );

        const genHistory = (count: number, offset: number = 0) =>
            Array.from(Array(count).keys()).map(
                (n, i) => new OrderLog(`Status ${n}`, new Date().toDateString())
            );

        const genOrders = (count: number, offset: number = 0) =>
            Array.from(Array(count).keys()).map(
                (n, i) =>
                    new OrderDetails(
                        `AA-3432-B-${i}`,
                        new Date().toDateString(),
                        new Money(100),
                        genHistory(5),
                        genPositions(4)
                    )
            );

        setOrder(genOrders(5));
    }, []);

    const jsxOrderDetailsFunc = (orderDet: OrderDetails) => {
        const jsxPositionsFunc = (op: OrderPosition) => (
            <a key={op.productId} href={`catalog/products/${op.productId}`}>
                <span>
                    <img src={op.image} alt="" />
                </span>
            </a>
        );

        const jsxHistoryFunc = (hist: OrderLog) => (
            <li key={hist.status}>
                <span>
                    <span>{hist.date}</span>
                    <span>{hist.status}</span>
                </span>
            </li>
        );

        const jsxPositions = orderDet.positions?.map((od, i) =>
            jsxPositionsFunc(od)
        );

        const jsxHistory = orderDet.history?.map((hist, i) =>
            jsxHistoryFunc(hist)
        );

        return (
            <div key={orderDet.id} className="kshop-w-orders-list-row">
                <div className="kshop-w-orders-list-row-header">
                    <div className="kshop-w-orders-list-row-header-date">
                        {orderDet.createDate}
                    </div>
                    <div className="kshop-w-orders-list-row-header-price">
                        Price {orderDet.price.value}
                    </div>
                </div>
                <div className="kshop-w-orders-list-row-body">
                    <div className="kshop-w-orders-list-row-body-history">
                        <ul>{jsxHistory}</ul>
                    </div>
                    <div className="kshop-w-orders-list-row-body-positions">
                        {jsxPositions}
                    </div>
                </div>
            </div>
        );
    };

    const jsxOrdersRows = orders.map((o, i) => jsxOrderDetailsFunc(o));

    return <div className="kshop-w-orders-list">{jsxOrdersRows}</div>;
};

export default WOrdersList;
