import * as React from 'react';
import WOrdersList from '../../widgets/w-orders-list/w-orders-list';

import "../base-page/base-page.sass";
import "./orders-page.sass";

interface IOrdersPageProps { }

const OrdersPage: React.FunctionComponent<IOrdersPageProps> = (props) => {
    return (
        <div className="kshop-base-page kshop-orders-page">
            Orders Page
            <WOrdersList />
        </div>
    )
};

export default OrdersPage;