import { useEffect, useState } from 'react';
import { productService, type PaginationParams } from '../services/productService';
import type { Product } from '../../../../types';

export interface PaginationResponse<T> {
    data: T[];
    count: number;
    pageCount: number;
    pageIndex: number;
    pageSize: number;
}

export interface ProductParams extends PaginationParams {
    categoryId?: number;
    discontinued?: boolean;
}

export function useProducts(initialParams: ProductParams = { pageIndex: 1, pageSize: 10, search: '' }) {
    const [params, setParams] = useState<ProductParams>(initialParams);
    const [products, setProducts] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<any | null>(null);
    const [pagination, setPagination] = useState<Omit<PaginationResponse<any>, 'data'> | null>(null);


    const fetchProducts = async () => {
        setLoading(true);
        setError(null);
        try {
            const response: PaginationResponse<Product> = await productService.getProducts(params);
            setProducts(response.data);
            setPagination({
                count: response.count,
                pageCount: response.pageCount,
                pageIndex: response.pageIndex,
                pageSize: response.pageSize
            });
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchProducts();
    }, [params]);

    return {
        products,
        loading,
        error,
        pagination,
        params,
        setParams,
        refetch: fetchProducts
    };
}