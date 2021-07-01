import * as React from "react";
import { toast } from "react-toastify";
import { ProductPresentation } from "../../../models/ProductPresentation";
import ProductCard from "../../controls/product-card/product-card";

import "./w-all-products-list.sass";

import "react-toastify/dist/ReactToastify.css";
import { AppServices } from "components/app/app-services";

interface IWAllProductsListProps {}

const WAllProductsList: React.FunctionComponent<IWAllProductsListProps> = (
    props
) => {
    const [page, setPage] = React.useState(0);
    const [prods, setProds] = React.useState<ProductPresentation[]>([]);

    React.useEffect(() => {
        AppServices.Clients.Products.getProductsForHome({
            pageIndex: 0
        })
            .then((e) => {
                if (!e.ErrorMessage) {
                    setProds(e.data);
                } else {
                    toast.warning(`Bad request: ${e.ErrorMessage}`);
                }
            })
            .catch((e) => {
                toast.error(`Products for home loading error, page: ${page}`);
            });
    }, [page]);

    const jsxProducts = prods.map(
        ({ id, title, price, description, image }, index) => (
            <ProductCard
                key={id}
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
