import "./home-page.sass";
import "../base-page/base-page.sass";

import * as React from "react";
import WAllProductsList from "../../widgets/w-all-products-list/w-all-products-list";

interface IHomePageProps {}

const HomePage: React.FunctionComponent<IHomePageProps> = (props) => {

    return (
        <div className="kshop-base-page">
            <h1>HomePage</h1>
            <WAllProductsList />
        </div>
    );
};

export default HomePage;
