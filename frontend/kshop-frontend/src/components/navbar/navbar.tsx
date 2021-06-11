import * as React from "react";
import { Link } from "react-router-dom";
import "./navbar.sass";

interface INavbarProps {}

const Navbar: React.FunctionComponent<INavbarProps> = (props) => {
    return (
        <div className="kshop-navbar">
            <ul>
                <li>
                    <Link to="/account"> Account </Link>
                </li>
                <li>
                    <Link to="/orders"> Orders </Link>
                </li>
                <li>
                    <Link to="/favorites"> Favorites </Link>
                </li>
            </ul>
        </div>
    );
};

export default Navbar;
