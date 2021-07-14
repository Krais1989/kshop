import "./home-page.sass";
import "styles/base-page.sass";

import * as React from "react";
import WAllProductsList from "components/widgets/w-all-products-list/w-all-products-list";
import { useAuth } from "components/providers/AuthProvider";
import WCategoriesList from "components/widgets/w-categories-list/w-categories-list";


interface IHomePageProps {}

const HomePage: React.FunctionComponent<IHomePageProps> = (props) => {

    const [categoryID, setCategoryID] = React.useState(0);

    return (
        <div className="kshop-base-page kshop-home-page">
            {/* <h1>HomePage</h1> */}
            <WCategoriesList curCategoryID={categoryID} setCurCategoryFn={setCategoryID} />
            <WAllProductsList categoryId={categoryID} />
        </div>
    );
};

export default HomePage;
