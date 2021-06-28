import { AppSettings } from "components/app/app-settings";
import {
    IIdentityClient,
    SignUpRequest,
    SignUpResponse,
    SignInRequest,
    SignInResponse,
    CurrentIdentityResponse,
    RefreshTokenRequest,
    RefreshTokenResponse,
    DeleteIdentityResponse,
    ChangePasswordRequest,
    ChangePasswordResponse,
} from "../abstractions/IIdentityClient";
import { HttpClient } from "./HttpClient";

export class IdentityClient implements IIdentityClient {
    private http: HttpClient = new HttpClient();

    signIn = async (req: SignInRequest) =>
        await this.http.post<SignInRequest, SignInResponse>(
            `${AppSettings.IdentityHost}/api/auth/sign-in`,
            req
        );

    current = async () =>
        await this.http.get<CurrentIdentityResponse>(
            `${AppSettings.IdentityHost}/api/auth/current`
        );

    signUp = async (req: SignUpRequest) =>
        await this.http.post<SignUpRequest, SignUpResponse>(
            `${AppSettings.IdentityHost}/api/account/sign-up`,
            req
        );

    changePassword = async (req: ChangePasswordRequest) =>
        await this.http.post<ChangePasswordRequest, ChangePasswordResponse>(
            `${AppSettings.IdentityHost}/api/delete`,
            req
        );

    refresh = async (req: RefreshTokenRequest) =>
        await this.http.post<RefreshTokenRequest, RefreshTokenResponse>(
            `${AppSettings.IdentityHost}/api/auth/refresh`,
            req
        );

    delete = async () =>
        await this.http.delete<undefined, DeleteIdentityResponse>(
            `${AppSettings.IdentityHost}/api/account/delete`
        );
}
