import * as React from "react";
import "./product-card.sass";
import nophoto from "./nophoto.png";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faCartArrowDown,
    faHeart,
    faHeartBroken,
    faShareSquare,
} from "@fortawesome/free-solid-svg-icons";
import DropDown from "components/controls/drop-down/drop-down";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import { Money } from "models/Money";
import RedirectService from "services/RedirectService";

export interface IProductCardProps {
    id: number;
    title: string;
    description?: string;
    image?: string;
    price: Money;
}

const ProductCard: React.FunctionComponent<IProductCardProps> = (props) => {
    
    const productLink = `catalog/products/${props.id}`;
    let title = `${props.title}`;
    if (props.description) title = `${title}, ${props.description}`;

    const max_title = 30;
    if (title.length > max_title) title = `${title.substr(0, max_title)}...`;

    return (
        <div key={props.id} className="kshop-product-card">
            <div className="kshop-product-card-image">
                <span className="kshop-product-card-fav">
                    <FontAwesomeIcon
                        onClick={() =>
                            toast(`LIKE ${props.id}`, { autoClose: 2000 })
                        }
                        icon={faHeartBroken}
                    />
                </span>
                <Link to={productLink}>
                    <img
                        className="kshop-product-card-image"
                        src={props.image ?? nophoto}
                        alt="Product"
                    ></img>
                </Link>
            </div>

            <div className="kshop-product-card-price">
                <span>{props.price.price} &#8381;</span>
            </div>

            <div className="kshop-product-card-title">
                <Link to={productLink}>
                    <span>{title}</span>
                </Link>
            </div>

            <div className="kshop-product-card-actions">
                <button
                    className="kshop-button-green"
                    onClick={() => {
                        toast(`В корзину ${props.id}`);
                    }}
                >
                    В корзину <FontAwesomeIcon icon={faCartArrowDown} />{" "}
                </button>
                <DropDown
                    title="..."
                    className="kshop-product-card-actions-extra"
                >
                    <a
                        href="#"
                        onClick={() => {
                            toast("Action 1");
                        }}
                    >
                        Action 1
                    </a>
                    <a
                        href="#"
                        onClick={() => {
                            toast("Action 2");
                        }}
                    >
                        Action 2
                    </a>
                    <a
                        href="#"
                        onClick={() => {
                            toast("Action 3");
                        }}
                    >
                        Action 3
                    </a>
                </DropDown>
            </div>
        </div>
    );
};

export default ProductCard;
