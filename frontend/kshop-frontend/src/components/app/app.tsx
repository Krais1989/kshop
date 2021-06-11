import React from "react";
import "./app.sass";

import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    Redirect,
} from "react-router-dom";
import HomePage from "../pages/home-page/home-page";
import CatalogPage from "../pages/catalog-page/catalog-page";

import Navbar from "../navbar/navbar";
import AppContent from "./app-content/app-content";
import AppFooter from "./app-footer/app-footer";
import AppHeader from "./app-header/app-header";

function App() {
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
                        <Route exact path="/catalog/product/:id" />
                        <Redirect to="/" />
                    </Switch>
                </AppContent>
                <AppFooter />
            </Router>
        </div>
    );
}

export default App;
