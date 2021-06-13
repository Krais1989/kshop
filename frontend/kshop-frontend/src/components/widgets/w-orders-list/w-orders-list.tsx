import * as React from 'react';
import "./w-orders-list.sass";

interface IWOrdersListProps { }

const WOrdersList: React.FunctionComponent<IWOrdersListProps> = (props) => {
    return (
        <div className="kshop-w-orders-list">
            Orders List Widget
        </div>
    )
};

export default WOrdersList;