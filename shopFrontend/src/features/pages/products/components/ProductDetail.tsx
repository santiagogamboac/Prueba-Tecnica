import type { Product } from '../../../../types';


interface ProductDetailProps {
  product: Product;
  onClose: () => void;
}

export default function ProductDetail({ product, onClose }: ProductDetailProps) {
  const displayImage = product.picture;

  return (
    <div
      className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4"
      onClick={onClose}
    >
      <div
        className="bg-white rounded-2xl w-full max-w-2xl max-h-[90vh] overflow-y-auto shadow-2xl"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="flex items-center justify-between p-6 border-b border-gray-200">
          <h2 className="text-2xl font-bold text-gray-900">{product.productName}</h2>
          <button
            className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-lg transition-colors"
            onClick={onClose}
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

        <div className="p-6">
          {displayImage && (
            <div className="w-full h-64 md:h-80 mb-6 rounded-xl overflow-hidden shadow-lg bg-gray-100">
              <img
                src={displayImage}
                alt={product.productName}
                loading="lazy"
                className="w-full h-full object-cover"
              />
            </div>
          )}

          <div className="mt-6">
            <h3 className="mb-4 text-lg font-semibold text-gray-900">Información General</h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="flex flex-col gap-2">
                <label className="text-xs font-bold text-gray-500 uppercase tracking-wider">
                  Categoría
                </label>
                <span className="text-sm font-medium text-gray-900">{product.categoryName}</span>
              </div>
              <div className="flex flex-col gap-2">
                <label className="text-xs font-bold text-gray-500 uppercase tracking-wider">
                  Precio
                </label>
                <span className="text-lg font-bold text-emerald-600">
                  ${product.unitPrice.toFixed(2)}
                </span>
              </div>
              <div className="flex flex-col gap-2">
                <label className="text-xs font-bold text-gray-500 uppercase tracking-wider">
                  Stock
                </label>
                <span
                  className={`text-sm font-semibold ${product.unitsInStock < 10 ? 'text-red-600' : 'text-gray-900'
                    }`}
                >
                  {product.unitsInStock} unidades
                </span>
              </div>
              <div className="flex flex-col gap-2">
                <label className="text-xs font-bold text-gray-500 uppercase tracking-wider">
                  Cantidad por Unidad
                </label>
                <span className="text-sm font-medium text-gray-900">
                  {product.quantityPerUnit || 'N/A'}
                </span>
              </div>
              <div className="flex flex-col gap-2">
                <label className="text-xs font-bold text-gray-500 uppercase tracking-wider">
                  Estado
                </label>
                <span
                  className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold w-fit ${product.discontinued
                    ? 'bg-red-100 text-red-800'
                    : 'bg-emerald-100 text-emerald-800'
                    }`}
                >
                  {product.discontinued ? 'Descontinuado' : 'Activo'}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
