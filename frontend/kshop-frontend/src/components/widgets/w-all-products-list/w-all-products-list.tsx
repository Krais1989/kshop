import * as React from "react";
import { toast } from "react-toastify";
import { ProductShort } from "../../../models/ProductShort";
import ProductCard from "../../product-card/product-card";

import "./w-all-products-list.sass";

import "react-toastify/dist/ReactToastify.css";

interface IWAllProductsListProps {}

const WAllProductsList: React.FunctionComponent<IWAllProductsListProps> = (
    props
) => {
    let prods: ProductShort[] = [];

    for (let i = 0; i < 60; i++) {
        prods.push(
            new ProductShort(
                i,
                `Product ${i}`,
                100,
                undefined,
                `Description for product ${i}`
            )
        );
    }

    const jsxProducts = prods.map(
        ({ id, title, price, description, image }, index) => (
            <ProductCard
                key={index}
                id={id}
                title={title}
                price={price}
                description={description}
                image={image}
            />
        )
    );

    return (
        <div className="kshop-w-all-products-list">
            {jsxProducts}
        </div>
    );
};

export default WAllProductsList;
