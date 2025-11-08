import type { Product } from "../../../../types";
import { shopApi } from "../../../common/api/shopApi";
import type { PaginationResponse } from "../hooks/useProducts";

export interface PaginationParams {
    pageIndex?: number;
    pageSize?: number;
    search?: string;
}

interface GeneralResponse {
    status: boolean
    message: string
}

export const productService = {
    getProducts: async (params: PaginationParams) => {
        const response = await shopApi.get('/product', { params }) as PaginationResponse<Product>;
        return response;
    },

    getProductById: async (id: number) => {
        const response = await shopApi.get(`/product/${id}`) as Product;
        return response;
    },

    createSingleProduct: async (productData: Omit<Product, 'productId'>) => {
        return await shopApi.post('/product', productData);

    },

    createProduct: async () => {
        const response = await shopApi.post('/product/bulk') as GeneralResponse;
        return response;
    },

    updateProduct: async (id: string | number, payload: Product) => {
        return await shopApi.put(`/product/${id}`, payload);

    },

    deleteProduct: async (id: number) => {
        return await shopApi.delete(`/product/${id}`);
    }
};