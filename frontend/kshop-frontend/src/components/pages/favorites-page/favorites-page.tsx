import WBookmarkedProducts from 'components/widgets/w-favorit-products/w-bookmarked-products';
import * as React from 'react';
import "./favorites-page.sass";

interface IFavoritesPageProps { }

const FavoritesPage: React.FunctionComponent<IFavoritesPageProps> = (props) => {
    return (
        <div className="kshop-base-page kshop-favorites-page">
            <WBookmarkedProducts/>
        </div>
    )
};

export default FavoritesPage;