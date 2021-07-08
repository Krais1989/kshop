import React, { useEffect } from "react";
import { createContext, useState } from "react";
import { ProductsClient } from "services/clients/ProductsClient";
import { useAuth } from "./AuthProvider";

export class BookmarksData {
    productsIDs: Array<number> = [];
}

export class BookmarksContextData {
    addBookmarks: (prodsIds: number[]) => void = () => {};
    delBookmarks: (prodsIds: number[]) => void = () => {};
    isBookmarked: (prodID: number) => boolean = (prodID) => false;
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
        ProductsClient.addBookmarks({ productsIDs: prodsIDs });
        const new_bookmarks = Object.assign(new BookmarksData(), {
            productsIDs: [...new Set([...bookmarks.productsIDs, prodsIDs])],
        });
        setBookmarks(new_bookmarks);
    };

    const delBookmarks = (prodsIDs: number[]) => {
        ProductsClient.delBookmarks({ productsIDs: prodsIDs });
        const new_bookmarks = Object.assign(new BookmarksData(), {
            productsIDs: bookmarks.productsIDs.filter((id) => !prodsIDs.includes(id)),
        });
        setBookmarks(new_bookmarks);
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
