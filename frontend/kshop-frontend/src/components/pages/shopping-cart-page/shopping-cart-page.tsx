import * as React from 'react';
import "./shopping-cart-page.sass";
import "styles/base-page.sass";
import WShoppingCart from 'components/widgets/w-shopping-cart/w-shopping-cart';
import { AppSettingsContext } from 'components/app/AppSettings';

interface IShoppingCartPageProps { }

const ShoppingCartPage: React.FunctionComponent<IShoppingCartPageProps> = (props) => {
    console.log(`CartPage: `);
    console.log(AppSettingsContext);
    
    return (
        <div className="kshop-base-page kshop-shopping-cart-page">
            <WShoppingCart />
        </div>
    )
};

export default ShoppingCartPage;