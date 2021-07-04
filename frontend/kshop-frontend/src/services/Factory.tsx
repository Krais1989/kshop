import { BaseResult } from "./BaseResult";

// class CFactory<T> {

//     public Create(type: {new(): T}):T {
//         const instance = new type() as T;
//         return instance;
//     }
// }

// export const Factory:CFactory = new CFactory();


export function CreateInstanceOf<T>(c: new() => T): T{
    return new c();
}