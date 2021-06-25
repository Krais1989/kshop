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

    let cssContent = `k-drop-down-content${isShow?"-show":""}`;
    return (
        <div className={`k-drop-down ${props.className?props.className:""}`}>
            <button className="k-drop-down-btn" onClick={()=>setIsShow(!isShow)} >{props.title}</button>
            <div className={cssContent}>
                {props.children}
            </div>
        </div>
    )
};

export default DropDown;