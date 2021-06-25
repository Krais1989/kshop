import * as React from "react";
import { toast } from "react-toastify";
import { ProductShort } from "../../../models/ProductShort";
import ProductCard from "../../controls/product-card/product-card";

import "./w-all-products-list.sass";

import "react-toastify/dist/ReactToastify.css";
import { Money } from "models/Money";
import { AppServices } from "components/app/app-services";

interface IWAllProductsListProps {}

const WAllProductsList: React.FunctionComponent<IWAllProductsListProps> = (
    props
) => {
    let prods: ProductShort[] = [];
    let page: number = 0;

    React.useEffect(() => {
        const result = AppServices.Clients.Products.getProductsForHome({
            page: 0,
        }).then((e) => {
            if (e.IsSuccess()){
                const resp = e.Data?.Data;
                e.Data.
            }
        });
    }, [page]);

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

    return <div className="kshop-w-all-products-list">{jsxProducts}</div>;
};

export default WAllProductsList;
