import * as React from "react";
import { Map } from "typescript";
import "./select.sass";

export class SelectOption {
    value: string = "";
    label: string = "";
}
interface ISelectProps {
    selected: string;
    data: Array<SelectOption>;
    onChange: (value: string) => void;
}

const Select: React.FunctionComponent<ISelectProps> = (props) => {
    const default_value = props.data.length > 0 ? props.data[0].value : "";
    const [state, setState] = React.useState<string>(default_value);

    const jsxOptions = props.data.map((e) => (
        <option key={e.value} value={e.value}>
            {e.label}
        </option>
    ));

    return (
        <select
            value={props.selected}
            className="kshop-select"
            onChange={(e) => props.onChange(e.target.value)}
        >
            {jsxOptions}
        </select>
    );
};

export default Select;
