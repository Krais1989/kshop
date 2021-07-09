import * as React from 'react';
import WOrdersList from '../../widgets/w-orders-list/w-orders-list';

import "styles/base-page.sass";
import "./my-orders-page.sass";

interface IMyOrdersPageProps { }

const MyOrdersPage: React.FunctionComponent<IMyOrdersPageProps> = (props) => {
    return (
        <div className="kshop-base-page kshop-orders-page">
            <WOrdersList />
        </div>
    )
};

export default MyOrdersPage;