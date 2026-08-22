import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  return next(req).pipe(
    catchError((error) => {
      if (error) {
        if (error.status === 401) {
          router.navigateByUrl('/not-authenticated');
        } else if (error.status === 500) {
          const navigationExtras: NavigationExtras = {
            state: { error: error.error }
          };
          router.navigateByUrl('/server-error', navigationExtras);
        }
      }
      return throwError(() => error);
    })
  );
};
