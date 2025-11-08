import React from 'react';

interface LoadingButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    isLoading?: boolean;
    leftIcon?: React.ReactNode;
    rightIcon?: React.ReactNode;
    loadingPosition?: 'left' | 'right';
}

const Spinner = ({ size = 4 }: { size?: number }) => (
    <div className={`w-${size} h-${size} animate-spin`}>
        <svg className="w-full h-full" viewBox="0 0 24 24" fill="none">
            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
            <path
                className="opacity-75"
                fill="currentColor"
                d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
            />
        </svg>
    </div>
);

export default function LoadingButton({
    children,
    isLoading = false,
    leftIcon,
    rightIcon,
    loadingPosition = 'right',
    disabled,
    className = '',
    ...props
}: LoadingButtonProps) {
    return (
        <button
            disabled={isLoading || disabled}
            className={`px-6 py-3 bg-blue-600 text-white rounded-xl font-medium shadow-lg hover:bg-blue-700 hover:shadow-xl transition-all duration-200 transform hover:-translate-y-0.5 active:translate-y-0 flex items-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed ${className}`}
            {...props}
        >
            {loadingPosition === 'left' && isLoading ? <Spinner /> : leftIcon}
            {children}
            {loadingPosition === 'right' && isLoading ? <Spinner /> : rightIcon}
        </button>
    );
}
