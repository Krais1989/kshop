import React from "react";
import "./app.sass";

import "styles/button.sass"
import "styles/select.sass"
import "styles/checkbox.sass"

import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    Redirect,
} from "react-router-dom";
import HomePage from "../pages/home-page/home-page";
import CatalogPage from "../pages/catalog-page/catalog-page";
import Navbar from "../widgets/navbar/navbar";
import AppContent from "./app-content/app-content";
import AppFooter from "./app-footer/app-footer";
import AppHeader from "./app-header/app-header";
import ProductDetailsPage from "../pages/product-details-page/product-details-page";
import OrdersPage from "../pages/orders-page/orders-page";
import AccountPage from "components/pages/account-page/account-page";
import FavoritesPage from "components/pages/favorites-page/favorites-page";
import ShoppingCartPage from "components/pages/shopping-cart-page/shopping-cart-page";

class DataContext {
}

const App:React.FC = () => {
    return (
        <div className="kshop-app">
            <Router>
                <AppHeader />
                <Navbar />
                <AppContent>
                    <Switch>
                        <Route exact path="/" component={HomePage} />
                        <Route exact path="/catalog" component={CatalogPage} />
                        <Route
                            exact
                            path="/catalog/group/:id"
                            component={CatalogPage}
                        />
                        <Route exact path="/catalog/products/:id" component={ProductDetailsPage} />

                        <Route exact path="/account" component={AccountPage} />
                        <Route exact path="/orders" component={OrdersPage} />
                        <Route exact path="/favorites" component={FavoritesPage} />
                        <Route exact path="/cart" component={ShoppingCartPage} />
                        <Redirect to="/" />
                    </Switch>
                </AppContent>
                <AppFooter />
            </Router>
        </div>
    );
}

export default App;
