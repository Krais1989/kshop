import { ProductShort } from "models/ProductShort";
import * as React from "react";
import { toast } from "react-toastify";
import "./w-shopping-cart.sass";

interface IWShoppingCartProps {}

class CartView {
    positions?: CartPosition[];
}

class CartPosition {
    productId: number;
    quantity: number;
    checked: boolean;
    title: string;
    price: number;
    description?: string;
    image?: string;

    constructor(
        productId: number,
        quantity: number,
        checked: boolean,
        title: string,
        price: number,
        description?: string,
        image?: string
    ) {
        this.productId = productId;
        this.quantity = quantity;
        this.checked = checked;
        this.title = title;
        this.description = description;
        this.image = image;
        this.price = price;
    }
}

const WShoppingCart: React.FunctionComponent<IWShoppingCartProps> = (props) => {
    //let data: CartPosition[] = [];

    const [positions, setPositions] = React.useState<CartPosition[]>([]);
    const removePosition = (delPos: CartPosition) => {
        setPositions((oldPos) => [...oldPos.filter((op) => op !== delPos)]);
    };
    const setChecked = (pos: CartPosition, val: boolean) => {
        setPositions((oldPos: CartPosition[]) => {
            let ccc: CartPosition = oldPos.filter((op) => op === pos)[0];
            if (ccc) ccc.checked = val;
            return [...oldPos];
        });
    };
    const setQuantity = (pos: CartPosition, quantity: number) => {
        setPositions((oldPos: CartPosition[]) => {
            let ccc: CartPosition = oldPos.filter((op) => op === pos)[0];
            if (ccc) ccc.quantity = quantity;
            return [...oldPos];
        });
    };

    React.useEffect(() => {
        // Загрузить данные с сервера
        // Преобразовать в массив CartPositions
        // Вызвать setPositions с новыми значениями
        // return () => {
        // }
        let positions: CartPosition[] = [];

        for (let i = 0; i < 10; i++) {
            let p = new CartPosition(
                i,
                2,
                false,
                `Product ${i}`,
                100,
                "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae libero facere eum suscipit accusantium repellat",
                "https://cdn1.ozone.ru/multimedia/wc100/1015663388.jpg"
            );
            positions.push(p);
        }
        setPositions(positions);
    }, []);

    toast('Render');

    const jsxPositions = positions.map((cartPos, index) => (
        <div key={cartPos.productId} className="kshop-w-shopping-cart-positions-row">
            <div className="kshop-w-shopping-cart-positions-row-check">
                <input
                    type="checkbox"
                    className="kshop-checkbox"
                    checked={cartPos.checked}
                    onChange={() => setChecked(cartPos, !cartPos.checked)}
                />
            </div>
            <div className="kshop-w-shopping-cart-positions-row-image">
                <img src={cartPos.image} alt="" />
            </div>
            <div className="kshop-w-shopping-cart-positions-row-info">
                <div className="kshop-w-shopping-cart-positions-row-info-title">
                    {cartPos.title}
                </div>
                <div className="kshop-w-shopping-cart-positions-row-info-description">
                    {cartPos.description}
                </div>
            </div>
            <div className="kshop-w-shopping-cart-positions-row-actions">
                <button
                    className="kshop-button-red"
                    onClick={() => removePosition(cartPos)}
                >
                    Delete
                </button>
            </div>
            <div className="kshop-w-shopping-cart-positions-row-price">
                <span>{cartPos.price} &#8381;</span>
            </div>

            <div className="kshop-w-shopping-cart-positions-row-quantity">
                <select
                    className="kshop-select"
                    value={cartPos.quantity.toString()}
                    onChange={(ev: React.ChangeEvent<HTMLSelectElement>) =>
                        setQuantity(cartPos, Number(ev.target.value))}
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

    return <div className="kshop-w-shopping-cart">
        <div className="kshop-w-shopping-cart-positions">{jsxPositions}</div>
        <div className="kshop-w-shopping-cart-actions">
            <div className="kshop-w-shopping-cart-actions-priceinfo">Price: 3334 &#8381;</div>
            <button className="kshop-button">Submit Order</button>
        </div>
        
    </div>;
};

export default WShoppingCart;
