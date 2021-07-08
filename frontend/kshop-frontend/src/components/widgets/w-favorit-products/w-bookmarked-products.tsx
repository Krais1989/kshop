import ProductCard from "components/controls/product-card/product-card";
import { useAuth } from "components/providers/AuthProvider";
import { Money } from "models/Money";
import { ProductPresentation } from "models/ProductPresentation";
import * as React from "react";
import { ProductsClient } from "services/clients/ProductsClient";
import "./w-bookmarked-products.sass";

interface IWFavoritProductsProps {}

const WBookmarkedProducts: React.FunctionComponent<IWFavoritProductsProps> = (
    props
) => {
    const [products, setProducts] = React.useState<ProductPresentation[]>([]);
    const {auth} = useAuth();

    React.useEffect(() => {
        const getProductsBookmarked = () => ProductsClient.getProductsBookmarked().then(r=>{
            setProducts(r.data);
        });
        getProductsBookmarked();
    }, [auth]);

    const jsxFavoritProducts = products.map((fp, i) => (
        <ProductCard
            key={fp.id}
            id={fp.id}
            title={fp.title}
            price={fp.price}
            description={fp.description}
            image={fp.image}
        />
    ));

    return <div className="kshop-w-favorit-products">{jsxFavoritProducts}</div>;
};

export default WBookmarkedProducts;
