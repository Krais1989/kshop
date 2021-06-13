import * as React from 'react';
import "./product-details-page.sass";

import { useParams } from 'react-router-dom';
import WProductDetails from '../../widgets/w-product-details/w-product-details';

export interface IProductDetailsPageProps {
}

interface IRouteParams {
    id: string;
}

const ProductDetailsPage: React.FC<IProductDetailsPageProps> = (props) => {
    
    const {id} = useParams<IRouteParams>();
    
    return (
            <div className="kshop-base-page kshop-product-details-page">
                <h1>Product Details Page {Number(id)}</h1>
                <WProductDetails productId = {Number(id)} />
            </div>
    )
};

export default ProductDetailsPage;