import * as React from "react";
import { Link } from "react-router-dom";
import "./navbar.sass";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBox, faHeart, faShoppingCart, faSignInAlt, faUserCircle } from '@fortawesome/free-solid-svg-icons'

interface INavbarProps {}

const Navbar: React.FunctionComponent<INavbarProps> = (props) => {
    return (
        <div className="kshop-navbar">
            <div className="kshop-navbar-home">
                <Link to="/">HOME</Link>
            </div>
            <ul>
                <li>
                    <Link to="/login"> <FontAwesomeIcon icon={faSignInAlt} /> Sign In </Link>
                </li>
                <li>
                    <Link to="/account"> <FontAwesomeIcon icon={faUserCircle} /> Account </Link>
                </li>
                <li>
                    <Link to="/orders"> <FontAwesomeIcon icon={faBox} /> Orders </Link>
                </li>
                <li>
                    <Link to="/favorites">  <FontAwesomeIcon icon={faHeart} /> Favorites </Link>
                </li>
                <li>
                    <Link to="/cart">  <FontAwesomeIcon icon={faShoppingCart} /> Cart </Link>
                </li>
            </ul>
        </div>
    );
};

export default Navbar;
