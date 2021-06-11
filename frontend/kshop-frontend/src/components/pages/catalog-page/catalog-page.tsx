import * as React from 'react';
import "./catalog-page.sass";

interface ICatalogPageProps { }

const CatalogPage: React.FunctionComponent<ICatalogPageProps> = (props) => {
    console.log(props.children);
    return (
        <h1>CatalogPage</h1>
    )
};

export default CatalogPage;