import * as React from 'react';
import "./app-content.sass";

interface IAppContentProps { }

const AppContent: React.FunctionComponent<IAppContentProps> = (props) => {
    return (
        <div className="kshop-app-content">
            {props.children}
        </div>
    )
};

export default AppContent;