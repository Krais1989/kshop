import * as React from 'react';
import "./app-header.sass";
import logo from "./logo_op_64.png"

interface IAppHeaderProps { }

const AppHeader: React.FunctionComponent<IAppHeaderProps> = (props) => {
    return (
        <div className="kshop-app-header">
                <img src={logo} alt="" />
                <span className="kshop-app-header-text">HeaderBar</span>            
        </div>
        
    )
};

export default AppHeader;