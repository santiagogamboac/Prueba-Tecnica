import { useState } from 'react';
import LoadingButton from '../../../common/components/LoadingButton';
import type { Category, Product } from '../../../../types';
import SearchableSelect from '../../../common/components/SearchableSelect';
import { productService } from '../services/productService';
import { useProducts } from '../hooks/useProducts';
import CategoryManager from '../../../common/components/CategoryManager';
import ProductForm from './ProductForm';
import { categoryService } from '../../categories/services/categoryService';

interface ProductTableProps {
  categories: Category[];
  onDelete: (productId: number) => void;
  onViewDetail: (productId: number) => void;
  showToast: (message: string, type: 'success' | 'error') => void;
  fetchCategories: () => void;
  refreshTrigger?: boolean;
}

export default function ProductTable({
  categories,
  onDelete,
  onViewDetail,
  showToast,
  fetchCategories
}: ProductTableProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<'all' | `${number}`>('all');
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [isBulkLoading, setIsBulkLoading] = useState(false);
  const [showProductForm, setShowProductForm] = useState(false);
  const [discontinuedFilter, setDiscontinuedFilter] = useState<'all' | '0' | '1'>('all');

  const { products, pagination, loading, setParams, refetch } = useProducts({
    pageIndex: 1,
    pageSize: 10,
    search: searchTerm
  });
  const categoryOptions = [
    { value: 'all', label: 'Todas las categorías' },
    ...(categories ?? []).map(cat => ({
      value: (cat.categoryId ?? '').toString(),
      label: cat.categoryName ?? 'Sin nombre'
    }))
  ];

  const discontinuedOptions = [
    { value: 'all', label: 'Todos' },
    { value: '0', label: 'Activos' },
    { value: '1', label: 'Descontinuados' }
  ];

  const handleSaveProduct = async (productData: Product) => {
    try {
      let response;
      if ('productId' in productData && productData.productId) {
        response = await productService.updateProduct(productData.productId, productData);
        if (response.status) showToast('Se ha actualizado el producto', 'success');
      } else {
        response = await productService.createSingleProduct(productData);
        if (response.status) showToast('Se ha creado el producto', 'success');
      }

      setShowProductForm(false);
      setEditingProduct(null);
      refetch();

    } catch (error) {
      console.error('Error saving product:', error);
      showToast('Error al guardar el producto', 'error');
    }
  };

  const handleSaveSuccess = () => {
    if (editingProduct) {
      showToast('Producto actualizado exitosamente', 'success');
    } else {
      showToast('Producto creado exitosamente', 'success');
    }
  };

  const handleSaveError = (message: string) => {
    showToast(message, 'error');
  };


  const handlePageChange = (page: number) => {
    setParams(prev => ({ ...prev, pageIndex: page }));
  };
  const handleSearch = (term: string) => {
    setSearchTerm(term);
    setParams(prev => ({ ...prev, search: term, pageIndex: 1 }));
  };

  const handleCategoryChange = (value: string | number) => {
    const val = value.toString();
    setSelectedCategory(val as 'all' | `${number}`);

    setParams(prev => ({
      ...prev,
      categoryId: val === 'all' ? undefined : parseInt(val),
      pageIndex: 1
    }));
  };
  const handleDiscontinuedChange = (value: string | number) => {
    if (value == null) return;
    const val = value.toString();

    setDiscontinuedFilter(val as 'all' | '0' | '1');

    setParams(prev => ({
      ...prev,
      discontinued: val === 'all' ? undefined : val === '1',
      pageIndex: 1
    }));
  };
  const handleDelete = async (productId: number) => {
    if (window.confirm('¿Estás seguro de que deseas eliminar este producto?')) {
      await onDelete(productId);
      await refetch();
    }
  };

  const handleBulkProductUpload = async () => {
    try {
      setIsBulkLoading(true);
      const response = await productService.createProduct();
      refetch();
      showToast(response.message, response.status ? 'success' : 'error');
    } catch (error) {
      console.log('Error creating bulk products:', error);
      showToast('Error al crear productos masivamente', 'error');
    } finally {
      setIsBulkLoading(false);
    }
  }

  const handleAddCategory = async (categoryData: Category) => {
    const res = await categoryService.createCategory(categoryData)
    showToast(res.message, res.status ? 'success' : 'error');
    fetchCategories();
  };

  const handleEditProduct = (product: Product) => {
    setEditingProduct(product);
    setShowProductForm(true);
  };

  const currentPage = pagination?.pageIndex || 1;
  const totalPages = pagination?.pageCount || 1;

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <CategoryManager categories={categories} onAddCategory={handleAddCategory} />

        <div className="flex gap-3">
          <LoadingButton
            onClick={() => setShowProductForm(true)}
            leftIcon={
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
              </svg>
            }
          >
            Nuevo Producto
          </LoadingButton>

          <LoadingButton
            onClick={handleBulkProductUpload}
            isLoading={isBulkLoading}
            className="bg-emerald-600 hover:bg-emerald-700"
            leftIcon={
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12" />
              </svg>
            }
          >
            Carga Masiva
          </LoadingButton>
        </div>
      </div>
      <div className="bg-white border border-gray-200 rounded-2xl shadow-xl overflow-hidden neo-outset">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 p-6 bg-gray-50 border-b border-gray-200">
          <div className="flex flex-col gap-2">
            <label className="text-xs font-bold text-gray-600 uppercase tracking-wider">Búsqueda</label>
            <div className="relative">
              <svg
                className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400 pointer-events-none"
                viewBox="0 0 16 16"
                fill="none"
              >
                <path
                  d="M7 12C9.76142 12 12 9.76142 12 7C12 4.23858 9.76142 2 7 2C4.23858 2 2 4.23858 2 7C2 9.76142 4.23858 12 7 12Z"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <path
                  d="M11 11L14 14"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
              </svg>
              <input
                type="text"
                placeholder="Buscar por nombre..."
                value={searchTerm}
                onChange={(e) => handleSearch(e.target.value)}
                className="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-xl text-sm text-gray-900 bg-white transition-all duration-200 focus:outline-none focus:border-blue-500 focus:ring-4 focus:ring-blue-100 neo-soft"
              />
            </div>
          </div>

          <div className="flex flex-col gap-2">
            <label className="text-xs font-bold text-gray-600 uppercase tracking-wider">
              Categoría
            </label>

            <SearchableSelect
              options={categoryOptions}
              value={selectedCategory ?? 'all'}
              onChange={handleCategoryChange}
              placeholder="Todas las categorías"
            />
          </div>

          <div className="flex flex-col gap-2">
            <label className="text-xs font-bold text-gray-600 uppercase tracking-wider">
              Estado
            </label>

            <SearchableSelect
              options={discontinuedOptions}
              value={discontinuedFilter}
              onChange={handleDiscontinuedChange}
              placeholder="Todos"
            />
          </div>

        </div>

        <div className="px-6 py-4 bg-white border-b border-gray-200">
          {loading ? (
            <span className="text-sm text-gray-600">Cargando...</span>
          ) : (
            <span className="text-sm text-gray-600">
              Mostrando <span className="font-semibold text-gray-900">{products.length}</span> de{' '}
              <span className="font-semibold text-gray-900">{pagination?.count || 0}</span> productos
            </span>
          )}
        </div>

        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-4 text-left text-xs font-bold text-gray-600 uppercase tracking-wider border-b border-gray-200">
                  Nombre
                </th>
                <th className="px-6 py-4 text-left text-xs font-bold text-gray-600 uppercase tracking-wider border-b border-gray-200">
                  Categoría
                </th>
                <th className="px-6 py-4 text-left text-xs font-bold text-gray-600 uppercase tracking-wider border-b border-gray-200">
                  Precio
                </th>
                <th className="px-6 py-4 text-left text-xs font-bold text-gray-600 uppercase tracking-wider border-b border-gray-200">
                  Stock
                </th>
                <th className="px-6 py-4 text-left text-xs font-bold text-gray-600 uppercase tracking-wider border-b border-gray-200">
                  Estado
                </th>
                <th className="px-6 py-4 text-left text-xs font-bold text-gray-600 uppercase tracking-wider border-b border-gray-200">
                  Acciones
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-100">
              {loading ? (
                <tr>
                  <td colSpan={6} className="px-6 py-12 text-center text-gray-400 text-sm">
                    Cargando productos...
                  </td>
                </tr>
              ) : products.length > 0 ? (
                products.map((product) => (
                  <tr key={product.productId} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-semibold text-gray-900">{product.productName}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-600">{product.categoryName}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-semibold text-emerald-600">
                        ${product.unitPrice}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span
                        className={`text-sm font-semibold ${product.unitsInStock < 10 ? 'text-red-600' : 'text-gray-900'
                          }`}
                      >
                        {product.unitsInStock}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span
                        className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold ${product.discontinued
                          ? 'bg-red-100 text-red-800'
                          : 'bg-emerald-100 text-emerald-800'
                          }`}
                      >
                        {product.discontinued ? 'Descontinuado' : 'Activo'}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center gap-2">
                        <button
                          className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                          onClick={() => onViewDetail(product.productId)}
                          title="Ver detalle"
                        >
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                          </svg>
                        </button>
                        <button
                          className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-lg transition-colors"
                          onClick={() => handleEditProduct(product)}
                          title="Editar"
                        >
                          <svg className="w-4 h-4" viewBox="0 0 16 16" fill="none">
                            <path
                              d="M11.3333 2.00001C11.5084 1.8249 11.7163 1.68603 11.9441 1.59129C12.1719 1.49655 12.4151 1.44775 12.6667 1.44775C12.9182 1.44775 13.1614 1.49655 13.3892 1.59129C13.617 1.68603 13.8249 1.8249 14 2.00001C14.1751 2.17512 14.314 2.38301 14.4087 2.6108C14.5035 2.83859 14.5523 3.08183 14.5523 3.33334C14.5523 3.58485 14.5035 3.82809 14.4087 4.05588C14.314 4.28367 14.1751 4.49156 14 4.66667L5.00001 13.6667L1.33334 14.6667L2.33334 11L11.3333 2.00001Z"
                              stroke="currentColor"
                              strokeWidth="1.5"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            />
                          </svg>
                        </button>
                        <button
                          className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                          onClick={() => handleDelete(product.productId)}
                          title="Eliminar"
                        >
                          <svg className="w-4 h-4" viewBox="0 0 16 16" fill="none">
                            <path
                              d="M2 4H14M12.6667 4V13.3333C12.6667 13.687 12.5262 14.0261 12.2761 14.2761C12.0261 14.5262 11.687 14.6667 11.3333 14.6667H4.66667C4.31305 14.6667 3.97391 14.5262 3.72386 14.2761C3.47381 14.0261 3.33334 13.687 3.33334 13.3333V4M5.33334 4V2.66667C5.33334 2.31305 5.47381 1.97391 5.72386 1.72386C5.97391 1.47381 6.31305 1.33334 6.66667 1.33334H9.33334C9.68696 1.33334 10.0261 1.47381 10.2761 1.72386C10.5262 1.97391 10.6667 2.31305 10.6667 2.66667V4"
                              stroke="currentColor"
                              strokeWidth="1.5"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            />
                          </svg>
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={6} className="px-6 py-12 text-center text-gray-400 text-sm">
                    No se encontraron productos
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {totalPages > 1 && (() => {
          const getPageNumbers = () => {
            const delta = 2;
            const pages: (number | string)[] = [];

            if (totalPages <= 7) {
              return Array.from({ length: totalPages }, (_, i) => i + 1);
            }

            pages.push(1);

            let start = Math.max(2, currentPage - delta);
            let end = Math.min(totalPages - 1, currentPage + delta);

            if (currentPage <= delta + 1) {
              end = Math.min(5, totalPages - 1);
            }

            if (currentPage >= totalPages - delta) {
              start = Math.max(totalPages - 4, 2);
            }

            if (start > 2) {
              pages.push('...');
            }

            for (let i = start; i <= end; i++) {
              pages.push(i);
            }

            if (end < totalPages - 1) {
              pages.push('...');
            }

            if (totalPages > 1) {
              pages.push(totalPages);
            }

            return pages;
          };

          const pageNumbers = getPageNumbers();

          return (
            <div className="flex items-center justify-center gap-2 px-6 py-6 bg-gray-50 border-t border-gray-200 flex-wrap">
              <button
                className="p-2 border border-gray-200 rounded-lg bg-white text-gray-700 shadow-sm hover:bg-gray-50 hover:border-blue-500 hover:text-blue-600 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
                onClick={() => handlePageChange(1)}
                disabled={currentPage === 1}
                title="Primera página"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 19l-7-7 7-7m8 14l-7-7 7-7" />
                </svg>
              </button>
              <button
                className="p-2 border border-gray-200 rounded-lg bg-white text-gray-700 shadow-sm hover:bg-gray-50 hover:border-blue-500 hover:text-blue-600 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
                onClick={() => handlePageChange(currentPage - 1)}
                disabled={currentPage === 1}
                title="Página anterior"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
                </svg>
              </button>

              <div className="flex items-center gap-1">
                {pageNumbers.map((page, index) => {
                  if (page === '...') {
                    return (
                      <span key={`ellipsis-${index}`} className="px-2 text-gray-400">
                        ...
                      </span>
                    );
                  }

                  const pageNum = page as number;
                  return (
                    <button
                      key={pageNum}
                      onClick={() => handlePageChange(pageNum)}
                      className={`min-w-10 px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 ${currentPage === pageNum
                        ? 'bg-blue-600 text-white shadow-lg'
                        : 'bg-white text-gray-700 border border-gray-200 hover:bg-gray-50 hover:border-blue-500 hover:text-blue-600'
                        }`}
                    >
                      {pageNum}
                    </button>
                  );
                })}
              </div>

              <button
                className="p-2 border border-gray-200 rounded-lg bg-white text-gray-700 shadow-sm hover:bg-gray-50 hover:border-blue-500 hover:text-blue-600 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
                onClick={() => handlePageChange(currentPage + 1)}
                disabled={currentPage === totalPages}
                title="Página siguiente"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
                </svg>
              </button>
              <button
                className="p-2 border border-gray-200 rounded-lg bg-white text-gray-700 shadow-sm hover:bg-gray-50 hover:border-blue-500 hover:text-blue-600 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
                onClick={() => handlePageChange(totalPages)}
                disabled={currentPage === totalPages}
                title="Última página"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 5l7 7-7 7M5 5l7 7-7 7" />
                </svg>
              </button>

              <div className="text-sm text-gray-600 min-w-[120px] text-center ml-2">
                Página <span className="font-semibold text-gray-900">{currentPage}</span> de{' '}
                <span className="font-semibold text-gray-900">{totalPages}</span>
              </div>
            </div>
          );
        })()}
      </div>

      {showProductForm && (
        <ProductForm
          product={editingProduct}
          categories={categories}
          onSubmit={handleSaveProduct}
          onCancel={() => {
            setShowProductForm(false);
            setEditingProduct(null);
          }}
          onSuccess={handleSaveSuccess}
          onError={handleSaveError}
        />
      )}

    </div>
  );
}