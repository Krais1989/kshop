import "./home-page.sass";
import "styles/base-page.sass";

import * as React from "react";
import WAllProductsList from "components/widgets/w-all-products-list/w-all-products-list";
import { AppSettings, useAppSettings } from "components/app/app-settings";
import { AppServices } from "components/app/app-services";
import RedirectService from "services/RedirectService";
import { useAuth } from "components/contexts/AuthContext";

interface IHomePageProps {}

const HomePage: React.FunctionComponent<IHomePageProps> = (props) => {
    const { auth, setAuth } = useAuth();

    return (
        <div className="kshop-base-page kshop-home-page">
            <h1>HomePage</h1>
            <WAllProductsList />
        </div>
    );
};

export default HomePage;
