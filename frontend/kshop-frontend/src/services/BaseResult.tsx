
export class BaseResult {
    ErrorMessage: string | undefined;
    // public IsSuccess(): boolean {
    //     return this.ErrorMessage === undefined || this.ErrorMessage === null || this.ErrorMessage === "";
    // }
    constructor(errorMessage: string | undefined = undefined) {
        this.ErrorMessage = errorMessage;
    }
}

// export class Result<T> {
//     Data?: T;
//     ErrorMessage: string|undefined;

//     IsSuccess() {
//         return this.ErrorMessage === undefined || this.ErrorMessage === null || this.ErrorMessage === "";
//     }

//     constructor(data?: T, errorMessage: string|undefined = undefined) {
//         this.Data = data;
//         this.ErrorMessage = errorMessage;
//     }
// }