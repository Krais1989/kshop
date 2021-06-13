import * as React from "react";
import "./product-card.sass";
import nophoto from "./nophoto.png";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCartArrowDown } from "@fortawesome/free-solid-svg-icons";
import DropDown from "components/controls/drop-down/drop-down";

export interface IProductCardProps {
    id: number;
    title: string;
    description?: string;
    image?: string;
    price: number;
}

const ProductCard: React.FunctionComponent<IProductCardProps> = (props) => {
    const productLink = `catalog/products/${props.id}`;
    let title = `${props.title}`;
    if (props.description)
        title = `${title}, ${props.description}`;

    const max_title = 30;
    if (title.length > max_title)
        title = `${title.substr(0, max_title)}...`
    
    return (
        <div className="kshop-product-card">
            <div className="kshop-product-card-image">
                <a href={productLink}>
                    <img
                        className="kshop-product-card-image"
                        src={nophoto}
                        alt="Product"
                    />
                </a>
            </div>

            <div className="kshop-product-card-price">
                <span>{props.price} &#8381;</span>
            </div>
            <div className="kshop-product-card-title">
                <span>
                    <a href={productLink}>
                        {title}
                    </a>
                </span>
            </div>

            <div className="kshop-product-card-actions">
                <button className="kshop-button-green">В корзину <FontAwesomeIcon icon={faCartArrowDown} /> </button>
                <DropDown title="...">
                    <a href="#">Link 1</a>
                    <a href="#">Link 2</a>
                    <a href="#">Link 3</a>
                </DropDown>
            </div>
        </div>
    );
};

export default ProductCard;
