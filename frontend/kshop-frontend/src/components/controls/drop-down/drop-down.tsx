import * as React from 'react';
import { useState } from 'react';
import "./drop-down.sass";

interface IDropDownProps {
    title?:string;
    children: React.ReactNode,
    className?: string
}

const DropDown: React.FunctionComponent<IDropDownProps> = (props) => {

    let [isShow, setIsShow] = useState(false);

    let cssContent = `ks-dropdown-content${isShow?"-show":""}`;
    return (
        <div className={`ks-dropdown ${props.className?props.className:""}`}>
            <button className="ks-dropdown-btn" onClick={()=>setIsShow(!isShow)} >{props.title}</button>
            <div className={cssContent}>
                {props.children}
            </div>
        </div>
    )
};

export default DropDown;