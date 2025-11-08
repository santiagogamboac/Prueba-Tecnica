import { useEffect, useState } from 'react';
import type { Product, Category } from '../../../../types';

import ProductDetail from './ProductDetail';
import Toast from '../../../common/components/Toast';
import ProductTable from './ProductTable';
import { useBoundStore } from '../../../../store';
import { productService } from '../services/productService';
import { categoryService } from '../../categories/services/categoryService';

type ToastMessage = {
  id: number;
  message: string;
  type: 'success' | 'error';
};


export default function Dashboard() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [viewingProduct, setViewingProduct] = useState<Product | null>(null);
  const [toasts, setToasts] = useState<ToastMessage[]>([]);

  const setLogout = useBoundStore((state) => state.setLogout)
  const handleLogout = () => {
    setLogout()
  };

  const showToast = (message: string, type: 'success' | 'error') => {
    const id = Date.now();
    setToasts((prev) => [...prev, { id, message, type }]);
  };

  const removeToast = (id: number) => {
    setToasts((prev) => prev.filter((toast) => toast.id !== id));
  };

  const fetchCategories = async () => {
    const categories = await categoryService.getCategories();
    setCategories(categories);
  };
  useEffect(() => {
    fetchCategories();
  }, []);

  const handleDeleteProduct = async (productId: number) => {
    const res = await productService.deleteProduct(productId);
    if (!res.status) {
      showToast('Error al eliminar el producto', 'error');
    } else {
      showToast('Producto eliminado exitosamente', 'success');
    }
  };

  const handleViewDetail = async (productId: number) => {
    const product = await productService.getProductById(productId);
    setViewingProduct(product);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white border-b border-gray-200 shadow-lg">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-4xl font-bold text-gray-900 tracking-tight mb-2">
                Gestión de Productos
              </h1>
            </div>

            <div className="flex items-center gap-4">
              <>
                <div className="hidden md:block h-12 w-px bg-gray-200"></div>
                <button
                  onClick={handleLogout}
                  className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-xl hover:bg-gray-50 hover:border-red-500 hover:text-red-600 transition-all duration-200 shadow-sm"
                  title="Cerrar sesión"
                >
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                  </svg>
                  <span className="hidden sm:inline">Cerrar Sesión</span>
                </button>
              </>

            </div>
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <ProductTable
          categories={categories}

          onDelete={handleDeleteProduct}
          onViewDetail={handleViewDetail}
          showToast={showToast}
          fetchCategories={fetchCategories}

        />
      </main>

      {viewingProduct && (
        <ProductDetail
          product={viewingProduct}
          onClose={() => setViewingProduct(null)}
        />
      )}

      {toasts.map((toast) => (
        <Toast
          key={toast.id}
          message={toast.message}
          type={toast.type}
          onClose={() => removeToast(toast.id)}
        />
      ))}
    </div>
  );
}

