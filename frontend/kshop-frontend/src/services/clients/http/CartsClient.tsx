import { AppSettings } from "components/app/app-settings";
import {
    ICartsClient,
    GetCartResponse,
    SetCartPositionsRequest,
    SetCartPositionResponse,
    RemoveCartPositionRequest,
    RemoveCartPositionResponse,
    ClearCartPositionResponse,
} from "../abstractions/ICartsClient";
import { HttpClient } from "./HttpClient";

export class CartsClient implements ICartsClient {
    private http: HttpClient = new HttpClient();

    current = async () => 
        await this.http.get<GetCartResponse>(`${AppSettings.CartsHost}/api/carts/current`);

    setPosition = async (req: SetCartPositionsRequest) => 
        await this.http.post<SetCartPositionsRequest, SetCartPositionResponse>(`${AppSettings.CartsHost}/api/carts/set-positions`, req);

    removePosition = async (req: RemoveCartPositionRequest) => 
        await this.http.delete<RemoveCartPositionRequest, RemoveCartPositionResponse>(`${AppSettings.CartsHost}/api/carts/remove-positions`, req);

    clear = async () => 
        await this.http.delete<undefined, ClearCartPositionResponse>(`${AppSettings.CartsHost}/api/carts/clear`);
}
