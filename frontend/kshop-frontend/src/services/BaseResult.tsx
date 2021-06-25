
export class BaseResult {
    readonly ErrorMessage: string | undefined;
    public IsSuccess(): boolean {
        return this.ErrorMessage === undefined;
    }
    constructor(errorMessage: string | undefined = undefined) {
        this.ErrorMessage = errorMessage;
    }
}

export class Result<T> {
    Data?: T;
    readonly ErrorMessage: string|undefined;

    IsSuccess = () => this.ErrorMessage === undefined || this.ErrorMessage === null || this.ErrorMessage === "";

    constructor(data?: T, errorMessage: string|undefined = undefined) {
        this.Data = data;
        this.ErrorMessage = errorMessage;
    }
}