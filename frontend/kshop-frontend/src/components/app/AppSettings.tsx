import devSettings from "configs/appsettings.dev.json";
import stageSettings from "configs/appsettings.dev.json";
import { createContext, useContext } from "react";

export const AppEnv = process.env.npm_config_env;

export type CAppSettings = {
    IdentityHost?: string;
    OrdersHost?: string;
    ProductsHost?: string;
    PaymentsHost?: string;
    ShipmentsHost?: string;
    CartsHost?: string;
}

export const AppSettings = AppEnv === "stage" ? stageSettings : devSettings;
export const AppSettingsContext = createContext<CAppSettings>(AppSettings);
export const useAppSettings = () => useContext(AppSettingsContext);