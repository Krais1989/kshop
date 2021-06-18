import * as React from 'react';
import "./shopping-cart-page.sass";
import "styles/base-page.sass";
import WShoppingCart from 'components/widgets/w-shopping-cart/w-shopping-cart';

interface IShoppingCartPageProps { }

const ShoppingCartPage: React.FunctionComponent<IShoppingCartPageProps> = (props) => {
    return (
        <div className="kshop-base-page kshop-shopping-cart-page">
            <WShoppingCart />
        </div>
    )
};

export default ShoppingCartPage;