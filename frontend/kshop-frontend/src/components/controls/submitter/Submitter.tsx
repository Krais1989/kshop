import * as React from "react";
import { useState } from "react";
import "./Submitter.sass";
import imgLoad from "./loading.gif";

interface ISubmitterProps {
    submit: () => Promise<any>;
    loader?: () => JSX.Element;
    //callback: () => Promise<any>;
    //children: any;
}

const Submitter: React.FunctionComponent<ISubmitterProps> = (props) => {
    const [isLoad, setIsLoad] = useState(false);

    // function onClickWrapper(e: React.MouseEvent<HTMLButtonElement, React.MouseEvent>): void {
    //     submitCallback();
    // }
    const submitCallback = () => {
        setIsLoad(true);
        props.submit().finally(() => {
            setIsLoad(false);
        });
    };

    const jsxLoadingPlaceholder = props.loader ?? (
        <div className="kshop-submitter-loading">
            <img src={imgLoad} alt={"loading"}></img>
        </div>
    );

    /* Клонирование дочернего элемента с переопределением onClick (кнопка) */
    const jsxChildren = React.Children.map(props.children, (child) => {
        if (React.isValidElement(child)) {
            return React.cloneElement(child, { onClick: (e:any) => {submitCallback()} });
        }
        return child;
    });

    const jsxSubmitter = jsxChildren;
    const jsxContent = isLoad ? jsxLoadingPlaceholder : jsxSubmitter;

    return <div className="kshop-submitter">{jsxContent}</div>;
};

export default Submitter;
