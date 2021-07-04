import { useCart } from "components/providers/CartProvider";
import { Money } from "models/Money";
import { ProductDetails } from "models/ProductDetails";
import * as React from "react";
import { ProductsClient } from "services/clients/ProductsClient";

import "./w-product-details.sass";

//import logo from "https://www.regard.ru/photo/goods/363952.png";

interface IWProductDetailsProps {
    productID: number;
}

const WProductDetails: React.FunctionComponent<IWProductDetailsProps> = (
    props
) => {
    const [details, setDetails] = React.useState<ProductDetails | null>(null);
    const { getQuantity, addToCart } = useCart();

    const { productID: productID } = props;

    React.useEffect(() => {
        // let product_details = new ProductDetails(
        //     productId,
        //     `Product ${productId}`,
        //     100,
        //     undefined,
        //     `[Description for ${productId}] Lorem ipsum dolor, sit amet consectetur adipisicing elit. Fugiat, nihil reiciendis quam non vero esse perferendis. In ex dignissimos ab!`
        // );

        // details.attributes = [
        //     new ProductAttribute("Идентификатор", details.id?.toString()),
        //     new ProductAttribute("Название", details?.title),
        //     new ProductAttribute("Цена", `${details.price} руб.`),
        // ];

        ProductsClient.getProductsDetails({
            productID: [productID],
        }).then((r) => {
            if (r.isSuccess) {
                const d = r.data.length > 0 ? r.data[0] : null;
                setDetails(d);
            } else {
            }
        });
    }, [productID]);

    if (details === null) {
        return <h2>Product not found</h2>;
    }

    const jsxAttrs = details.attributes?.map((pa, index) => (
        <div
            key={pa.id}
            className="kshop-w-product-details-info-attributes-row"
        >
            <span>{pa.title}</span>
            <span>{pa.value}</span>
        </div>
    ));

    return (
        <div className="kshop-w-product-details">
            <div className="kshop-w-product-details-title">
                <span>
                    {details.title} #{details.id}
                </span>
            </div>
            <div className="kshop-w-product-details-topbar"></div>

            <div className="kshop-w-product-details-flex">
                <div className="kshop-w-product-details-galery">
                    <img
                        alt={details.title}
                        src={
                            details.image ??
                            "https://www.regard.ru/photo/goods/363952.png"
                        }
                    />
                    {/* {product_details.image ?? "No Image"} */}
                </div>
                <div className="kshop-w-product-details-price">
                    <span className="kshop-w-product-details-price-original">
                        {details.price?.price} {details.price?.currency};
                    </span>
                    {getQuantity(details.id) === 0 ? (
                        <button
                            onClick={(e) =>
                                addToCart([
                                    {
                                        productID: details.id,
                                        quantity: 1,
                                        title: details.title,
                                        checked: false,
                                        price: details.price,
                                    },
                                ])
                            }
                            className="kshop-button-blue"
                        >
                            Add to cart
                        </button>
                    ) : (
                        <h2>In cart</h2>
                    )}
                </div>
                <div className="kshop-w-product-details-info">
                    <div className="kshop-w-product-details-info-attributes">
                        {jsxAttrs}
                    </div>
                    <div className="kshop-w-product-details-info-description">
                        {details.description}
                    </div>
                </div>
            </div>

            {/* {jsxAttrs} */}
        </div>
    );
};

export default WProductDetails;
