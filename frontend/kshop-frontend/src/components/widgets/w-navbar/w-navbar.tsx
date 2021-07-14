import * as React from "react";
import { Link } from "react-router-dom";
import "./w-navbar.sass";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faBookmark,
    faBox,
    faHeart,
    faShoppingCart,
    faUserCircle,
} from "@fortawesome/free-solid-svg-icons";
import WLoginPanel from "components/widgets/w-login-panel/w-login-panel";
import { useAuth } from "components/providers/AuthProvider";

interface INavbarProps {}

const WNavbar: React.FunctionComponent<INavbarProps> = (props) => {
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
            <Link to="/my-orders">
                {" "}
                <FontAwesomeIcon icon={faBox} />
                &nbsp;Orders{" "}
            </Link>
            <Link to="/favorites">
                {" "}
                <FontAwesomeIcon icon={faBookmark} />
                &nbsp;Bookmarks{" "}
            </Link>
        </React.Fragment>
    );

    const isAuth = isAuthenticated();
    return (
        <div className="kshop-w-navbar">
            <WLoginPanel />
            {isAuth && jsxAuthTabs}
            <Link to="/cart">
                <FontAwesomeIcon icon={faShoppingCart} />
                &nbsp;Cart
            </Link>
        </div>
    );
};

export default WNavbar;
