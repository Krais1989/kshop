import { AppServices } from "components/app/app-services";
import { Cart, CartPosition } from "models/Cart";
import { createContext, useContext, useEffect, useState } from "react";
import { useAuth } from "./AuthContext";

class CartContextData {
    cart: Cart = new Cart();
    setCart: React.Dispatch<React.SetStateAction<Cart>> = (c) => {};
    addToCart: (pos: Array<CartPosition>) => void = (p) => {};
    removeFromCart: (ids: Array<number>) => void = (i) => {};
    clearCart: () => void = () => {};

    getQuantity: (prodId: number) => number = (prodId) => 0;
    setQuantity: (prodId: number, quantity: number) => void = (p, q) => {};

    setChecked: (prodId: number, value:boolean) => void = (p,v) =>{};
}

export const CartContext = createContext<CartContextData>(
    new CartContextData()
);

export const useCart = () => useContext(CartContext);

interface IProps {}

export const CartProvider: React.FunctionComponent<IProps> = (props) => {
    const { auth, isAuthenticated } = useAuth();

    const rawCart = localStorage.getItem("cart");
    let defCart = new Cart();
    if (rawCart !== null) defCart = Object.assign(defCart, JSON.parse(rawCart));

    const [cart, setCart] = useState<Cart>(defCart);

    const loadLocalCart = () => {
        let rawCart = localStorage.getItem("cart");
        if (!rawCart) {
            rawCart = JSON.stringify(new Cart());
            localStorage.setItem("cart", rawCart);
        }
        const newCart = Object.assign(new Cart(), JSON.parse(rawCart));
        setCart(newCart);
    };

    const getPositionQuantity = (prodId: number) => {
        let findPos = cart.positions.find((e) => e.productID === prodId);
        if (!findPos) return 0;
        else return findPos.quantity;
    };
    const setPositionQuantity = (prodId: number, quantity: number) => {
        let findPos = cart.positions.find((e) => e.productID === prodId);
        if (!findPos) return;
        findPos.quantity = quantity;

        addToCart([findPos]);
    };

    const setPositionChecked = (prodId: number, value: boolean) => {
        let findPos = cart.positions.find((e) => e.productID === prodId);
        if (!findPos) return;
        findPos.checked = value;
        addToCart([findPos]);
    };
    

    const addToCart = (posRange: Array<CartPosition>) => {
        const newCart: Cart = Object.assign(new Cart(), cart);
        newCart.addRange(posRange);
        localStorage.setItem("cart", JSON.stringify(newCart));
        if (isAuthenticated()) {
            AppServices.Clients.Carts.setPositions({
                positions: posRange,
            });
        }
        loadLocalCart();
    };

    const removeFromCart = (prodsIds: Array<number>) => {
        const newCart: Cart = Object.assign(new Cart(), cart);
        newCart.removeRange(prodsIds);
        localStorage.setItem("cart", JSON.stringify(newCart));
        if (isAuthenticated()) {
            AppServices.Clients.Carts.removePosition({
                productIDs: prodsIds,
            });
        }
        loadLocalCart();
    };

    const clearCart = () => {
        localStorage.removeItem("cart");
        if (isAuthenticated()) {
            AppServices.Clients.Carts.clear();
        }
        loadLocalCart();
    };

    useEffect(() => {
        if (isAuthenticated()) {
            AppServices.Clients.Carts.current()
                .then((r) => {
                    if (!r.ErrorMessage) {
                        localStorage.setItem("cart", JSON.stringify(r.data));
                        loadLocalCart();
                    }
                })
                .catch((err) => {
                    if (!err.ErrorMessage) loadLocalCart();
                });
        } else {
            loadLocalCart();
        }
    }, [auth, isAuthenticated]);

    return (
        <CartContext.Provider
            value={{
                cart: cart,
                setCart: setCart,
                addToCart: addToCart,
                removeFromCart: removeFromCart,
                clearCart: clearCart,
                getQuantity: getPositionQuantity,
                setQuantity: setPositionQuantity,
                setChecked: setPositionChecked
            }}
        >
            {props.children}
        </CartContext.Provider>
    );
};
