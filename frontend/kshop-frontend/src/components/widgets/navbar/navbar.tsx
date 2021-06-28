import * as React from "react";
import { Link } from "react-router-dom";
import "./navbar.sass";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faBox,
    faHeart,
    faShoppingCart,
    faSignInAlt,
    faUserCircle,
} from "@fortawesome/free-solid-svg-icons";
import WLoginPanel from "components/widgets/w-login-panel/w-login-panel";
import { useState } from "react";
import { AppServices } from "components/app/app-services";
import { toast } from "react-toastify";
import { AppSettingsContext } from "components/app/app-settings";
import { useAuth } from "components/contexts/AuthContext";

interface INavbarProps {}

const Navbar: React.FunctionComponent<INavbarProps> = (props) => {
    const { auth, isAuthenticated } = useAuth();

    // useEffect(() => {
    //     const svc = AppServices.CatalogsApi?.getProducts(new GetProductsRequest());
    // }, [])

    //console.warn("AppSettings Test\n");

    const jsxAuthTabs = (
        <React.Fragment>
            <Link to="/account">
                {" "}
                <FontAwesomeIcon icon={faUserCircle} />
                &nbsp;Account{" "}
            </Link>
            <Link to="/orders">
                {" "}
                <FontAwesomeIcon icon={faBox} />
                &nbsp;Orders{" "}
            </Link>
            <Link to="/favorites">
                {" "}
                <FontAwesomeIcon icon={faHeart} />
                &nbsp;Favorites{" "}
            </Link>
        </React.Fragment>
    );

    const isAuth = isAuthenticated();
    return (
        <div className="kshop-navbar">
            <WLoginPanel />
            {isAuth && jsxAuthTabs}
            <Link to="/cart">
                <FontAwesomeIcon icon={faShoppingCart} />
                &nbsp;Cart
            </Link>
        </div>
    );
};

export default Navbar;
