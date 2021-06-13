import * as React from 'react';
import "./button.sass";


export const Button = (props:React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>) => {
    return (
        <button className={`kshop-button ${props.className}`} onClick={props.onClick}>
            {props.children}
        </button>
    );
};

export const ButtonGreen = (props:React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>) => {
    return (
        <button className={`kshop-button-green ${props.className}`} onClick={props.onClick}>
            {props.children}
        </button>
    );
};

export const ButtonBlue = (props:React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>) => {
    return (
        <button className={`kshop-button-blue ${props.className}`} onClick={props.onClick}>
            {props.children}
        </button>
    );
};

export const ButtonRed = (props:React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>) => {
    return (
        <button className={`kshop-button-red ${props.className}`} onClick={props.onClick}>
            {props.children}
        </button>
    );
};

export const ButtonGray = (props:React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>) => {
    return (
        <button className={`kshop-button-gray ${props.className}`} onClick={props.onClick}>
            {props.children}
        </button>
    );
};

export const ButtonBlack = (props:React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>) => {
    return (
        <button className={`kshop-button-black ${props.className}`} onClick={props.onClick}>
            {props.children}
        </button>
    );
};

export default Button;