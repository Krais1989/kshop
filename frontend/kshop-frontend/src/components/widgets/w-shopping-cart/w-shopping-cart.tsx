import { ProductShort } from "models/ProductShort";
import * as React from "react";
import { toast } from "react-toastify";
import { useAuth } from "components/contexts/AuthContext";
import "./w-shopping-cart.sass";
import { useCart } from "components/contexts/CartContext";
import { Cart, CartPosition } from "models/Cart";

interface IWShoppingCartProps {}

const WShoppingCart: React.FunctionComponent<IWShoppingCartProps> = (props) => {
    const { cart, setCart, removeFromCart, setQuantity, setChecked } =
        useCart();

    const removePosition = (ids: Array<number>) => {
        removeFromCart(ids);
    };

    // const setQuantity = (pos: CartPosition, quantity: number) => {
    //     pos.quantity = quantity;
    //     const newCart = { ...cart } as Cart;
    //     setCart(newCart);
    // };

    // React.useEffect(() => {
    //     // Загрузить данные с сервера
    //     // Преобразовать в массив CartPositions
    //     // Вызвать setPositions с новыми значениями
    //     // return () => {
    //     // }
    //     let positions: CartPosition[] = [];

    //     for (let i = 0; i < 10; i++) {
    //         let p = new CartPosition(
    //             i,
    //             1,
    //             false,
    //             `Product ${i}`,
    //             100,
    //             "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae libero facere eum suscipit accusantium repellat",
    //             "https://cdn1.ozone.ru/multimedia/wc100/1015663388.jpg"
    //         );
    //         positions.push(p);
    //     }
    //     setPositions(positions);
    // }, [auth.userId]);

    const jsxPositions = cart.positions.map((cartPos, index) => (
        <div
            key={cartPos.productID}
            className="kshop-w-shopping-cart-positions-row"
        >
            <div className="kshop-w-shopping-cart-positions-row-check">
                <input
                    type="checkbox"
                    className="kshop-checkbox"
                    checked={cartPos.checked}
                    onChange={(e) =>
                        setChecked(cartPos.productID, e.target.checked)
                    }
                />
            </div>
            <div className="kshop-w-shopping-cart-positions-row-image">
                <img src={cartPos.image} alt="" />
            </div>
            <div className="kshop-w-shopping-cart-positions-row-info">
                <div className="kshop-w-shopping-cart-positions-row-info-title">
                    {cartPos.title} #{cartPos.productID}
                </div>
                <div className="kshop-w-shopping-cart-positions-row-info-description">
                    {cartPos.description}
                </div>
            </div>
            <div className="kshop-w-shopping-cart-positions-row-actions">
                <button
                    className="kshop-button-red"
                    onClick={() => removePosition([cartPos.productID])}
                >
                    Delete
                </button>
            </div>
            <div className="kshop-w-shopping-cart-positions-row-price">
                <span>
                    {cartPos.price.price} {cartPos.price.currency} &#8381;
                </span>
            </div>

            <div className="kshop-w-shopping-cart-positions-row-quantity">
                <select
                    className="kshop-select"
                    value={cartPos.quantity.toString()}
                    onChange={(ev: React.ChangeEvent<HTMLSelectElement>) =>
                        setQuantity(cartPos.productID, Number(ev.target.value))
                    }
                >
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5">5</option>
                    <option value="6">6</option>
                    <option value="7">7</option>
                    <option value="8">8</option>
                    <option value="9">9</option>
                    <option value="10">10</option>
                </select>
            </div>
        </div>
    ));

    return (
        <div className="kshop-w-shopping-cart">
            <div className="kshop-w-shopping-cart-positions">
                {jsxPositions}
            </div>
            <div className="kshop-w-shopping-cart-actions">
                <div className="kshop-w-shopping-cart-actions-priceinfo">
                    Price: 3334 &#8381;
                </div>
                <button className="kshop-button">Submit Order</button>
            </div>
        </div>
    );
};

export default WShoppingCart;
