import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import type { Product, Category } from '../../../../types';
import SearchableSelect from '../../../common/components/SearchableSelect';

interface ProductFormProps {
  product?: Product | null;
  categories: Category[];
  onSubmit: (product: Product) => void;
  onCancel: () => void;
  onSuccess?: () => void;
  onError?: (message: string) => void;
}
const productSchema = z.object({
  productId: z.number().optional(),

  productName: z.string()
    .min(1, 'Este campo es requerido')
    .max(200)
    .trim(),

  unitPrice: z.number()
    .nonnegative('El precio debe ser mayor o igual a 0'),

  unitsInStock: z.number()
    .int('El stock debe ser un número entero')
    .nonnegative('El stock debe ser mayor o igual a 0'),

  quantityPerUnit: z.string()
    .min(1, 'Este campo es requerido')
    .max(100)
    .trim(),

  categoryId: z.number()
    .int()
    .positive('Selecciona una categoría válida'),

  categoryName: z.string().optional(),
  discontinued: z.boolean(),
  picture: z.string().trim().optional(),
});


type ProductFormData = z.infer<typeof productSchema>;


export default function ProductForm({
  product,
  categories,
  onSubmit,
  onCancel,
  onSuccess,
  onError,
}: ProductFormProps) {
  const {
    register,
    handleSubmit,
    setValue,
    reset,
    watch,
    formState: { errors, isSubmitting },
  } = useForm<ProductFormData>({
    resolver: zodResolver(productSchema),
    defaultValues: {
      productName: '',
      unitPrice: 0,
      unitsInStock: 0,
      quantityPerUnit: '',
      categoryId: 0,
      discontinued: false,
      picture: '',
    },
  });


  useEffect(() => {
    if (product) {
      reset({
        productId: product.productId,
        productName: product.productName,
        unitPrice: product.unitPrice,
        unitsInStock: product.unitsInStock,
        quantityPerUnit: product.quantityPerUnit || '',
        categoryId: product.categoryId,
        discontinued: product.discontinued,
        picture: product.picture || '',
      });
    } else {
      reset();
    }
  }, [product, reset]);

  const onSubmitForm = async (data: ProductFormData) => {
    try {
      const selectedCategory = categories.find((c) => c.categoryId === data.categoryId);
      const validatedProduct: Product = {
        ...data,
        quantityPerUnit: data.quantityPerUnit,
        categoryName: selectedCategory?.categoryName || '',
      } as Product;

      await onSubmit(validatedProduct);
      onSuccess?.();
    } catch (err) {
      const message =
        err instanceof Error
          ? err.message
          : 'Error al guardar el producto. Intenta de nuevo.';
      onError?.(message);
    }
  };

  const categoryOptions = categories.map((cat) => ({
    value: cat.categoryId!,
    label: cat.categoryName,
  }));
  const categoryIdValue = watch('categoryId');

  return (
    <div
      className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4"
      onClick={onCancel}
    >
      <div
        className="bg-white rounded-2xl w-full max-w-4xl h-3/4 overflow-y-auto shadow-2xl"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="flex items-center justify-between p-6 border-b border-gray-200">
          <h2 className="text-2xl font-bold text-gray-900">
            {product ? 'Editar Producto' : 'Nuevo Producto'}
          </h2>
          <button
            type="button"
            className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-lg transition-colors"
            onClick={onCancel}
            disabled={isSubmitting}
          >
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
              <path
                d="M15 5L5 15M5 5L15 15"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
              />
            </svg>
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmitForm)} className="p-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
            {/* Nombre */}
            <div className="lg:col-span-2">
              <label className="block mb-2 text-sm font-semibold text-gray-700 uppercase tracking-wide">
                Nombre del Producto *
              </label>
              <input
                type="text"
                {...register('productName')}
                placeholder="Ej: Laptop Dell"
                disabled={isSubmitting}
                className={`w-full px-4 py-3 border rounded-xl text-sm transition-all duration-200 ${errors.productName
                  ? 'border-red-500 focus:border-red-500 focus:ring-red-100'
                  : 'border-gray-300 focus:border-blue-500 focus:ring-blue-100'
                  }`}
              />
              {errors.productName && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.productName.message}
                </p>
              )}
            </div>

            {/* Precio */}
            <div>
              <label className="block mb-2 text-sm font-semibold text-gray-700 uppercase tracking-wide">
                Precio *
              </label>
              <input
                type="number"
                step="0.01"
                min="0"
                {...register('unitPrice', { valueAsNumber: true })}
                placeholder="0.00"
                disabled={isSubmitting}
                className={`w-full px-4 py-3 border rounded-xl text-sm ${errors.unitPrice
                  ? 'border-red-500 focus:border-red-500 focus:ring-red-100'
                  : 'border-gray-300 focus:border-blue-500 focus:ring-blue-100'
                  }`}
              />
              {errors.unitPrice && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.unitPrice.message}
                </p>
              )}
            </div>

            {/* Stock */}
            <div>
              <label className="block mb-2 text-sm font-semibold text-gray-700 uppercase tracking-wide">
                Stock *
              </label>
              <input
                type="number"
                step="1"
                min="0"
                {...register('unitsInStock', { valueAsNumber: true })}
                placeholder="0"
                disabled={isSubmitting}
                className={`w-full px-4 py-3 border rounded-xl text-sm ${errors.unitsInStock
                  ? 'border-red-500 focus:border-red-500 focus:ring-red-100'
                  : 'border-gray-300 focus:border-blue-500 focus:ring-blue-100'
                  }`}
              />
              {errors.unitsInStock && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.unitsInStock.message}
                </p>
              )}
            </div>

            {/* Cantidad por unidad */}
            <div>
              <label className="block mb-2 text-sm font-semibold text-gray-700 uppercase tracking-wide">
                Cantidad por Unidad *
              </label>
              <input
                type="text"
                {...register('quantityPerUnit')}
                placeholder="Ej: 10 boxes x 20 bags"
                disabled={isSubmitting}
                className={`w-full px-4 py-3 border rounded-xl text-sm ${errors.quantityPerUnit
                  ? 'border-red-500 focus:border-red-500 focus:ring-red-100'
                  : 'border-gray-300 focus:border-blue-500 focus:ring-blue-100'
                  }`}
              />
              {errors.quantityPerUnit && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.quantityPerUnit.message}
                </p>
              )}
            </div>

            {/* Categoría */}
            <div>
              <label className="block mb-2 text-sm font-semibold text-gray-700 uppercase tracking-wide">
                Categoría *
              </label>
              <SearchableSelect
                options={categoryOptions}
                value={categoryIdValue}
                onChange={(value) =>
                  setValue('categoryId', value as number, { shouldValidate: true })
                }
                placeholder="Seleccionar categoría..."
                hasError={!!errors.categoryId}
              />
              {errors.categoryId && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.categoryId.message}
                </p>
              )}
            </div>
          </div>

          {/* Descontinuado */}
          <div className="mb-6">
            <label className="flex items-center gap-3 cursor-pointer">
              <input
                type="checkbox"
                {...register('discontinued')}
                className="w-5 h-5 rounded border-gray-300 text-blue-600 focus:ring-2 focus:ring-blue-500 cursor-pointer"
              />
              <span className="text-sm font-medium text-gray-700">
                Producto descontinuado
              </span>
            </label>
          </div>

          {/* Botones */}
          <div className="flex gap-3 pt-6 border-t border-gray-200">
            <button
              type="submit"
              disabled={isSubmitting}
              className="flex-1 px-6 py-3 bg-blue-600 text-white rounded-xl font-medium shadow-lg hover:bg-blue-700 transition-all duration-200 disabled:bg-blue-400"
            >
              {isSubmitting
                ? 'Guardando...'
                : product
                  ? 'Actualizar Producto'
                  : 'Crear Producto'}
            </button>
            <button
              type="button"
              onClick={onCancel}
              disabled={isSubmitting}
              className="flex-1 px-6 py-3 bg-white text-gray-700 border border-gray-300 rounded-xl font-medium shadow-md hover:bg-gray-50 transition-all duration-200"
            >
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}