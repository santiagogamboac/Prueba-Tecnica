# Etapa 1: build con Node
FROM node:18-alpine AS build
WORKDIR /app

# Copia package.json y lock para instalar dependencias
COPY package*.json ./
RUN npm ci

# Copia código y genera el build de producción (Vite -> dist)
COPY . .
RUN npm run build

# Etapa 2: servidor estático (nginx)
FROM nginx:stable-alpine AS production
# Opcional: eliminar la configuración default si quieres
# COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copia la carpeta 'dist' generada al directorio servido por nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Exponer puerto 80 (el contenedor servirá en 80)
EXPOSE 80

# Ejecutar nginx en primer plano
CMD ["nginx", "-g", "daemon off;"]
