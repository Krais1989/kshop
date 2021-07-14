import * as React from "react";
import { Category } from "models/Category";
import { ProductsClient } from "services/clients/ProductsClient";
import "./w-categories-list.sass";

interface IWCategoriesListProps {
    curCategoryID: number;
    setCurCategoryFn: React.Dispatch<React.SetStateAction<number>>;
}

const WCategoriesList: React.FunctionComponent<IWCategoriesListProps> = (props) => {
    const [categories, setCategories] = React.useState<Array<Category>>([]);
    //const [curCategory, setCurCategory] = React.useState(0);

    React.useEffect(() => {
        async function loadCategories() {
            ProductsClient.getCategories().then((r) => {
                if (r.isSuccess) {
                    const categories = [new Category(0, "All"),...r.categories];
                    setCategories(categories);
                }
            });
        }
        loadCategories();
    }, []);

    if (categories.length === 0) return null;

    const jsxItems = categories.map((v, i) => (
        
        <div className={"kshop-w-categories-item" + (v.id===props.curCategoryID?"-selected":"")} key={v.id} onClick={(e) => props.setCurCategoryFn(v.id)}>
            <span>{v.name}</span>
            {/* <button onClick={(e) => props.setCurCategoryFn(v.id)}>{v.name}</button> */}
        </div>
    ));

    return <div className="kshop-w-categories">{jsxItems}</div>;
};

export default WCategoriesList;
