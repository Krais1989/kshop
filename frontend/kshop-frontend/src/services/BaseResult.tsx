export class BaseResult {
    readonly errorMessage: string | undefined = undefined;
    readonly isSuccess: boolean = true;
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
