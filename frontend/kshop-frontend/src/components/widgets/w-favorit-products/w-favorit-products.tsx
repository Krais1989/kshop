import ProductCard from "components/controls/product-card/product-card";
import { Money } from "models/Money";
import { ProductPresentation } from "models/ProductPresentation";
import * as React from "react";
import "./w-favorit-products.sass";

interface IWFavoritProductsProps {}

const WFavoritProducts: React.FunctionComponent<IWFavoritProductsProps> = (
    props
) => {
    const [products, setProducts] = React.useState<ProductPresentation[]>([]);
    React.useEffect(() => {
        const genProductsFunc = (count: number) =>
            Array.from(Array(count).keys()).map(
                (n) =>
                {
                    let pp:ProductPresentation = {
                        id: n,
                        title: `Product ${n}`,
                        price: new Money(0),
                        image: "https://cdn1.ozone.ru/multimedia/wc250/1013639927.jpg",
                        description: `Description for product ${n}`,
                    }
                    return pp;
                }
                    
            );

        setProducts(genProductsFunc(10));
    }, []);

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

export default WFavoritProducts;
