import type { Category } from "../../../../types";
import { shopApi } from "../../../common/api/shopApi";

export interface GeneralResponse {
    status: boolean;
    message: string;
}

export const categoryService = {

    getCategories: async () => {
        return await shopApi.get('/category') as Category[];
    },

    createCategory: async (categoryData: Category) => {
        return await shopApi.post('/category', categoryData) as GeneralResponse;

    },
};
