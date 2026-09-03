import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { UserService } from '../core/services/user-service';


export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token'); // שליפה ישירה מה-storage

  // אם זה נתיב של התחברות, אל תוסיפי טוקן!
  if (req.url.includes('/login') || req.url.includes('/register')) {
    return next(req);
  }

  if (token) {
    const authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
    return next(authReq);
  }

  return next(req);
};