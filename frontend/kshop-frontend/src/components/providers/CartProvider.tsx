import { Cart, CartPosition } from "models/Cart";
import { createContext, useContext, useEffect, useState } from "react";
import { toast } from "react-toastify";
import { CartsClient } from "services/clients/CartsClient";
import { useAuth } from "./AuthProvider";

class CartContextData {
    cart: Cart = new Cart();
    addToCart: (pos: Array<CartPosition>) => void = (p) => {};
    removeFromCart: (ids: Array<number>) => void = (i) => {};
    clearCart: () => void = () => {};

    getQuantity: (prodId: number) => number = (prodId) => 0;
    setQuantity: (prodId: number, quantity: number) => void = (p, q) => {};
    setChecked: (prodId: number, value: boolean) => void = (p, v) => {};
}

export const CartContext = createContext<CartContextData>(new CartContextData());

export const useCart = () => useContext(CartContext);

interface IProps {}

export const CartProvider: React.FunctionComponent<IProps> = (props) => {
    const { auth, isAuthenticated } = useAuth();

    const [cart, setCart] = useState<Cart>(new Cart());

    const getLocal = () => {
        const local = localStorage.getItem("cart");
        if (local === null) return new Cart();
        return Object.assign(new Cart(), JSON.parse(local)) as Cart;
    };

    const saveLocal = (value: Cart) => {
        localStorage.setItem("cart", JSON.stringify(value));
    };

    const saveRemote = async (value: Cart) =>
        await CartsClient.setPositions({ positions: cart.positions, useMerge: false });

    const saveBoth = (value: Cart) => {
        saveLocal(value);
        if (isAuthenticated()) saveRemote(value);
    };

    const getRemote = async (onSuccess: (c: Cart) => void, onError: (msg: string) => void) =>
        await CartsClient.current()
            .then((r) => {
                if (r.isSuccess) {
                    const newCart = Object.assign(new Cart(), r.data) as Cart;
                    onSuccess(newCart);
                } else {
                    onError(r.errorMessage ?? "");
                }
            })
            .catch((err) => {
                console.error(
                    `Auth Exception: Cart nullified > Carts client error: ${err.Message}`
                );
                onError(err.Message);
            });

    const getPositionQuantity = (prodId: number) => {
        const findPos = cart?.positions.find((e) => e.productID === prodId);
        if (!findPos) return 0;
        else return findPos.quantity;
    };

    const setPositionQuantity = (prodId: number, quantity: number) => {
        const findPos = cart?.positions.find((e) => e.productID === prodId);
        if (!findPos) return;
        findPos.quantity = quantity;
        addToCart([findPos]);
    };

    const setPositionChecked = (prodId: number, value: boolean) => {
        let findPos = cart?.positions.find((e) => e.productID === prodId);
        if (!findPos) return;
        findPos.checked = value;
        addToCart([findPos]);
    };

    const addToCart = (posRange: Array<CartPosition>) => {
        const new_cart: Cart = Object.assign(new Cart(), cart) as Cart;
        new_cart.addRange(posRange);
        saveBoth(new_cart);
        setCart(new_cart);
    };

    const removeFromCart = (prodsIds: Array<number>) => {
        const new_cart: Cart = Object.assign(new Cart(), cart) as Cart;
        new_cart.removeRange(prodsIds);
        saveBoth(new_cart);
        setCart(new_cart);
    };

    const clearCart = () => {
        localStorage.removeItem("cart");
        if (isAuthenticated()) CartsClient.clear();
        setCart(new Cart());
    };

    useEffect(() => {
        if (isAuthenticated()) {
            getRemote(
                (r) => {
                    saveLocal(r);
                    setCart(r);
                },
                (err) => {
                    toast.error(err);
                    setCart(getLocal());
                }
            );
        } else {
            setCart(getLocal());
        }
    }, [auth]);

    return (
        <CartContext.Provider
            value={{
                cart: cart,
                addToCart: addToCart,
                removeFromCart: removeFromCart,
                clearCart: clearCart,
                getQuantity: getPositionQuantity,
                setQuantity: setPositionQuantity,
                setChecked: setPositionChecked,
            }}
        >
            {props.children}
        </CartContext.Provider>
    );
};
