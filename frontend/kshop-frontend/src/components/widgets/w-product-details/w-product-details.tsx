import { AppServices } from "components/app/app-services";
import { ProductDetails, ProductAttribute } from "models/ProductDetails";
import * as React from "react";

import "./w-product-details.sass";

//import logo from "https://www.regard.ru/photo/goods/363952.png";

interface IWProductDetailsProps {
    productID: number;
}

const WProductDetails: React.FunctionComponent<IWProductDetailsProps> = (
    props
) => {
    const [details, setDetails] = React.useState<ProductDetails>(new ProductDetails());

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

        AppServices.Clients.Products.getProductDetails({
            productID: productID,
        }).then((r) => {
            if (!r.ErrorMessage) {
                setDetails(r.data);
            } else {
            }
        });
    }, [productID]);

    if (!details || Object.keys(details).length === 0) {
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
            <div className="kshop-w-product-details-topbar">Topbar</div>

            <div className="kshop-w-product-details-flex">
                <div className="kshop-w-product-details-galery">
                    <img src="https://www.regard.ru/photo/goods/363952.png" />
                    {/* {product_details.image ?? "No Image"} */}
                </div>
                <div className="kshop-w-product-details-price">
                    <span className="kshop-w-product-details-price-original">
                        {details.price?.price} {details.price?.currency};
                    </span>
                    <button className="kshop-button-blue">Add to cart</button>
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
