import * as React from "react";
import { ProductAttribute, ProductDetails } from "../../../models/ProductDetails";
import "./w-product-details.sass";

//import logo from "https://www.regard.ru/photo/goods/363952.png";

interface IWProductDetailsProps {
    productId: number;
}

const WProductDetails: React.FunctionComponent<IWProductDetailsProps> = (
    props
) => {
    const { productId } = props;

    let product_details = new ProductDetails(
        productId,
        `Product ${productId}`,
        100,
        undefined,
        `[Description for ${productId}] Lorem ipsum dolor, sit amet consectetur adipisicing elit. Fugiat, nihil reiciendis quam non vero esse perferendis. In ex dignissimos ab!`
    );
    product_details.attributes = [
        new ProductAttribute("Идентификатор", product_details.id.toString()),
        new ProductAttribute("Название", product_details.title),
        new ProductAttribute("Цена", `${product_details.price} руб.`),
    ];

    const jsxAttrs = product_details.attributes.map((pa, index) => (
        <div className="kshop-w-product-details-info-attributes-row">
            <span>{pa.name}</span>
            <span>{pa.value}</span>
        </div>
    ));

    return (
        <div className="kshop-w-product-details">
            <div className="kshop-w-product-details-title">
                <span>{product_details.title}</span>
            </div>
            <div className="kshop-w-product-details-topbar">Topbar</div>

            <div className="kshop-w-product-details-flex">
                <div className="kshop-w-product-details-galery">
                    <img src="https://www.regard.ru/photo/goods/363952.png" />
                    {/* {product_details.image ?? "No Image"} */}
                </div>
                <div className="kshop-w-product-details-price">
                    <span className="kshop-w-product-details-price-original">
                        {product_details.price} &#8381;
                    </span>
                    <button className="kshop-button-blue">Add to cart</button>
                </div>
                <div className="kshop-w-product-details-info">
                    <div className="kshop-w-product-details-info-attributes">
                        {jsxAttrs}
                    </div>
                    <div className="kshop-w-product-details-info-description">
                        {product_details.description}
                    </div>
                </div>
            </div>

            {/* {jsxAttrs} */}
        </div>
    );
};

export default WProductDetails;
