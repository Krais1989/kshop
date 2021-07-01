import WOrderingDialog from 'components/widgets/w-ordering-dialog/w-ordering-dialog';
import * as React from 'react';
import "./ordering-page.sass";

interface IOrderingPageProps { }

const OrderingPage: React.FunctionComponent<IOrderingPageProps> = (props) => {
    return (
        <div className="kshop-base-page kshop-ordering-page">
            <h1>OrderingPage</h1>
            <WOrderingDialog />
        </div>
    )
};

export default OrderingPage;