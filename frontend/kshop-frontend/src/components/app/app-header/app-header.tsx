import * as React from 'react';
import "./app-header.sass";
import logo from "./logo_op_64.png"

interface IAppHeaderProps { }

const AppHeader: React.FunctionComponent<IAppHeaderProps> = (props) => {
    return (
        <div className="kshop-app-header">
                <a href="/"><img src={logo} alt="" /></a>        
        </div>
        
    )
};

export default AppHeader;