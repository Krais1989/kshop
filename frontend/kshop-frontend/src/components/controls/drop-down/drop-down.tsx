import * as React from 'react';
import { useState } from 'react';
import "./drop-down.sass";

interface IDropDownProps {
    title?:string;
    children: React.ReactNode
}

const DropDown: React.FunctionComponent<IDropDownProps> = (props) => {

    let [isShow, setIsShow] = useState(false);


    let cssContent = `k-drop-down-ctrl-content${isShow?"-show":""}`;
    console.log(isShow);
    console.log(cssContent);

    return (
        <div className="k-drop-down-ctrl">
            <button className="k-drop-down-ctrl-btn" onClick={()=>setIsShow(!isShow)} >{props.title}</button>
            <div className={cssContent}>
                {props.children}
            </div>
        </div>
    )
};

export default DropDown;