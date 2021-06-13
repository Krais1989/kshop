import "./home-page.sass";
import "../base-page/base-page.sass";

import * as React from "react";
import WAllProductsList from "components/widgets/w-all-products-list/w-all-products-list";
import { ButtonGreen } from "components/controls/button/button";
import { toast } from "react-toastify";

interface IHomePageProps {}

const HomePage: React.FunctionComponent<IHomePageProps> = (props) => {

    const asd = ()=>{ toast("asd"); };

    return (
        <div className="kshop-base-page kshop-home-page">
            <h1>HomePage</h1>
            <ButtonGreen onClick={asd} style={{width:"100%"}}>SHOW</ButtonGreen>
            <WAllProductsList />
            <button></button>
        </div>
    );
};

export default HomePage;
