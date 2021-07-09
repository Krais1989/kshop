import Submitter from "components/controls/submitter/Submitter";
import * as React from "react";
import { toast } from "react-toastify";
import { ChangePasswordResponse } from "services/clients/dtos/IdentityDtos";
import { IdentityClient } from "services/clients/IdentityClient";
import "./w-change-password.sass";

interface IWChangePasswordProps {}

const WChangePassword: React.FunctionComponent<IWChangePasswordProps> = (props) => {
    const [oldPassword, setOldPassword] = React.useState("");
    const [newPassword, setNewPassword] = React.useState("");
    //const [isLoad, setIsLoad] = React.useState(false);

    const submit = () => {
        //if (isLoad) return new ChangePasswordResponse();
        //setIsLoad(true);
        return IdentityClient.changePassword({
            oldPassword: oldPassword,
            newPassword: newPassword,
        }).then((r) => {
            if (r.isSuccess) {
                toast.success("Password changed");
            } else {
                toast.error(`Password error: ${r.errorMessage}`);
            }
        });
        //.catch((err) => setIsLoad(false));
    };

    return (
        <div className="kshop-w-change-password">
            <input
                className="kshop-input kshop-w-change-password-row"
                type="password"
                value={oldPassword}
                onChange={(e) => setOldPassword(e.target.value)}
                placeholder="old password..."
            ></input>
            <input
                className="kshop-input kshop-w-change-password-row"
                type="password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                placeholder="new password..."
            ></input>

            <Submitter submit={() => submit()}>
                <button className="kshop-button kshop-w-change-password-row" onClick={e=>{}}>
                    Submit
                </button>
            </Submitter>
            {/* <button
                className="kshop-button kshop-w-change-password-row"
                disabled={isLoad}
                onClick={(e) => submit()}
            >
                Submit
            </button> */}
        </div>
    );
};

export default WChangePassword;
