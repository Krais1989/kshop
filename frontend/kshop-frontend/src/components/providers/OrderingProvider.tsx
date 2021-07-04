import { OrderPosition } from "models/Orders";
import React, { createContext, useContext, useState } from "react";
import { OrdersClient } from "services/clients/OrdersClient";

export class OrderingData {
    positions: Array<OrderPosition> = [];
}

export class OrderingContextData {
    ordering: OrderingData = new OrderingData();
    setOrdering: (data: Array<OrderPosition>) => void = () => {};
}

const OrderingContext = createContext<OrderingContextData>(new OrderingContextData());

interface IProps {}

export const OrderingProvider: React.FunctionComponent<IProps> = (props) => {
    const [ordering, setOrdering] = useState(new OrderingData());

    return (
        <OrderingContext.Provider
            value={{
                ordering: ordering,
                setOrdering: (data) => setOrdering({ positions: data }),
            }}
        >
            {props.children}
        </OrderingContext.Provider>
    );
};

export const useOrdering = () => useContext(OrderingContext);
