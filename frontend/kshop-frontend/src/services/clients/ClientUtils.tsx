class CClientUtils {
    objToQueryParams: (obj: any) => string = (obj) => {
        return Object.keys(obj)
            .map((v, i) => `${i === 0 ? "?" : "&"}${v}=${obj[v]}`)
            .join("");
    };

}

export const ClientUtils:CClientUtils = new CClientUtils();
