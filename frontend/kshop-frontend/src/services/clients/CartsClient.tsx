import { AppSettings } from "components/app/AppSettings";
import { AuthService } from "services/AuthService";
import {
    GetCartResponse,
    SetCartPositionsRequest,
    SetCartPositionResponse,
    RemoveCartPositionRequest,
    RemoveCartPositionResponse,
    ClearCartPositionResponse,
} from "./dtos/CartDtos";
import { HttpClient } from "./HttpClient";

class CCartsClient {

    current = async () =>
        await HttpClient.get<GetCartResponse>(
            GetCartResponse,
            `${AppSettings.CartsHost}/api/carts/current`,
            AuthService.getAuthHeader()
        );

    setPositions = async (req: SetCartPositionsRequest) =>
        await HttpClient.post<SetCartPositionsRequest, SetCartPositionResponse>(
            SetCartPositionResponse,
            `${AppSettings.CartsHost}/api/carts/set-positions`,
            req,
            AuthService.getAuthHeader()
        );

    removePosition = async (req: RemoveCartPositionRequest) =>
        await HttpClient.delete<RemoveCartPositionRequest, RemoveCartPositionResponse>(
            RemoveCartPositionResponse,
            `${AppSettings.CartsHost}/api/carts/remove-positions`,
            req,
            AuthService.getAuthHeader()
        );

    clear = async () =>
        await HttpClient.delete<undefined, ClearCartPositionResponse>(
            ClearCartPositionResponse,
            `${AppSettings.CartsHost}/api/carts/clear`,
            undefined,
            AuthService.getAuthHeader()
        );
}

export const CartsClient:CCartsClient = new CCartsClient();