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

interface INavbarProps {}

const Navbar: React.FunctionComponent<INavbarProps> = (props) => {
    const [isAuth, setIsAuth] = useState(false);
    
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
    const jsxGuestTabs = <WLoginPanel />;

    return (
        <div className="kshop-navbar">
            <button
                className="kshop-button-gray"
                onClick={() => setIsAuth(!isAuth)}
            >
                {isAuth ? "test logout" : "test login"}
            </button>
            {isAuth ? jsxAuthTabs : jsxGuestTabs}
            <Link to="/cart">
                <FontAwesomeIcon icon={faShoppingCart} />
                &nbsp;Cart
            </Link>
        </div>
    );
};

export default Navbar;
