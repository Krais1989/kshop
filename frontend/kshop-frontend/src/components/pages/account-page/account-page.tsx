import WChangePassword from "components/widgets/w-change-password/w-change-password";
import * as React from "react";
import "./account-page.sass";

interface IAccountPageProps {}

const AccountPage: React.FunctionComponent<IAccountPageProps> = (props) => {
    return (
        <div className="kshop-base-page kshop-account-page">
            AccountPage
            <WChangePassword />
        </div>
    );
};

export default AccountPage;
