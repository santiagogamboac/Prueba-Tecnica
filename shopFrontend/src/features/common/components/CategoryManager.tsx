import { useState, useRef } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import type { Category } from '../../../types';

interface CategoryManagerProps {
  categories: Category[];
  onAddCategory: (category: Category) => void;
  onError?: (message: string) => void;
}

const categorySchema = z.object({
  categoryName: z
    .string()
    .min(1, 'Este campo es requerido')
    .max(100, 'El nombre no puede exceder 100 caracteres')
    .trim(),
  description: z
    .string()
    .min(1, 'Este campo es requerido')
    .max(500, 'La descripción no puede exceder 500 caracteres')
    .trim(),
  picture: z
    .string()
    .min(1, 'Este campo es requerido')
    .trim(),
});

type CategoryFormData = z.infer<typeof categorySchema>;

export default function CategoryManager({ onAddCategory, onError }: CategoryManagerProps) {
  const [isOpen, setIsOpen] = useState(false);
  const [imagePreview, setImagePreview] = useState<string>('');
  const [imageSource, setImageSource] = useState<'url' | 'file'>('url');
  const fileInputRef = useRef<HTMLInputElement>(null);

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<CategoryFormData>({
    resolver: zodResolver(categorySchema),
    defaultValues: {
      categoryName: '',
      description: '',
      picture: '',
    },
  });


  const handleClose = () => {
    setIsOpen(false);
    reset();
    setImagePreview('');
    if (fileInputRef.current) fileInputRef.current.value = '';
  };

  const onSubmit = (data: CategoryFormData) => {
    onAddCategory({
      categoryName: data.categoryName.trim(),
      description: data.description?.trim(),
      picture: data.picture?.trim() || undefined,
    });
    handleClose();
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      onError?.('Por favor selecciona un archivo de imagen válido');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      onError?.('La imagen no debe superar los 5MB');
      return;
    }

    const reader = new FileReader();
    reader.onloadend = () => {
      const base64String = reader.result as string;
      setValue('picture', base64String);
      setImagePreview(base64String);
      setImageSource('file');
    };
    reader.onerror = () => {
      onError?.('Error al leer el archivo. Por favor intenta de nuevo.');
    };
    reader.readAsDataURL(file);
  };

  const handleImageUrlChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const url = e.target.value;
    setValue('picture', url);
    setImagePreview(url);
    setImageSource('url');
  };

  const handleRemoveImage = () => {
    setValue('picture', '');
    setImagePreview('');
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  return (
    <>
      <button
        className="px-6 py-3 bg-blue-600 text-white rounded-xl font-medium shadow-lg hover:bg-blue-700 hover:shadow-xl transition-all duration-200 flex items-center gap-2"
        onClick={() => setIsOpen(true)}
      >
        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
        </svg>
        Nueva Categoría
      </button>

      {isOpen && (
        <div
          className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4"
          onClick={handleClose}
        >
          <div
            className="bg-white rounded-2xl w-full max-w-md shadow-2xl"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="flex items-center justify-between p-6 border-b border-gray-200">
              <h2 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
                <svg
                  className="w-6 h-6 text-blue-600"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z"
                  />
                </svg>
                Crear Categoría
              </h2>
              <button
                className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-lg transition-colors"
                onClick={handleClose}
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

            <form onSubmit={handleSubmit(onSubmit)} className="p-6">
              {/* Nombre */}
              <div className="mb-5">
                <label className="block mb-2 text-sm font-semibold text-gray-700 uppercase tracking-wide">
                  Nombre de la categoría *
                </label>
                <input
                  type="text"
                  {...register('categoryName')}
                  placeholder="Ej: Electrónica"
                  disabled={isSubmitting}
                  className={`w-full px-4 py-3 border rounded-xl text-sm transition-all duration-200 ${errors.categoryName
                    ? 'border-red-500 focus:border-red-500 focus:ring-red-100'
                    : 'border-gray-300 focus:border-blue-500 focus:ring-blue-100'
                    }`}
                />
                {errors.categoryName && (
                  <p className="mt-1 text-sm text-red-600">{errors.categoryName.message}</p>
                )}
              </div>

              {/* Descripción */}
              <div className="mb-5">
                <label className="block text-sm font-semibold text-gray-700 uppercase tracking-wide">
                  Descripción
                </label>
                <textarea
                  {...register('description')}
                  placeholder="Descripción de la categoría (opcional)"
                  disabled={isSubmitting}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md text-sm focus:outline-none focus:border-blue-500 focus:ring-4 focus:ring-blue-100"
                />
                {errors.description && (
                  <p className="mt-1 text-sm text-red-600">
                    {errors.description.message}
                  </p>
                )}
              </div>

              {/* Imagen */}
              <div className="mb-5">
                <label className="block mb-2 text-sm font-semibold text-gray-700 uppercase tracking-wide">
                  Imagen de la categoría *
                </label>

                <div className="flex gap-2 mb-3">
                  <button
                    type="button"
                    onClick={() => setImageSource('url')}
                    className={`px-4 py-2 rounded-lg text-sm font-medium transition-all duration-200 ${imageSource === 'url'
                      ? 'bg-blue-600 text-white shadow-md'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                      }`}
                  >
                    Desde URL
                  </button>
                  <button
                    type="button"
                    onClick={() => setImageSource('file')}
                    className={`px-4 py-2 rounded-lg text-sm font-medium transition-all duration-200 ${imageSource === 'file'
                      ? 'bg-blue-600 text-white shadow-md'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                      }`}
                  >
                    Subir Archivo
                  </button>
                </div>

                {imageSource === 'file' ? (
                  <div className="space-y-3">
                    <input
                      ref={fileInputRef}
                      type="file"
                      accept="image/*"
                      onChange={handleFileChange}
                      className="hidden"
                      id="category-image-file"
                    />
                    <label
                      htmlFor="category-image-file"
                      className="flex items-center justify-center gap-2 px-4 py-3 border-2 border-dashed border-gray-300 rounded-xl cursor-pointer hover:border-blue-500 hover:bg-blue-50 transition-all duration-200"
                    >
                      <svg
                        className="w-5 h-5 text-gray-400"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth={2}
                          d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
                        />
                      </svg>
                      <span className="text-sm text-gray-600 font-medium">
                        {imagePreview ? 'Cambiar imagen' : 'Seleccionar imagen'}
                      </span>
                    </label>
                  </div>
                ) : (
                  <input
                    type="text"
                    {...register('picture')}
                    onChange={handleImageUrlChange}
                    placeholder="https://ejemplo.com/imagen.jpg"
                    disabled={isSubmitting}
                    className={`w-full px-4 py-3 border rounded-xl text-sm transition-all duration-200 ${errors.picture
                      ? 'border-red-500 focus:border-red-500 focus:ring-red-100'
                      : 'border-gray-300 focus:border-blue-500 focus:ring-blue-100'
                      }`}
                  />
                )}

                {errors.picture && (
                  <p className="mt-1 text-sm text-red-600">{errors.picture.message}</p>
                )}

                {imagePreview && (
                  <div className="mt-4 relative">
                    <div className="relative w-full h-48 rounded-lg overflow-hidden border border-gray-200 bg-gray-50">
                      <img
                        src={imagePreview}
                        alt="Preview"
                        className="w-full h-full object-cover"
                      />
                      <button
                        type="button"
                        onClick={handleRemoveImage}
                        className="absolute top-2 right-2 p-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-colors shadow-lg"
                      >
                        <svg
                          className="w-4 h-4"
                          fill="none"
                          stroke="currentColor"
                          viewBox="0 0 24 24"
                        >
                          <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth={2}
                            d="M6 18L18 6M6 6l12 12"
                          />
                        </svg>
                      </button>
                    </div>
                  </div>
                )}
              </div>

              {/* Botones */}
              <div className="flex gap-3 mt-6 pt-6 border-t border-gray-200">
                <button
                  type="submit"
                  disabled={isSubmitting}
                  className="flex-1 px-6 py-3 bg-blue-600 text-white rounded-xl font-medium shadow-lg hover:bg-blue-700 hover:shadow-xl transition-all duration-200"
                >
                  {isSubmitting ? 'Guardando...' : 'Crear Categoría'}
                </button>
                <button
                  type="button"
                  onClick={handleClose}
                  disabled={isSubmitting}
                  className="flex-1 px-6 py-3 bg-white text-gray-700 border border-gray-300 rounded-xl font-medium shadow-md hover:bg-gray-50 transition-all duration-200"
                >
                  Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
}
