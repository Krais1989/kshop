import { BaseResult } from "services/BaseResult";

export class SignUpRequest {
    email: string = "";
    password: string = "";
    phoneNumber: string = "";
}
export class SignUpResponse extends BaseResult {
    id: string = "";
}

export class SignInRequest {
    email: string = "";
    password: string = "";
}
export class SignInResponse extends BaseResult {
    email: string = "";
    token: string = "";
    refreshToken: string = "";
}

export class CurrentIdentityResponse extends BaseResult {
    email: string = "";
}

export class DeleteIdentityResponse extends BaseResult {}

export class RefreshTokenRequest {
    refreshToken: string = "";
}
export class RefreshTokenResponse extends BaseResult {}

export class ChangePasswordRequest{
    oldPassword: string = "";
    newPassword: string = "";
}
export class ChangePasswordResponse extends BaseResult {}