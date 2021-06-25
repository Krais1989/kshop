import WFavoritProducts from 'components/widgets/w-favorit-products/w-favorit-products';
import * as React from 'react';
import "./favorites-page.sass";

interface IFavoritesPageProps { }

const FavoritesPage: React.FunctionComponent<IFavoritesPageProps> = (props) => {
    return (
        <div className="kshop-base-page kshop-favorites-page">
            <WFavoritProducts/>
        </div>
    )
};

export default FavoritesPage;