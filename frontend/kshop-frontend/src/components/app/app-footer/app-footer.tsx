import * as React from 'react';
import "./app-footer.sass";

interface IAppFooterProps { }

const AppFooter: React.FunctionComponent<IAppFooterProps> = (props) => {
    return (

        <div className="kshop-app-footer">
            <h2>Footer Bar</h2>
        </div>
    )
};

export default AppFooter;