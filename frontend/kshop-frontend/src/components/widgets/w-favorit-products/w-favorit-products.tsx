import ProductCard from "components/controls/product-card/product-card";
import { Money } from "models/Money";
import { ProductShort } from "models/ProductShort";
import * as React from "react";
import "./w-favorit-products.sass";

interface IWFavoritProductsProps {}

const WFavoritProducts: React.FunctionComponent<IWFavoritProductsProps> = (
    props
) => {
    const [products, setProducts] = React.useState<ProductShort[]>([]);
    React.useEffect(() => {
        const genProductsFunc = (count: number) =>
            Array.from(Array(count).keys()).map(
                (n) =>
                    new ProductShort(
                        n,
                        `Product ${n}`,
                        new Money(100),
                        "https://cdn1.ozone.ru/multimedia/wc250/1013639927.jpg",
                        `Description for product ${n}`
                    )
            );

        setProducts(genProductsFunc(10));
    }, []);

    const jsxFavoritProducts = products.map((fp, i) => (
        <ProductCard
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
