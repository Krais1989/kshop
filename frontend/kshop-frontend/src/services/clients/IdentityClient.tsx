import { AppSettings } from "components/app/AppSettings";
import { AuthService } from "services/AuthService";
import { SignInRequest, SignInResponse, CurrentIdentityResponse, SignUpRequest, SignUpResponse, ChangePasswordRequest, ChangePasswordResponse, RefreshTokenRequest, RefreshTokenResponse, DeleteIdentityResponse } from "./dtos/IdentityDtos";
import { HttpClient } from "./HttpClient";

class CIdentityClient {

    signIn = async (req: SignInRequest) =>
        await HttpClient.post<SignInRequest, SignInResponse>(
            SignInResponse,
            `${AppSettings.IdentityHost}/api/auth/sign-in`,
            req
        );

    current = async () =>
        await HttpClient.get<CurrentIdentityResponse>(
            CurrentIdentityResponse,
            `${AppSettings.IdentityHost}/api/auth/current`, AuthService.getAuthHeader()
        );

    signUp = async (req: SignUpRequest) =>
        await HttpClient.post<SignUpRequest, SignUpResponse>(
            SignUpResponse,
            `${AppSettings.IdentityHost}/api/account/sign-up`,
            req
        );

    changePassword = async (req: ChangePasswordRequest) =>
        await HttpClient.post<ChangePasswordRequest, ChangePasswordResponse>(
            ChangePasswordResponse,
            `${AppSettings.IdentityHost}/api/account/change-password`,
            req,
            AuthService.getAuthHeader()
        );

    refresh = async (req: RefreshTokenRequest) =>
        await HttpClient.post<RefreshTokenRequest, RefreshTokenResponse>(
            RefreshTokenResponse,
            `${AppSettings.IdentityHost}/api/auth/refresh`,
            req
        );

    delete = async () =>
        await HttpClient.delete<undefined, DeleteIdentityResponse>(
            DeleteIdentityResponse,
            `${AppSettings.IdentityHost}/api/account/delete`,
            undefined,
            AuthService.getAuthHeader()
        );
}

export const IdentityClient:CIdentityClient = new CIdentityClient();