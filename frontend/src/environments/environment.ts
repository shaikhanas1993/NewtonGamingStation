export const environment = {
  production: true,
  // In the Docker setup nginx proxies /api to the backend container,
  // so a relative base URL works in every environment.
  apiBaseUrl: '/api'
};
