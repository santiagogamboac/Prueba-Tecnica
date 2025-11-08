export interface Category {
  categoryId?: number;
  categoryName: string;
  description: string;
  picture?: string;
}

export interface Product {
  productId: number;
  productName: string;
  unitsInStock: number;
  unitPrice: number;
  quantityPerUnit: string;
  categoryName: string;
  categoryId?: number;
  discontinued: boolean;
  picture?: string
}
