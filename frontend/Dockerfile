# ---- Build stage ----
FROM node:22-alpine AS build
WORKDIR /app

COPY package.json package-lock.json* .npmrc* ./
# .npmrc sets legacy-peer-deps=true so installs succeed against bleeding-edge Angular.
# Use npm ci when a lockfile exists, otherwise fall back to npm install.
RUN if [ -f package-lock.json ]; then npm ci; else npm install; fi

COPY . .
RUN npm run build

# ---- Runtime stage ----
FROM nginx:1.27-alpine AS final
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/newton-gaming-station/browser /usr/share/nginx/html
EXPOSE 80
