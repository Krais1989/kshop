import * as React from 'react';
import "./base-page.sass";

interface IBasePageProps { }

const BasePage: React.FunctionComponent<IBasePageProps> = (props) => {
    return (
        <div className={'kshop-base-page ${props.className}'}>
            {props.children}
        </div>
    )
};

export default BasePage;