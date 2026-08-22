import { HttpInterceptorFn } from '@angular/common/http';

export const tunnelInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    setHeaders: {
      'bypass-tunnel-reminder': 'true',
      'ngrok-skip-browser-warning': 'true'
    }
  });
  return next(clonedRequest);
};
