import * as React from 'react';
import ProductAttribute from '../../../models/ProductAttribute';
import { ProductDetails } from '../../../models/ProductDetails';
import "./w-product-details.sass";

interface IWProductDetailsProps { 
    productId:number;
}

const WProductDetails: React.FunctionComponent<IWProductDetailsProps> = (props) => {

    const {productId} = props;

    let product_details = new ProductDetails(productId, `Product ${productId}`, 100);
    product_details.attributes = [
        new ProductAttribute("Идентификатор", product_details.id.toString()),
        new ProductAttribute("Название", product_details.title),
        new ProductAttribute("Цена", `${product_details.price} руб.`),
    ];

    const jsxAttrs = product_details.attributes.map((pa, index) => (
        <div className="kshop-w-product-details-attr">
            <div className="kshop-w-product-details-attr-name">{pa.name}</div>
            <div className="kshop-w-product-details-attr-value">{pa.value}</div>
        </div>
    ));

    return (
        <div className="kshop-w-product-details">
            {jsxAttrs}
        </div>
    )
};

export default WProductDetails;