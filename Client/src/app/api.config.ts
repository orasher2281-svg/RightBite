import { isDevMode } from '@angular/core';

export const API_URL = isDevMode()
  ? 'https://localhost:7231'
  : 'https://rightbight-api.onrender.com';