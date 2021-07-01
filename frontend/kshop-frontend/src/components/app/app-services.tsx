import { AuthService, IAuthService } from "services/AuthService";
import { ICartsClient } from "services/clients/abstractions/ICartsClient";
import { IIdentityClient } from "services/clients/abstractions/IIdentityClient";
import { IOrdersClient } from "services/clients/abstractions/IOrdersClient";
import { IProductsClient } from "services/clients/abstractions/IProductsClient";
import { CartsClient } from "services/clients/http/CartsClient";
import { IdentityClient } from "services/clients/http/IdentityClient";
import { OrdersClient } from "services/clients/http/OrdersClient";
import { ProductsClient } from "services/clients/http/ProductsClient";

interface IAppClients {
    Identity: IIdentityClient;
    Orders: IOrdersClient;
    Products: IProductsClient;
    Carts: ICartsClient;
}

class HttpAppClientsType implements IAppClients {
    Identity: IIdentityClient = new IdentityClient();
    Orders: IOrdersClient = new OrdersClient();
    Products: IProductsClient = new ProductsClient();
    Carts: ICartsClient = new CartsClient();
}

interface IAppServices {
    Clients: IAppClients;
    Auth: IAuthService;
}

class AppServicesClass implements IAppServices {
    Clients: IAppClients = new HttpAppClientsType();
    Auth: IAuthService = new AuthService();
}

export const AppServices: AppServicesClass = new AppServicesClass();
