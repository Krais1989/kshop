import React, { useEffect } from "react";
import { createContext, useState } from "react";
import { toast } from "react-toastify";
import { ProductsClient } from "services/clients/ProductsClient";
import { useAuth } from "./AuthProvider";

export class BookmarksData {
    productsIDs: Array<number> = [];
}

export class BookmarksContextData {
    addBookmarks!: (prodsIds: number[]) => Promise<any>;
    delBookmarks!: (prodsIds: number[]) => Promise<any>;
    isBookmarked!: (prodID: number) => boolean;
}

const BookmarksContext = createContext<BookmarksContextData>(new BookmarksContextData());

interface IProps {}

export const BookmarksProvider: React.FunctionComponent<IProps> = (props) => {
    const [bookmarks, setBookmarks] = useState(new BookmarksData());
    const { auth, isAuthenticated } = useAuth();

    useEffect(() => {
        async function getBookmarks() {
            ProductsClient.getBookmarks().then((r) => {
                if (r.isSuccess) setBookmarks({ productsIDs: r.productsIDs });
            });
        }

        if (isAuthenticated()) getBookmarks();
    }, [auth]);

    const addBookmarks = (prodsIDs: number[]) => {
        return ProductsClient.addBookmarks({ productsIDs: prodsIDs }).then((r) => {
            if (r.isSuccess) {
                const new_bookmarks = Object.assign({
                    productsIDs: [...new Set([...bookmarks.productsIDs, ...prodsIDs])],
                });
                setBookmarks(new_bookmarks);
                console.log("ADD BOOKMARK");
                toast.success(`Favorit added #${prodsIDs[0]}`, { autoClose: 2000 });
            }
        });
    };

    const delBookmarks = (prodsIDs: number[]) => {
        return ProductsClient.delBookmarks({ productsIDs: prodsIDs }).then((r) => {
            if (r.isSuccess) {
                const new_bookmarks = Object.assign({
                    productsIDs: bookmarks.productsIDs.filter((id) => !prodsIDs.includes(id)),
                });
                setBookmarks(new_bookmarks);
                console.log("DEL BOOKMARK");
                toast.success(`Favorit removed #${prodsIDs[0]}`, { autoClose: 2000 });
            }
        });
    };

    const isBookmarked = (prodID: number) => {
        const state = bookmarks.productsIDs.includes(prodID);
        return state;
    };

    return (
        <BookmarksContext.Provider
            value={{
                addBookmarks: addBookmarks,
                delBookmarks: delBookmarks,
                isBookmarked: isBookmarked,
            }}
        >
            {props.children}
        </BookmarksContext.Provider>
    );
};

export const useBookmarks = () => React.useContext(BookmarksContext);
