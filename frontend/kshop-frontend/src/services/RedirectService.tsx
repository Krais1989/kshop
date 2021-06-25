import { createBrowserHistory } from "history";

const go = (path: string) => createBrowserHistory().push(path);
const back = () => createBrowserHistory().goBack();


const RedirectService = {
    go: go,
    back: back,

    toHome: () => go("/"),
    toOrders: () => go("/orders"),
    toProductDetail: (id: number) => go(`/catalog/products/${id}`),
    toCart: () => go("/cart"),
    toFavorits: () => go("/favorits"),
};

export default RedirectService;
