import { AppSettings } from "components/app/app-settings";
import {
    ICartsClient,
    GetCartResponse,
    SetCartPositionRequest,
    SetCartPositionResponse,
    RemoveCartPositionRequest,
    RemoveCartPositionResponse,
    ClearCartPositionResponse,
} from "../abstractions/ICartsClient";
import { HttpClient } from "./HttpClient";

export class CartsClient implements ICartsClient {
    private http: HttpClient = new HttpClient();

    get = async () => 
        await this.http.get<GetCartResponse>(`${AppSettings.CartsHost}/api/carts`);

    setPosition = async (req: SetCartPositionRequest) => 
        await this.http.post<SetCartPositionRequest, SetCartPositionResponse>(`${AppSettings.CartsHost}/api/carts/set-position`, req);

    removePosition = async (req: RemoveCartPositionRequest) => 
        await this.http.delete<RemoveCartPositionRequest, RemoveCartPositionResponse>(`${AppSettings.CartsHost}/api/carts/remove-position`, req);

    clear = async () => 
        await this.http.delete<undefined, ClearCartPositionResponse>(`${AppSettings.CartsHost}/api/carts/clear`);
}
