
export class BaseResult {
    readonly ErrorMessage: string | undefined;
    public IsSuccess(): boolean {
        return this.ErrorMessage == undefined;
    }
    constructor(errorMessage: string | undefined = undefined) {
        this.ErrorMessage = errorMessage;
    }
}
