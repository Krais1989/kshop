import * as React from "react";
import { toast } from "react-toastify";
import { ProductPresentation } from "../../../models/ProductPresentation";
import ProductCard from "../../controls/product-card/product-card";

import "./w-all-products-list.sass";

import "react-toastify/dist/ReactToastify.css";
import { ProductsClient } from "services/clients/ProductsClient";
import { useCallback } from "react";

interface IProps {
    categoryId: number;
}

class State {
    items: ProductPresentation[] = [];
    nextPage: number = 0;
    hasMore: boolean = true;
    hasError: boolean = false;
}

const WAllProductsList: React.FunctionComponent<IProps> = (props) => {
    const [state, setState] = React.useState<State>(new State());
    const categoryId = props.categoryId;
    
    /* при смене categoryId необходимо сбросить состояние загрузки продуктов */
    React.useEffect(()=>{
        state.items = [];
        state.hasError = false;
        state.hasMore = true;
        state.nextPage = 0;
    }, [categoryId]);

    React.useEffect(() => {
        /* hasMore */
        let { nextPage, items, hasMore, hasError } = state;
        if (hasMore && !hasError) {
            console.log(`Loading page: ${nextPage} hasMore:${hasMore} hasError:${hasError}`);
            ProductsClient.getProductsForHome({
                pageIndex: nextPage,
                categoryID: categoryId
            })
                .then((e) => {
                    if (e.isSuccess) {
                        const hasData = e.data.length > 0;
                        hasMore = hasData;

                        if (hasData) {
                            nextPage++;
                            items = [...items, ...e.data];
                        }
                    } else {
                        toast.warning(`Bad request: ${e.errorMessage}`);
                        hasError = true;
                    }
                })
                .catch((e) => {
                    toast.error(`Products for home loading error, page: ${nextPage}`);
                    hasError = true;
                })
                .finally(() => {
                    setState({
                        items: items,
                        hasError: hasError,
                        hasMore: hasMore,
                        nextPage: nextPage,
                    });
                });
        }
    }, [state, categoryId]);

    const jsxProducts = state.items.map(({ id, title, price, description, image }, index) => (
        <ProductCard
            key={id}
            id={id}
            title={title}
            price={price}
            description={description}
            image={image}
        />
    ));

    return <div className="kshop-w-all-products-list">{jsxProducts}</div>;
};

export default WAllProductsList;
