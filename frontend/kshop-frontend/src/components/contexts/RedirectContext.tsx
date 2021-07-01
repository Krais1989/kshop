import React, { createContext, useContext } from "react";
import { useHistory } from "react-router-dom";

class RedirectContextData {
    goto: (path: string) => void = () => {};
    toHome: () => void = () => this.goto("");
    toAccount: () => void = () => this.goto("");
    toProductDetails: (productId: number) => void = (productId: number) =>
        this.goto(`/products/details/${productId}`);
    toMyOrders: () => void = () => this.goto("my-orders");
    toOrdering: () => void = () => this.goto("ordering");
    toFavorits: () => void = () => this.goto("favorits");
    toCart: () => void = () => this.goto("cart");
}

export const RedirectContext = createContext<RedirectContextData>(
    new RedirectContextData()
);
export const useRedirect = () => useContext(RedirectContext);

export const RedirectProvider: React.FC = (props) => {
    const history = useHistory();
    const rcd = Object.assign(new RedirectContextData(), {
        goto: (path: string) => history.push(path)
    });
    //rcd.goto = (path: string) => history.push(path);
    // rcd.toOrdering = () => history.push("ordering");

    return (
        <RedirectContext.Provider value={rcd}>
            {props.children}
        </RedirectContext.Provider>
    );
};
